using System.Net.Http.Headers;

namespace SmartScannerBackend.Services.OpenAI
{
    public interface IResolveContent
    {
        public Task<bool> Decode(string content);

        public Task<T?> ReadResultAs<T>() where T : class;
    }

    public class ResolveContent : IResolveContent
    {
        private readonly HttpClient _httpClient;

        private readonly string _openAIEndpoint;

        private readonly string _openAiKey;

        private readonly string _prompt = @"
            Please use the given text to produce a json object.
            the object must contain the following attributes: 
            1. 'Company', 2. 'Expenses', and 3. 'Summary'. 
            The 'Company' attribute must contain: 
                1. 'Name': the name of the business or shop in String format, 
                2. 'Address': the address or the business or shop in String format, and 
                3. 'PhoneNumber': the contact number of the business or shop in String format. 
            The 'Expenses' attribute must contain an array of 'Expense', each 'Expense' must contain: 
                1. 'ItemName': the name of the item in String format, 
                2. 'ItemQuantity': the unit quantity of the buying item in decimal format, 
                3. 'PricePerUnit': the price per unit, and 
                4. 'ItemTotalPrice': the total price in decimal format.
            The 'Summary' attribute must contain:
                1. 'TotalPrice': the price of this transaction
                2. 'DatetimeOfExpense': the datetime of this expense, in string pattern of 'yyyy-MM-dd HH:mm:ss'
            These are the given words:
        ";

        private HttpContent? _httpContent = null;

        public ResolveContent(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _openAIEndpoint = System.Configuration.ConfigurationManager.AppSettings["OpenAIEndpoint"]!;
            _openAiKey = System.Configuration.ConfigurationManager.AppSettings["OpenAiKey"]!;
        }

        public async Task<bool> Decode(string content)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _openAiKey);

            var requestBody = new
            {
                model = "gpt-4",
                messages = new[] { new { role = "user", content = _prompt + Environment.NewLine + content } },
                temperature = 0.9,
                max_tokens = 4096,
            };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_openAIEndpoint, requestBody);

            if (response.IsSuccessStatusCode)
            {
                _httpContent = response.Content;

                var resString = await _httpContent.ReadAsStringAsync();
                Console.WriteLine(resString);
            };

            return response.IsSuccessStatusCode;
        }

        public async Task<T?> ReadResultAs<T>() where T : class
        {
            return await _httpContent!.ReadFromJsonAsync<T>();
        }

    }
}