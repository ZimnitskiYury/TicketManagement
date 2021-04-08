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
    public class EventApiService : IEventApiService
    {
        private readonly IConfiguration configuration;
        private readonly AuthenticatedUserModel authenticated;
        public EventApiService(AuthenticatedUserModel authenticatedUser, IConfiguration configuration)
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
                BaseAddress = new Uri(configuration.GetValue<string>("eventApi"))
            };
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<EventModel> GetEventAsync(int id)
        {
            using HttpResponseMessage response = await ApiClient.GetAsync("events/" + id);
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EventModel>(jsondata);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticated.Token);
            using HttpResponseMessage response = await ApiClient.DeleteAsync("events/delete/" + id).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> UpdateEventAsync(EventModel model)
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
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticated.Token);
            using HttpResponseMessage response = await ApiClient.PostAsync("events/update/", formContent).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<IEnumerable<EventModel>> GetAllAsync()
        {
            using HttpResponseMessage response = await ApiClient.GetAsync("events/getall").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<EventModel>>(jsondata);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> AddEventAsync(EventModel eventModel)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", eventModel.Name),
                new KeyValuePair<string, string>("description", eventModel.Description),
                new KeyValuePair<string, string>("category", eventModel.Category.ToString()),
                new KeyValuePair<string, string>("layoutid", eventModel.LayoutId.ToString()),
                new KeyValuePair<string, string>("startdatetime", eventModel.StartDateTime.ToString("dd/MM/yyyy HH:mm")),
                new KeyValuePair<string, string>("enddatetime", eventModel.EndDateTime.ToString("dd/MM/yyyy HH:mm")),
            });
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticated.Token);
            using HttpResponseMessage response = await ApiClient.PostAsync("events/create", formContent).ConfigureAwait(false);
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
