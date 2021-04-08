using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TicketManagement.WebUI.Models.Account;
using TicketManagement.WebUI.Models.Profile;

namespace TicketManagement.WebUI.Services
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Dispose by using")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Bug", "S2931:Classes with \"IDisposable\" members should implement \"IDisposable\"", Justification = "Dispose by using")]
    public class UserService
    {
        private readonly IConfiguration _configuration;
        private HttpClient _httpClient;

        public UserService(IConfiguration config)
        {
            _configuration = config;
            InitializeClient();
        }

        private void InitializeClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configuration.GetValue<string>("userApi")),
            };
        }

        public async Task<ProfileViewModel> GetProfile(string login, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await _httpClient.GetAsync("users/profile/"+login);
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProfileViewModel>(data);
        }

        public async Task<int> EditProfile(ProfileViewModel model, string token)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("id", model.Id),
                new KeyValuePair<string, string>("login", model.Login),
                new KeyValuePair<string, string>("firstname", model.FirstName),
                new KeyValuePair<string, string>("surname", model.SurName),
                new KeyValuePair<string, string>("email", model.Email),
                new KeyValuePair<string, string>("language", model.Language),
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await _httpClient.PostAsync("users/profile/edit", formContent);
            if (response.IsSuccessStatusCode)
            {
                return 1;
            }

            return 0;
        }

        public async Task<HttpResponseMessage> Login(LoginViewModel model)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("login", model.Login),
                new KeyValuePair<string, string>("password", model.Password),
            });
            var result = await _httpClient.PostAsync("users/login", formContent);
            return result;
        }
    }
}
