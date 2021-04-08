using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TicketManagement.PurchaseAPI.Models;

namespace TicketManagement.PurchaseAPI.Services
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Dispose by using")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Bug", "S2931:Classes with \"IDisposable\" members should implement \"IDisposable\"", Justification = "Dispose by using")]
    /// <summary>
    /// Service for access to EventApi.
    /// </summary>
    public class EventApiService
    {
        private readonly IConfiguration _configuration;
        private HttpClient _httpClient;

        public EventApiService(IConfiguration config)
        {
            _configuration = config;
            InitializeClient();
        }

        private void InitializeClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configuration.GetValue<string>("eventApi")),
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Get seat from Api.
        /// </summary>
        /// <param name="id">Seat.</param>
        /// <param name="token">Token.</param>
        /// <returns>EventSeat.</returns>
        public async Task<EventSeatModel> GetSeatAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await _httpClient.GetAsync("events/seats/" + id);
            var data = await response.Content.ReadAsStringAsync();
            var eventSeat = JsonConvert.DeserializeObject<EventSeatModel>(data);
            return eventSeat;
        }

        /// <summary>
        /// Get Area from Api.
        /// </summary>
        /// <param name="id">Area.</param>
        /// <param name="token">Token.</param>
        public async Task<EventAreaModel> GetAreaAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await _httpClient.GetAsync("events/areas/" + id);
            var data = await response.Content.ReadAsStringAsync();
            var eventArea = JsonConvert.DeserializeObject<EventAreaModel>(data);
            return eventArea;
        }

        /// <summary>
        /// Get Event from Api.
        /// </summary>
        /// <param name="id">Event.</param>
        /// <param name="token">Token.</param>
        public async Task<EventModel> GetEventAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await _httpClient.GetAsync("events/" + id);
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<EventModel>(data);
        }

        /// <summary>
        /// Update seat info after changing state.
        /// </summary>
        /// <param name="model">Seat.</param>
        /// <param name="token">Token.</param>
        public async Task UpdateSeatAsync(EventSeatModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var formContent = new FormUrlEncodedContent(new[]
{
                new KeyValuePair<string, string>("id", model.Id.ToString()),
                new KeyValuePair<string, string>("eventareaid", model.EventAreaId.ToString()),
                new KeyValuePair<string, string>("number", model.Number.ToString()),
                new KeyValuePair<string, string>("row", model.Row.ToString()),
                new KeyValuePair<string, string>("State", ((int)model.State).ToString()),
});
            using var result = await _httpClient.PostAsync("events/seats/update", formContent);
        }
    }
}
