using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SmartScannerBackend.DataAccess;
using SmartScannerBackend.Services.Authentication;

namespace SmartScannerTests
{
    public class SmartScannerTestBase
    {
        public WebApplicationFactory<Startup>? _factory;

        public HttpClient? _httpClient;

        [TestInitialize]
        public void Initialize()
        {
            _factory = new WebApplicationFactory<Startup>();

            var scope = _factory.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<SmartScannerDbContext>()
                .Database.EnsureCreated();

            _httpClient = _factory.CreateClient();
        }

        [TestCleanup]
        public void Cleanup()
        {
            var scope = _factory!.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<SmartScannerDbContext>()
                .Database.EnsureDeleted();
        }

        public string GetValidUserToken()
        {
            var scope = _factory!.Services.CreateScope();
            var user = scope.ServiceProvider.GetRequiredService<SmartScannerDbContext>()
                .Users.Where(u => u.UserName == "shiningdevusername").First(); // should exist

            return PasswordUtilities.IssueJwt(user);
        }
    }
}