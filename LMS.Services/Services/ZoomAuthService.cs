using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LMS.Services.Services
{
    public class ZoomAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ZoomAuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<string> GetAccessTokenAsync()
        {
            // 1️⃣ Read config
            var accountId = _configuration["Zoom:AccountId"];
            var clientId = _configuration["Zoom:ClientId"];
            var clientSecret = _configuration["Zoom:ClientSecret"];

            // 2️⃣ Prepare basic auth header
            var authToken = Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")
            );

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", authToken);

            // 3️⃣ Prepare request
            var requestUrl =
                $"https://zoom.us/oauth/token?grant_type=account_credentials&account_id={accountId}";

            var response = await _httpClient.PostAsync(requestUrl, null);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to get Zoom access token");
            }

            // 4️⃣ Parse response
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            return doc.RootElement
                      .GetProperty("access_token")
                      .GetString()!;
        }
    }
}