using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Globalization;

namespace HisaabPlus.Web.Services
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<T> GetAsync<T>(string endpoint, string token)
        {
            var client = _httpClientFactory.CreateClient("HisaabPlusApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();
            if(response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? throw new Exception("Failed to deserialize response");
            }
            throw new Exception(content);
        }
        public async Task<T> PostAsync<T>(string endpoint, object data, string token) 
        {
            var client = _httpClientFactory.CreateClient("HisaabPlusApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var serializeData = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(serializeData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(endpoint, stringContent);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? throw new Exception("Failed to deserialize response");
            }
            throw new Exception(content);
        }
        public async Task<T> PutAsync<T>(string endpoint, object data, string token)
        {
            var client = _httpClientFactory.CreateClient("HisaabPlusApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var serializeData = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(serializeData, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(endpoint, stringContent);
            var content = await response.Content.ReadAsStringAsync();
            if(response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? throw new Exception("Failed to deserialize response");
            }
            throw new Exception(content);
        }
        public async Task<bool> DeleteAsync(string endpoint, string token)
        {
            var client = _httpClientFactory.CreateClient("HisaabPlusApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.DeleteAsync(endpoint);
            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            throw new Exception("Response is not valid!");
        }
    }
}

