using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SpendManagement.Integration.Tests.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SpendManagement.Integration.Tests.Constants;
using Flurl;

namespace SpendManagement.Integration.Tests.Helpers
{
    public class BaseTests<T> where T : class
    {
        private readonly HttpClient _httpClient;

        public BaseTests()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Tests");
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        protected async Task<HttpResponseMessage?> PostAsync(string resource, T body)
        {
            var json = JsonConvert.SerializeObject(body);
            StringContent httpContent = new(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJWToken());
            var url = ConstantsValues.APIVersion + resource;
            using var response = await _httpClient.PostAsync(url, httpContent);
            return response;
        }

        protected async Task<HttpResponseMessage?> DeleteAsync(string resource, Guid id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJWToken());

            var url = ConstantsValues.APIVersion
                .AppendPathSegment(resource)
                .AppendPathSegment(id);

            using var response = await _httpClient.DeleteAsync(url);
            return response;
        }

        protected async Task<HttpResponseMessage?> PatchAsync(string resource, Guid id, string jsonPatch)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BaseTests<T>.GenerateJWToken());

            var httpContent = new StringContent(jsonPatch, Encoding.UTF8, "application/json");

            var url = ConstantsValues.APIVersion
                .AppendPathSegment(resource)
                .AppendPathSegment(id);

            using var response = await _httpClient.PatchAsync(url, httpContent);
            return response;
        }

        private static string GenerateJWToken()
        {
            var settings = TestSettings.JwtOptions;
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings!.SecurityKey ?? throw new Exception("Invalid token security key")));

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

        private static List<Claim> GenerateClaims()
        {
            return
            [
                new(Application.Claims.ClaimTypes.Receipt, "Read"),
                new(Application.Claims.ClaimTypes.Receipt, "Insert"),
                new(Application.Claims.ClaimTypes.Receipt, "Update"),
                new(Application.Claims.ClaimTypes.Receipt, "Delete"),
                new(Application.Claims.ClaimTypes.Category, "Read"),
                new(Application.Claims.ClaimTypes.Category, "Insert"),
                new(Application.Claims.ClaimTypes.Category, "Update"),
                new(Application.Claims.ClaimTypes.Category, "Delete"),
            ];
        }
    }
}
