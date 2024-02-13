using System.Net.Http.Json;
using SmartScannerBackend.Services.Authentication;

namespace SmartScannerTests.AuthenticationTests
{
    [TestClass]
    public class SignInTests : SmartScannerTestBase
    {
        [TestMethod]
        /// <see cref="SmartScannerBackend.Controllers.AuthenticationController.SignIn"/>
        public async Task TestAuthenticationSignInSuccess()
        {
            var response = await _httpClient!.PostAsJsonAsync(
                "api/Authentication/SignIn",
                /// <see cref="SmartScannerBackend.DataAccess.SmartScannerDbContext.OnModelCreating"/> for default username and password
                new SignInRequest { username = "shiningdevusername", password = "shiningdevpassword" });

            Assert.IsTrue(response.IsSuccessStatusCode);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<SignInTestResponse>();
            Assert.IsInstanceOfType<SignInTestResponse>(result);

            Assert.IsTrue(result.success);
            Assert.AreNotEqual(0, result.payload!.token.Length);
        }

        [TestMethod]
        public async Task TestAuthenticationSignInFailedIncorrectUsername()
        {
            var response = await _httpClient!.PostAsJsonAsync(
                "api/Authentication/SignIn",
                new SignInRequest { username = "shiningdevusernameIncorrect", password = "shiningdevpassword" });

            Assert.IsFalse(response.IsSuccessStatusCode);

            var result = await response.Content.ReadFromJsonAsync<SignInTestResponseFailed>();
            Assert.IsInstanceOfType<SignInTestResponseFailed>(result);

            Assert.IsFalse(result.success);
            Assert.AreEqual("Username or password incorrect", result.error);
        }

        [TestMethod]
        public async Task TestAuthenticationSignInFailedIncorrectPassword()
        {
            var response = await _httpClient!.PostAsJsonAsync(
                "api/Authentication/SignIn",
                new SignInRequest { username = "shiningdevusername", password = "shiningdevpasswordIncorrect" });

            Assert.IsFalse(response.IsSuccessStatusCode);

            var result = await response.Content.ReadFromJsonAsync<SignInTestResponseFailed>();
            Assert.IsInstanceOfType<SignInTestResponseFailed>(result);

            Assert.IsFalse(result.success);
            Assert.AreEqual("Username or password incorrect", result.error);
        }

        [TestMethod]
        public async Task TestAuthenticationSignInIncompleteRequest()
        {
            var response = await _httpClient!.PostAsJsonAsync(
                "api/Authentication/SignIn", new { username = "shiningdevusername" });

            Assert.IsFalse(response.IsSuccessStatusCode);

            var result = await response.Content.ReadFromJsonAsync<SignInTestResponseFailed>();
            Assert.IsInstanceOfType<SignInTestResponseFailed>(result);

            Assert.IsFalse(result.success);
            Assert.AreEqual("Username or password incorrect", result.error);
        }
    }

    public class SignInTestResponse
    {
        public bool success { get; set; } = false;

        public SignInTestResponsePayload? payload { get; set; } = null;
    }

    public class SignInTestResponsePayload
    {
        public string token { get; set; } = string.Empty;
    }

    public class SignInTestResponseFailed
    {
        public bool success { get; set; } = false;

        public string error { get; set; } = string.Empty;
    }
}