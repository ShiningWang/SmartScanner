using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartScannerBackend.DataAccess;
using SmartScannerBackend.Services.Authentication;

namespace SmartScannerBackend.Controllers
{
    [ApiController]
    [Route("api/Authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly SmartScannerDbContext _dbContext;

        public AuthenticationController(SmartScannerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("SignIn")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInRequest request)
        {
            var targetUser = await _dbContext
                .Users.Where(u => u.UserName == request.username.ToString())
                .FirstOrDefaultAsync();

            if (targetUser == null)
                return new BadRequestObjectResult(new { success = false, error = "Username or password incorrect" });

            string hashedPasswordForValidation =
                PasswordUtilities.GetHashedPassword(Convert.FromBase64String(targetUser.Salt), request.password);

            if (hashedPasswordForValidation != targetUser.Password)
                return new BadRequestObjectResult(new { success = false, error = "Username or password incorrect" });

            var issuedToken = PasswordUtilities.IssueJwt(targetUser);

            return new OkObjectResult(new { success = true, payload = new { token = issuedToken } });
        }

        [Route("Validate")]
        [HttpGet]
        [Authorize]
        public IActionResult Validate()
        {
            return new OkObjectResult(new { success = true });
        }
    }
}
