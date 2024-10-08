﻿using Microsoft.Extensions.Options;
using Outbound_company.Models;
using Outbound_company.Repository.Interface;
using Outbound_company.Services.Interfaces;
using Serilog;
using System.Net.Http.Headers;
using System.Text;

namespace Outbound_company.Services
{
    public class CallsManagementService : ICallsManagementService
    {
        private readonly IAsteriskCountOfCallsService _countOfCallsService;
        private readonly AsteriskSettings _asteriskSettings;
        private readonly IServiceProvider _serviceProvider;
        private static readonly HttpClient client = new HttpClient();
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);
        private CancellationTokenSource _cts;
        private Task _backgroundTask;

        public CallsManagementService(IOptions<AsteriskSettings> asteriskSettings, IAsteriskCountOfCallsService countOfCallsService, IServiceProvider serviceProvider)
        {
            _asteriskSettings = asteriskSettings.Value ?? throw new ArgumentNullException(nameof(asteriskSettings.Value));
            _countOfCallsService = countOfCallsService ?? throw new ArgumentNullException(nameof(countOfCallsService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void Start(OutboundCompany company, NumberPool numberPool, IEnumerable<BlackListNumber> blackListNumber, int maximumCountOfCalls)
        {
            _cts = null;
            _cts = new CancellationTokenSource();
            _backgroundTask = Task.Run(() => Call(company, numberPool, blackListNumber, _cts.Token, maximumCountOfCalls));
        }

        public void Stop()
        {
            if (_cts == null) return;

            _cts.Cancel();
            try
            {
                _backgroundTask.Wait();
            }
            catch (AggregateException ex)
            {
                ex.Handle(e => e is TaskCanceledException);
            }
            finally
            {
                _cts = null;
            }
        }

        private async Task Call(OutboundCompany company, NumberPool numberPool, IEnumerable<BlackListNumber> blackListNumber, CancellationToken cancellationToken, int maximumCountOfCalls)
        {
            Log.Information($"Starting call process for company {company.Id} with {numberPool.PhoneNumbers.Count} numbers in pool.");

            foreach (var phoneNumber in numberPool.PhoneNumbers)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Log.Information($"Call process for company {company.Id} was cancelled.");
                    break;
                }

                if (blackListNumber.Any(b => b.Number == phoneNumber.Number))
                {
                    await RecordCallStatisticsAsync(company.Id, phoneNumber.Id, 2);
                    continue;
                }

                int activeCalls = await _countOfCallsService.GetActiveCallsAsync();

                while (activeCalls >= maximumCountOfCalls)
                {
                    await Task.Delay(_interval, cancellationToken);
                    activeCalls = await _countOfCallsService.GetActiveCallsAsync();
                }

                var content = CreateRequestContent(company, phoneNumber.Number);
                var success = await SendHttpRequestAsync(content);

                await RecordCallStatisticsAsync(company.Id, phoneNumber.Id, success ? 1 : 3);

                if (!success)
                {
                    Log.Error($"Call to {phoneNumber.Number} failed for company {company.Id}. Aborting call process.");
                    break;
                }
            }
            Log.Information($"Completed call process for company {company.Id}.");
        }

        private StringContent CreateRequestContent(OutboundCompany company, string phoneNumber)
        {
            var endpoint = $"{company.TrunkType}/{phoneNumber}@{company.Channel}";

            var jsonContent = new
            {
                endpoint = endpoint,
                extension = company.Extension,
                context = company.Context,
                callerId = company.CallerId
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(jsonContent), Encoding.UTF8, "application/json");
            return content;
        }

        private async Task<bool> SendHttpRequestAsync(StringContent content)
        {
           // return true;
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_asteriskSettings.Username}:{_asteriskSettings.Password}"));
            var requestUri = $"http://{_asteriskSettings.Url}/ari/channels";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var response = await client.PostAsync(requestUri, content);
            return response.IsSuccessStatusCode;
        }

        private async Task RecordCallStatisticsAsync(int companyId, int phoneNumberId, int statusId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedCallStatisticsRepository = scope.ServiceProvider.GetRequiredService<ICallStatisticsRepository>();

                var callStatistics = new CallStatistics
                {
                    CompanyId = companyId,
                    PhoneNumberId = phoneNumberId,
                    CallStatusId = statusId,  // 1 - Успешно, 3 - Ошибка HTTP
                    CallTime = DateTime.UtcNow
                };

                await scopedCallStatisticsRepository.AddAsync(callStatistics);
            }
        }
    }
}
