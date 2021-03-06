﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WagsReaderLibrary.Exceptions;

namespace WagsReaderLibrary
{
    public class ApiUtilities
    {
        public static HttpContent GetRequestContentAsJson(object requestParameters)
        {
            return new StringContent(JsonConvert.SerializeObject(requestParameters), System.Text.Encoding.UTF8, "application/json");
        }

        public static HttpContent GetRequestContentAsDictionary(object requestParameters)
        {
            var parameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(requestParameters));

            var content = new FormUrlEncodedContent(parameters);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            return content;
        }

        public static async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = "";

                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    var respJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                    if (respJson.ContainsKey("error_description"))
                    {
                        errorMessage = respJson["error_description"];
                    }
                    else if (respJson.ContainsKey("error_message"))
                    {
                        errorMessage = respJson["error_message"];
                    }
                    else if (respJson.ContainsKey("error"))
                    {
                        errorMessage = respJson["error"];
                    }
                }
                catch
                {
                    errorMessage = string.Empty;
                }                

                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(content, errorMessage);
                }

                throw new ApiRequestExceptionException(response.StatusCode, errorMessage, content);
            }
        }
    }
}
