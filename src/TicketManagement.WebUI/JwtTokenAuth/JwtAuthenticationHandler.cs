using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TicketManagement.WebUI.JwtTokenAuth
{
    public class JwtAuthenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
    {
        private readonly IConfiguration _configuration;

        public JwtAuthenticationHandler(
            IOptionsMonitor<JwtAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock, IConfiguration config)
            : base(options, logger, encoder, clock)
        {
            _configuration = config;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = Context.Request.Cookies["secret_jwt_key"];
            if (token != null)
            {
                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(_configuration.GetValue<string>("userApi")),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.GetAsync($"users/validate?token={token}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);
                    var identity = new ClaimsIdentity(jwtToken.Claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
            }

            return AuthenticateResult.Fail("Unauthorized");
        }
    }
}
