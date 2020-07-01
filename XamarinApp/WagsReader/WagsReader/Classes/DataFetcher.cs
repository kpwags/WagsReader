using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace WagsReader.Classes
{
    public class DataFetcher
    {
        public static ReturnData GetData(string url, Dictionary<string, string> parameters)
        {
            ReturnData result = new ReturnData();

            //string urlParameters = BuildUrlParameters(parameters);

            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(url);

            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //HttpResponseMessage response = client.PostAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            //if (response.IsSuccessStatusCode)
            //{
            //    // Parse the response body.
            //    var dataObjects = response.Content.ReadAsAsync<IEnumerable<DataObject>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
            //    foreach (var d in dataObjects)
            //    {
            //        Console.WriteLine("{0}", d.Name);
            //    }
            //}
            //else
            //{
            //    result.Status = ReturnData.CallStatus.Error;
            //    result.Messages.Add(string.Format("{0} (StatusCode: {1}", response.StatusCode, response.ReasonPhrase));
            //}

            //client.Dispose();

            return result;
        }

        private static string BuildUrlParameters(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                return "";
            }

            StringBuilder paramString = new StringBuilder("?");

            foreach (KeyValuePair<string, string> param in parameters)
            {
                paramString.AppendFormat("{0}={1}", param.Key, param.Value);
            }

            return paramString.ToString();
        }
    }
}
