using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Area.AcceptanceTests.Utilities
{
    public class ApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(HttpClient client)
        {
            _client = client;
        }

        public void SetBearer(string? value)
        {
            if (value == null)
                _client.DefaultRequestHeaders.Remove("Authorization");
            else
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", value);
        }

        public async Task<ApiResponse<TResult>> PostAsync<TResult, TRequest>(string endpoint, TRequest form)
        {
            var serializedForm = JsonConvert.SerializeObject(form);
            var response = await _client.PostAsync(endpoint, new StringContent(serializedForm, Encoding.UTF8, "application/json"));

            return new ApiResponse<TResult> {
                Data = JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync()),
                Status = response.StatusCode
            };
        }

        public async Task<ApiResponse<TResult>> GetAsync<TResult>(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);

            return new ApiResponse<TResult> {
                Data = JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync()),
                Status = response.StatusCode
            };
        }

        public async Task<ApiResponse<TResult>> DeleteAsync<TResult>(string endpoint)
        {
            var response = await _client.DeleteAsync(endpoint);

            return new ApiResponse<TResult> {
                Data = JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync()),
                Status = response.StatusCode
            };
        }
    }
}