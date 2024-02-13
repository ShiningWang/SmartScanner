namespace SmartScannerBackend.Services.Authentication
{
    public class SignInRequest
    {
        public string username { get; set; } = string.Empty;

        public string password { get; set; } = string.Empty;
    }
}