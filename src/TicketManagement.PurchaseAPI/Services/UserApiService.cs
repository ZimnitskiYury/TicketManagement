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
    /// Service for connect to UserApi.
    /// </summary>
    public class UserApiService
    {
        private readonly IConfiguration _configuration;
        private HttpClient _httpClient;

        public UserApiService(IConfiguration config)
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
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Get user info.
        /// </summary>
        /// <param name="login">Login for getting info.</param>
        /// <param name="token">Token.</param>
        /// <returns>UserModel.</returns>
        public async Task<UserModel> GetUser(string login, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("users/profile/" + login);
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserModel>(data);
        }

        /// <summary>
        /// Update Balance after buying ticket.
        /// </summary>
        /// <param name="model">User model to api for changing.</param>
        /// <param name="token">Token.</param>
        public async Task UpdateBalance(UserModel model, string token)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("id", model.Id),
                new KeyValuePair<string, string>("login", model.Login),
                new KeyValuePair<string, string>("firstname", model.FirstName),
                new KeyValuePair<string, string>("surname", model.SurName),
                new KeyValuePair<string, string>("email", model.Email),
                new KeyValuePair<string, string>("language", model.Language),
                new KeyValuePair<string, string>("balance", model.Balance.ToString()),
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            await _httpClient.PostAsync("users/profile/edit", formContent);
        }
    }
}
