using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TicketManagement.DesktopUI.Models;
using TicketManagement.DesktopUI.Services.Interfaces;

namespace TicketManagement.DesktopUI.Services
{
    public class VenueApiService : IVenueApiService
    {
        private readonly AuthenticatedUserModel authenticated;
        private readonly IConfiguration configuration;

        public VenueApiService(AuthenticatedUserModel authenticatedUser, IConfiguration configuration)
        {
            this.configuration = configuration;
            authenticated = authenticatedUser;
            InitializeClient();
        }

        private HttpClient ApiClient { get; set; }

        private void InitializeClient()
        {
            ApiClient = new HttpClient
            {
                BaseAddress = new Uri(configuration.GetValue<string>("venueApi"))
            };
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<VenueModel>> GetAllAsync()
        {
            using HttpResponseMessage response = await ApiClient.GetAsync("venues/getall").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<VenueModel>>(jsondata);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> AddVenueAsync(VenueModel venueModel)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("address", venueModel.Address),
                new KeyValuePair<string, string>("description", venueModel.Description),
                new KeyValuePair<string, string>("phone", venueModel.Phone),
            });
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticated.Token);
            using HttpResponseMessage response = await ApiClient.PostAsync("venues/create", formContent).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> UpdateVenueAsync(VenueModel model)
        {
            var formContent = new FormUrlEncodedContent(new[]
{
                new KeyValuePair<string, string>("id", model.Id.ToString()),
                new KeyValuePair<string, string>("description", model.Description),
                new KeyValuePair<string, string>("address", model.Address),
                new KeyValuePair<string, string>("phone", model.Phone),
            });
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticated.Token);
            using HttpResponseMessage response = await ApiClient.PutAsync("venues/update/", formContent).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> DeleteVenueAsync(int id)
        {
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticated.Token);
            using HttpResponseMessage response = await ApiClient.DeleteAsync("venues/delete/" + id).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
