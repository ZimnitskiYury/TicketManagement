using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TicketManagement.WebUI.Models.Event;

namespace TicketManagement.WebUI.Services
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Dispose by using")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Bug", "S2931:Classes with \"IDisposable\" members should implement \"IDisposable\"", Justification = "Dispose by using")]
    public class EventService
    {
        private readonly IConfiguration _configuration;
        private HttpClient _httpClient;

        public EventService(IConfiguration config)
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
        }

        public async Task<List<EventAreaViewModel>> GetAreas(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await _httpClient.GetAsync("events/" + id + "/prices");
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<EventAreaViewModel>>(data);
        }

        public async Task<EventSeatViewModel> GetSeatAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await _httpClient.GetAsync("events/seats/" + id);
            var data = await response.Content.ReadAsStringAsync();
            var eventSeat = JsonConvert.DeserializeObject<EventSeatViewModel>(data);
            return eventSeat;
        }

        public async Task<EventAreaViewModel> GetAreaAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await _httpClient.GetAsync("events/areas/" + id);
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<EventAreaViewModel>(data);
        }

        public async Task<List<EventSeatViewModel>> GetSeatsAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await _httpClient.GetAsync("events/" + id + "/seats");
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<EventSeatViewModel>>(data);
        }

        public async Task UpdatePriceAsync(EventAreaViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var formContent = new FormUrlEncodedContent(new[]
{
                new KeyValuePair<string, string>("id", model.Id.ToString()),
                new KeyValuePair<string, string>("description", model.Description),
                new KeyValuePair<string, string>("eventid", model.EventId.ToString()),
                new KeyValuePair<string, string>("coordx", model.CoordX.ToString()),
                new KeyValuePair<string, string>("coordy", model.CoordY.ToString()),
                new KeyValuePair<string, string>("price", model.Price.ToString()),
});
            using var response = await _httpClient.PostAsync("events/prices/update/", formContent);
        }

        public async Task<List<EventDataViewModel>> GetAllAsync()
        {
            using var response = await _httpClient.GetAsync("events/getall");
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<EventDataViewModel>>(data);
        }

        public async Task<EventDataViewModel> GetEventAsync(int id)
        {
            var request = "events/" + id;
            using var response = await _httpClient.GetAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<EventDataViewModel>(data);
        }

        public async Task<string> CreateEventAsync(EventDataViewModel model, string token)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", model.Name),
                new KeyValuePair<string, string>("description", model.Description),
                new KeyValuePair<string, string>("category", model.Category.ToString()),
                new KeyValuePair<string, string>("layoutid", model.LayoutId.ToString()),
                new KeyValuePair<string, string>("startdatetime", model.StartDateTime.ToString("dd/MM/yyyy HH:mm")),
                new KeyValuePair<string, string>("enddatetime", model.EndDateTime.ToString("dd/MM/yyyy HH:mm")),
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = "events/create";
            using var result = await _httpClient.PostAsync(request, formContent);
            if (result.IsSuccessStatusCode)
            {
                var idevent = await result.Content.ReadAsStringAsync();
                return idevent;
            }

            return "0";
        }

        public async Task<int> UpdateEventAsync(EventDataViewModel model, string token)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("id", model.Id.ToString()),
                new KeyValuePair<string, string>("name", model.Name),
                new KeyValuePair<string, string>("description", model.Description),
                new KeyValuePair<string, string>("category", model.Category.ToString()),
                new KeyValuePair<string, string>("layoutid", model.LayoutId.ToString()),
                new KeyValuePair<string, string>("startdatetime", model.StartDateTime.ToString("dd/MM/yyyy HH:mm")),
                new KeyValuePair<string, string>("enddatetime", model.EndDateTime.ToString("dd/MM/yyyy HH:mm")),
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = "events/update";
            using var result = await _httpClient.PostAsync(request, formContent);
            if (result.IsSuccessStatusCode)
            {
                return model.Id;
            }

            return 0;
        }

        public async Task<int> DeleteEventAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = "events/delete/" + id;
            using var result = await _httpClient.DeleteAsync(request);
            if (result.IsSuccessStatusCode)
            {
                return 1;
            }

            return 0;
        }
    }
}
