using System.Diagnostics;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartScannerBackend.DataAccess;
using SmartScannerBackend.Services.AzureAI;

namespace SmartScannerTests.AITests
{
    [TestClass]
    public class ReceiptResolveTests : SmartScannerTestBase
    {
        [TestInitialize]
        public new void Initialize()
        {
            _factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.RemoveAll(typeof(IContentExtraction));
                        services.AddScoped<IContentExtraction>(s => new TestContentExtraction());
                    });
                });

            var scope = _factory.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<SmartScannerDbContext>()
                .Database.EnsureCreated();

            _httpClient = _factory.CreateClient();
        }

        [TestMethod]
        public async Task TestDemo()
        {
            var response = await _httpClient!.PostAsJsonAsync("api/AIPlatform/ReadImage", new { });

            var result = await response.Content.ReadAsStringAsync();

            Trace.WriteLine(result);

            Assert.IsTrue(false);
        }
    }

    public class TestContentExtraction : ContentExtraction
    {
        public new async Task Resolve()
        {
            await Task.Run(() =>
            {
                _decodedLines =
                    @"coles\r\nexpress\r\nEUREKA OPERATIONS PTY LTD\r\nTAX INVOICE / ABN 78 104 811 216\r\n
                    www.colesexpress.com.au\r\nGARDENVALE\r\n(03) 9596 6552\r\n191 NEPEAN HWY & ELSTER AVE, GARDENVALE\r\n
                    $\r\n% V-POWER\r\nPUMP 8\r\n49.71\r\nQTY: 35.03\r\nL @ 141.9 c/L\r\n% UNLEADED\r\nPUMP 8\r\n44.97\r\n
                    QTY: 36.89\r\nL @ 121.9 c/L\r\n% QUILTON TOILET ROLL\r\n20.00\r\nQTY: 2\r\n@\r\n$10.00 EACH\r\n4 c/L 
                    Fuel Discount\r\n-2.88\r\n10 c/L Fuel Discount\r\n-7.19\r\nTOTAL\r\n$104.61\r\nEFT\r\n104.61\r\nGST 
                    INCLUDED IN THE TRANSACTION\r\n$9.51\r\n% = TAXABLE ITEMS\r\nTOTAL SAVINGS $10.07\r\n";
            });
        }

    }
}