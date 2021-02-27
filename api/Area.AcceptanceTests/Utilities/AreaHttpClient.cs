using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Area.AcceptanceTests.Models;
using Area.AcceptanceTests.Models.Responses;
using Newtonsoft.Json;

namespace Area.AcceptanceTests.Utilities
{
    public class AreaHttpClient
    {
        private readonly HttpClient _client;

        public AreaHttpClient(string baseAddress)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.AllowAutoRedirect = false;
            _client = new HttpClient(httpClientHandler) {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public void SetBearer(string? value)
        {
            if (value == null)
                _client.DefaultRequestHeaders.Remove("Authorization");
            else
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", value);
        }

        public async Task<HttpResponseMessage> RawPostAsync<TRequest>(string endpoint, TRequest form)
        {
            var serializedForm = JsonConvert.SerializeObject(form);
            return await _client.PostAsync(endpoint,
                new StringContent(serializedForm, Encoding.UTF8, "application/json"));
        }

        public async Task<ResponseHolder<TResponse>> PostAsync<TResponse, TRequest>(string endpoint, TRequest form)
            where TResponse : StatusModel
        {
            var response = await RawPostAsync(endpoint, form);

            return new ResponseHolder<TResponse> {
                Content = JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync()),
                Status = response.StatusCode
            };
        }

        public async Task<ResponseHolder<TResponse>> PostAsync<TResponse>(string endpoint)
            where TResponse : StatusModel => await PostAsync<TResponse, object>(endpoint, new object());

        public async Task<ResponseHolder<StatusModel>> PostAsync(string endpoint) =>
            await PostAsync<StatusModel, object>(endpoint, new object());

        public async Task<ResponseHolder<StatusModel>> PostAsync<TRequest>(string endpoint, TRequest form) =>
            await PostAsync<StatusModel, TRequest>(endpoint, form);

        public async Task<ResponseHolder<TResponse>> GetAsync<TResponse>(string endpoint)
            where TResponse : StatusModel
        {
            var response = await _client.GetAsync(endpoint);

            return new ResponseHolder<TResponse> {
                Content = JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync()),
                Status = response.StatusCode
            };
        }

        public async Task<ResponseHolder<StatusModel>> GetAsync(string endpoint) =>
            await GetAsync<StatusModel>(endpoint);

        public async Task<ResponseHolder<TResponse>> DeleteAsync<TResponse>(string endpoint)
            where TResponse : StatusModel
        {
            var response = await _client.DeleteAsync(endpoint);

            return new ResponseHolder<TResponse> {
                Content = JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync()),
                Status = response.StatusCode
            };
        }

        public async Task<ResponseHolder<StatusModel>> DeleteAsync(string endpoint) =>
            await DeleteAsync<StatusModel>(endpoint);
    }
}