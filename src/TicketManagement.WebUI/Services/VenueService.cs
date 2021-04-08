using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TicketManagement.WebUI.Models.Venue;

namespace TicketManagement.WebUI.Services
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Dispose by using")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Bug", "S2931:Classes with \"IDisposable\" members should implement \"IDisposable\"", Justification = "Dispose by using")]
    public class VenueService
    {
        private readonly IConfiguration _configuration;
        private HttpClient _httpClient;

        public VenueService(IConfiguration config)
        {
            _configuration = config;
            InitializeClient();
        }

        private void InitializeClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configuration.GetValue<string>("venueApi")),
            };
        }

        public async Task<List<VenueViewModel>> GetAllAsync()
        {
            using var response = await _httpClient.GetAsync("venues/getall");
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<VenueViewModel>>(data);
        }

        public async Task<VenueViewModel> GetVenueAsync(int id)
        {
            var request = "venues/"+id;
            using var response = await _httpClient.GetAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<VenueViewModel>(data);
        }
    }
}
