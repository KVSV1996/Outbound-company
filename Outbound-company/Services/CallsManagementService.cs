using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Outbound_company.Models;
using Outbound_company.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace Outbound_company.Services
{
    public class CallsManagementService : ICallsManagementService
    {
        private readonly IAsteriskCountOfCallsService _countOfCallsService;
        //private readonly ICompaniesService _companiesService;
        private readonly AsteriskSettings _asteriskSettings;
       // private readonly INumberService _numberService;
        private static readonly HttpClient client = new HttpClient();
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);
        private CancellationTokenSource _cts;
        private Task _backgroundTask;


        public CallsManagementService(IOptions<AsteriskSettings> asteriskSettings, IAsteriskCountOfCallsService countOfCallsService)
        {
            //_numberService = numberService ?? throw new ArgumentNullException(nameof(numberService));
            //_companiesService = companiesService ?? throw new ArgumentNullException(nameof(companiesService));
            _asteriskSettings = asteriskSettings.Value ?? throw new ArgumentNullException(nameof(asteriskSettings.Value));
            _countOfCallsService = countOfCallsService ?? throw new ArgumentNullException(nameof(countOfCallsService));
        }

        public void Start(OutboundCompany company, NumberPool numberPool)
        {
            if (_cts != null) return;

            _cts = new CancellationTokenSource();
            //_backgroundTask = Task.Run(() => CallManagerAsync(id, _cts.Token));
            _backgroundTask = Task.Run(() => Call(company, numberPool, _cts.Token));
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

        //private async Task CallManagerAsync(int id, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var company = _companiesService.GetCompanyById(id);
        //        if (company == null)
        //        {
        //            throw new NullReferenceException();;
        //        }

        //        var numberPool = _numberService.GetById(company.NumberPoolId);
        //        if (numberPool == null)
        //        {
        //            throw new NullReferenceException();
        //        }

        //        foreach (var phoneNumber in numberPool.PhoneNumbers)
        //        {
        //            if (cancellationToken.IsCancellationRequested)
        //            {
        //                break;
        //            }

        //            int activeCalls = await _countOfCallsService.GetActiveCallsAsync();

        //            while (activeCalls >= 3)
        //            {
        //                await Task.Delay(_interval, cancellationToken);
        //                activeCalls = await _countOfCallsService.GetActiveCallsAsync();
        //            }

        //            var content = CreateRequestContent(company, phoneNumber.Number);
        //            var success = await SendHttpRequestAsync(content);

        //            if (!success)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception ex){}
        //}

        private async Task Call(OutboundCompany company, NumberPool numberPool, CancellationToken cancellationToken)
        {
            foreach (var phoneNumber in numberPool.PhoneNumbers)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                int activeCalls = await _countOfCallsService.GetActiveCallsAsync();

                while (activeCalls >= 3)
                {
                    await Task.Delay(_interval, cancellationToken);
                    activeCalls = await _countOfCallsService.GetActiveCallsAsync();
                }

                var content = CreateRequestContent(company, phoneNumber.Number);
                //var endpoint = $"{company.TrunkType}/{phoneNumber.Number}@{company.Channel}";

                //var jsonContent = new
                //{
                //    endpoint = endpoint,
                //    extension = company.Extension,
                //    context = company.Context,
                //    callerId = company.CallerId
                //};

                //var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(jsonContent), Encoding.UTF8, "application/json");
                 var success = await SendHttpRequestAsync(content);
                //var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_asteriskSettings.Username}:{_asteriskSettings.Password}"));
                //var requestUri = $"http://{_asteriskSettings.Url}/ari/channels";
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                //var response = await client.PostAsync(requestUri, content);
                //return response.IsSuccessStatusCode;

                if (!success)
                {
                    break;
                }
            }
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
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_asteriskSettings.Username}:{_asteriskSettings.Password}"));
            var requestUri = $"http://{_asteriskSettings.Url}/ari/channels";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var response = await client.PostAsync(requestUri, content);
            return response.IsSuccessStatusCode;
        }
    }
}
