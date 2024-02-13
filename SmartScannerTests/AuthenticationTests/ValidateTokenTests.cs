using System.Net.Http.Headers;
using System.Net.Http.Json;
using SmartScannerBackend.Services.Authentication;

namespace SmartScannerTests.AuthenticationTests
{
    [TestClass]
    public class ValidateTokenTests : SmartScannerTestBase
    {
        [TestMethod]
        /// <see cref="SmartScannerBackend.Controllers.AuthenticationController.Validate"/>
        public async Task TestAuthenticationValidateSuccess()
        {
            string token = GetValidUserToken();

            _httpClient!.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient!.GetAsync("api/Authentication/Validate");
            Assert.IsTrue(response.IsSuccessStatusCode);

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ValidateTokenResponse>();
            Assert.IsInstanceOfType<ValidateTokenResponse>(result);

            Assert.IsTrue(result.success);
        }

        [TestMethod]
        public async Task TestAuthenticationValidateFailedNoToken()
        {
            var response = await _httpClient!.GetAsync("api/Authentication/Validate");
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task TestAuthenticationValidateFaileInvalidToken()
        {
            string token = GetValidUserToken();

            _httpClient!.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token + "Invalid");

            var response = await _httpClient!.GetAsync("api/Authentication/Validate");
            Assert.IsFalse(response.IsSuccessStatusCode);
        }
    }

    public class ValidateTokenResponse
    {
        public bool success { get; set; }
    }
}