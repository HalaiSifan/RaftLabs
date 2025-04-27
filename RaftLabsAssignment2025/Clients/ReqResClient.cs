using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Polly;
using RaftLabsAssignment2025.Configuration;
using RaftLabsAssignment2025.Models;
using System.Text.Json;

namespace RaftLabsAssignment2025.Clients
{
    public class ReqResClient : IReqResClient
    {
        private readonly HttpClient _httpClient;
        private readonly ReqResApiOptions _options;
        private readonly IMemoryCache _cache;

        public ReqResClient(HttpClient httpClient, IOptions<ReqResApiOptions> options, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _cache = cache;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var cacheKey = $"user_{userId}";
            if (_cache.TryGetValue(cacheKey, out User cachedUser))
                return cachedUser;

            var response = await ExecuteWithRetryAsync(() => _httpClient.GetAsync($"{_options.BaseUrl}/users/{userId}"));
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to fetch user with status code {response.StatusCode}");

            var content = await response.Content.ReadAsStringAsync();
            var document = JsonDocument.Parse(content);
            var user = JsonSerializer.Deserialize<User>(document.RootElement.GetProperty("data").GetRawText());

            _cache.Set(cacheKey, user, TimeSpan.FromMinutes(5));
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var cacheKey = "all_users";
            if (_cache.TryGetValue(cacheKey, out List<User> cachedUsers))
                return cachedUsers;

            var allUsers = new List<User>();
            int page = 1;
            int totalPages;

            do
            {
                var response = await ExecuteWithRetryAsync(() => _httpClient.GetAsync($"{_options.BaseUrl}/users?page={page}"));
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Failed to fetch users page {page} with status code {response.StatusCode}");

                var content = await response.Content.ReadAsStringAsync();
                var pageData = JsonSerializer.Deserialize<UserListResponse>(content);

                allUsers.AddRange(pageData.Data);
                totalPages = pageData.TotalPages;
                page++;
            } while (page <= totalPages);

            _cache.Set(cacheKey, allUsers, TimeSpan.FromMinutes(10));
            return allUsers;
        }

        private async Task<HttpResponseMessage> ExecuteWithRetryAsync(Func<Task<HttpResponseMessage>> action)
        {
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            return await policy.ExecuteAsync(action);
        }
    }
}
