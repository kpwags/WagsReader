using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using WagsReaderLibrary;
using WagsReaderLibrary.Interfaces;
using WagsReaderLibrary.Json;

namespace WagsReader.Services
{
    public class RequestProvider : IRequestProvider
    {
        HttpClient _client;
        readonly JsonSerializerSettings _serializerSettings;

        public RequestProvider(string baseAddress = "")
        {
            _client = CreateHttpClient(string.Empty);

            if (baseAddress != "")
            {
                _client.BaseAddress = new Uri(baseAddress);
            }

            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new StandardContractResolver(true),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public async Task<string> GetAsync(string uri, string token = "")
        {
            _client.SetBearerToken(token);
            HttpResponseMessage response = await _client.GetAsync(uri);

            await ApiUtilities.HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();
            return serialized;
        }

        public async Task<TResult> PostAsync<TResult>(string uri, HttpContent content, string clientId = "", string clientSecret = "")
        {
            if (!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret))
            {
                AddBasicAuthenticationHeader(clientId, clientSecret);
            }
            
            HttpResponseMessage response = await _client.PostAsync(uri, content);

            await ApiUtilities.HandleResponse(response);

            string serialized = await response.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine($"Response: {serialized}");

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        HttpClient CreateHttpClient(string token = "")
        {
            _client = new HttpClient
            (
                new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                    {
                        //bypass
                        return true;
                    },
                },
                false
            );

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return _client;
        }

        void AddBasicAuthenticationHeader(string clientId, string clientSecret)
        {
            if (_client == null)
                return;

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                return;

            _client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(clientId, clientSecret);
        }
    }
}
