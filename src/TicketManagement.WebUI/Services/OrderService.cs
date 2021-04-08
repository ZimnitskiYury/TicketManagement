using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TicketManagement.WebUI.Models.Cart;
using TicketManagement.WebUI.Models.Order;

namespace TicketManagement.WebUI.Services
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Dispose by using")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Bug", "S2931:Classes with \"IDisposable\" members should implement \"IDisposable\"", Justification = "Dispose by using")]
    public class OrderService
    {
        private readonly IConfiguration _configuration;
        private HttpClient _httpClient;

        public OrderService(IConfiguration config)
        {
            _configuration = config;
            InitializeClient();
        }

        private void InitializeClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configuration.GetValue<string>("orderApi")),
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task ReserveSeatAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("id", id.ToString()),
            });
            using var response = await _httpClient.PostAsync("purchase/reserve/", formContent);
        }

        public async Task<PaymentViewModel> GetPaymentAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await _httpClient.GetAsync("purchase/payment/" + id);
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PaymentViewModel>(data);
        }

        public async Task PayAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("id", id.ToString()),
            });
            using var response = await _httpClient.PostAsync("purchase/payment/", formContent);
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrdersAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = "purchase/order/getall";
            using var result = await _httpClient.GetAsync(request);
            var jsonData = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<IEnumerable<OrderViewModel>>(jsonData);
            return model;
        }

        public async Task<OrderViewModel> GetOrderAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var result = await _httpClient.GetAsync("purchase/order/" + id);
            var jsonData = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<OrderViewModel>(jsonData);
            return model;
        }
    }
}
