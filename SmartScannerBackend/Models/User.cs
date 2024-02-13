using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using SmartScannerBackend.Services.Authentication;

namespace SmartScannerBackend.Models
{
    public class User : IdentityUser
    {
        public bool Disqualified { get; set; } = false;

        [JsonPropertyName("Salt")]
        private string _salt = string.Empty;

        [JsonIgnore]
        public string Salt
        {
            get
            {
                if (_salt.Length == 0)
                {
                    _salt = Convert.ToBase64String(
                        PasswordUtilities.GetNewSalt());
                };

                return _salt;
            }
            set
            {
                _salt = value;
            }
        }

        [JsonPropertyName("Password")]
        private string _password = string.Empty;

        [JsonIgnore]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = PasswordUtilities.GetHashedPassword(
                    Convert.FromBase64String(Salt), value);
            }
        }

        [JsonPropertyName("Role")]
        public UserRole Role { get; set; } = UserRole.User;

        public enum UserRole
        {
            User,

            Admin
        }
    }
}