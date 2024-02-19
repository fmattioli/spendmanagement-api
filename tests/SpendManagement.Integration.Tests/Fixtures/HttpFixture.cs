using Flurl;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SpendManagement.Integration.Tests.Configuration;
using SpendManagement.Integration.Tests.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SpendManagement.Integration.Tests.Fixtures
{
    public class HttpFixture
    {
        private readonly HttpClient _httpClient;

        public HttpFixture()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Tests");
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        public async Task<HttpResponseMessage?> PostAsync<T>(string resource, T body) where T : class
        {
            var json = JsonConvert.SerializeObject(body);
            StringContent httpContent = new(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJWToken());
            var url = ConstantsValues.APIVersion + resource;
            var response = await _httpClient.PostAsync(url, httpContent);
            return response;
        }

        public async Task<HttpResponseMessage?> DeleteAsync(string resource, Guid id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJWToken());

            var url = ConstantsValues.APIVersion
                .AppendPathSegment(resource)
                .AppendPathSegment(id);

            var response = await _httpClient.DeleteAsync(url);
            return response;
        }

        public async Task<HttpResponseMessage?> PatchAsync(string resource, Guid id, string jsonPatch)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJWToken());

            var httpContent = new StringContent(jsonPatch, Encoding.UTF8, "application/json");

            var url = ConstantsValues.APIVersion
                .AppendPathSegment(resource)
                .AppendPathSegment(id);

            var response = await _httpClient.PatchAsync(url, httpContent);
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
