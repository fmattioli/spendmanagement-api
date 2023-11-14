using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SpendManagement.Integration.Tests.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SpendManagement.Integration.Tests.Tests
{
    public class BaseTests<T> where T : class
    {
        private const string APIVersion = "api/v1";
        private readonly HttpClient _httpClient;

        public BaseTests()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        protected async Task<HttpResponseMessage?> PostAsync(string resource, T body)
        {
            var json = JsonConvert.SerializeObject(body);
            StringContent httpContent = new(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BaseTests<T>.GenerateJWToken());
            var url = APIVersion + resource;

            return await _httpClient.PostAsync(url, httpContent);
        }

        private static string GenerateJWToken()
        {
            var settings = TestSettings.JwtOptions;
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.SecurityKey ?? throw new Exception("Invalid token security key")));

            var claims = GenerateClaims();

            var credenciais = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                settings.Issuer,
                settings.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(settings.AccessTokenExpiration),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static IEnumerable<Claim> GenerateClaims()
        {
            return new List<Claim>
            {
                new Claim(Application.Claims.ClaimTypes.Receipt, "Read"),
                new Claim(Application.Claims.ClaimTypes.Receipt, "Insert"),
                new Claim(Application.Claims.ClaimTypes.Receipt, "Update"),
                new Claim(Application.Claims.ClaimTypes.Receipt, "Delete"),
                new Claim(Application.Claims.ClaimTypes.Category, "Read"),
                new Claim(Application.Claims.ClaimTypes.Category, "Insert"),
                new Claim(Application.Claims.ClaimTypes.Category, "Update"),
                new Claim(Application.Claims.ClaimTypes.Category, "Delete"),
            };
        }
    }
}
