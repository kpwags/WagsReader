using Newtonsoft.Json;

namespace WagsReaderLibrary
{
    public class ApiResponse<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("error")]
        public string ErrorMessage { get; set; } = "";
    }
}
