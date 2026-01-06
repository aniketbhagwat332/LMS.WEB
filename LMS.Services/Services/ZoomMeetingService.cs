using LMS.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LMS.Services.Services
{
    public class ZoomMeetingService
    {
        private readonly ZoomAuthService _zoomAuthService;
        private readonly HttpClient _httpClient;

        public ZoomMeetingService(ZoomAuthService zoomAuthService)
        {
            _zoomAuthService = zoomAuthService;
            _httpClient = new HttpClient();
        }

        public async Task<ZoomMeetingResult> CreateMeetingAsync(
            string topic,
            DateTime startTime,
            int durationMinutes)
        {
            // 1️⃣ Get access token
            var accessToken = await _zoomAuthService.GetAccessTokenAsync();

            // 2️⃣ Prepare request
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var payload = new
            {
                topic = topic,
                type = 2, // scheduled meeting
                start_time = startTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"),
                duration = durationMinutes,
                settings = new
                {
                    join_before_host = false,
                    waiting_room = true
                }
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // 3️⃣ Call Zoom API
            var response = await _httpClient.PostAsync(
                "https://api.zoom.us/v2/users/me/meetings",
                content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to create Zoom meeting");
            }

            // 4️⃣ Parse response
            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);

            return new ZoomMeetingResult
            {
                MeetingId = doc.RootElement.GetProperty("id").GetRawText(),
                JoinUrl = doc.RootElement.GetProperty("join_url").GetString(),
                StartUrl = doc.RootElement.GetProperty("start_url").GetString()
            };
        }
    }
}
