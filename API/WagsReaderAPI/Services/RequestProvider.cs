using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using WagsReaderLibrary;
using WagsReaderLibrary.Exceptions;
using WagsReaderLibrary.Interfaces;

namespace WagsReaderAPI.Services
{
    public class RequestProvider : IRequestProvider
    {
        HttpClient _client;
        readonly JsonSerializerSettings _serializerSettings;

        public RequestProvider()
        {
            _client = CreateHttpClient(string.Empty);

            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<string> GetAsync(string uri, string token = "")
        {
            _client.SetBearerToken(token);
            HttpResponseMessage response = await _client.GetAsync(uri);

            await ApiUtilities.HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();
            return serialized;
        }

        public async Task<TResult> PostAsync<TResult>(string uri, HttpContent content, string contentType = "application/x-www-form-urlencoded", string clientId = "", string clientSecret = "")
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret))
                {
                    AddBasicAuthenticationHeader(clientId, clientSecret);
                }

                if (contentType != null)
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                }

                HttpResponseMessage response = await _client.PostAsync(uri, content);

                await ApiUtilities.HandleResponse(response);
                string serialized = await response.Content.ReadAsStringAsync();

                TResult result = await Task.Run(() =>
                    JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

                return result;
            }
            catch (ServiceAuthenticationException ex)
            {
                throw new ServiceAuthenticationException(ex.Content, ex.AuthErrorMessage);
            }
            catch (ApiRequestExceptionException ex)
            {
                throw new ApiRequestExceptionException(ex.HttpCode, ex.Message, ex.Content);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        HttpClient CreateHttpClient(string token = "")
        {
            _client = new HttpClient();
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
