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
    public class UserApiService : IUserApiService
    {
        private readonly IConfiguration configuration;
        private string token;
        private readonly AuthenticatedUserModel authenticated;

        public UserApiService(AuthenticatedUserModel authenticatedUser, IConfiguration configuration)
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
                BaseAddress = new Uri(configuration.GetValue<string>("userApi"))
            };
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<ProfileModel>> GetAllAsync()
        {
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticated.Token);
            using HttpResponseMessage response = await ApiClient.GetAsync("users").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ProfileModel>>(jsondata);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<IEnumerable<RoleModel>> GetRolesAsync()
        {
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticated.Token);
            using HttpResponseMessage response = await ApiClient.GetAsync("users/roles").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<RoleModel>>(jsondata);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> AddRoleAsync(string login, string role)
        {
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticated.Token);
            var requestUri = "users/" + login + "/roles/" + role;
            using HttpResponseMessage response = await ApiClient.GetAsync(requestUri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> DeleteRoleAsync(string login, string role)
        {
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticated.Token);
            var requestUri = "users/" + login + "/roles/" + role;
            using HttpResponseMessage response = await ApiClient.DeleteAsync(requestUri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> UpdateUserAsync(ProfileModel model)
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
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticated.Token);
            using HttpResponseMessage response = await ApiClient.PostAsync("users/profile/edit", formContent).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> DeleteUserAsync(string login)
        {
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticated.Token);
            using HttpResponseMessage response = await ApiClient.DeleteAsync("users/delete/" + login).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<AuthenticatedUserModel> AuthenticateAsync(string login, string password)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("login", login),
                new KeyValuePair<string, string>("password", password),
            });
            using (HttpResponseMessage response = await ApiClient.PostAsync("users/login", formContent))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Get Token by login+pass
                    token = await response.Content.ReadAsStringAsync();
                    token = token.Trim('"');
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using (HttpResponseMessage response = await ApiClient.GetAsync("users/profile/" + login))
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<AuthenticatedUserModel>(data);
                    user.Token = token;
                    return user;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }
    }
}
