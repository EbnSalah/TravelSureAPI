using Microsoft.AspNetCore.Mvc;
using TravelSureAPI.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace TravelSureAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserAccount user)
        {
            // Assign user 'ID'
            user.UserId = Guid.NewGuid();

            //Check if 'userName' exists
            if (UserStore.UserNameExists(user.UserName))
            {
                return BadRequest(new { message = "Username already exists" });
            }

            //Check if 'Email' exists
            if(UserStore.GetUserByEmail(user.Email)!=null)
            {
                return BadRequest(new {message = "Email already registered"});
            }
            
            // 'Password' Encryption
            user.PasswordHash = HashPassword(user.PasswordHash);

            //Validate 'MembershipTier'
            var allowedTiers = new[] { "Basic", "Premium" };
            if (!allowedTiers.Contains(user.MembershipTier))
            {
                return BadRequest(new {message = "Invalid Membership Tier. Allowed: Basic or Premium"});
            }

            // Save\add User 
            UserStore.AddUser(user);

            //Success Register Message
            return Ok(new { message = "Registeration successful" });

        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginData)
        {
            // Get user by email
            var user = UserStore.GetUserByEmail(loginData.Email);
            if(user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Check password
            var hashed = HashPassword(loginData.Password);
            if (user.PasswordHash != hashed)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(new 
            {
                message = "Login Successful",
                username = user.UserName,
                memberShip = user.MembershipTier,
            });

        }

        [HttpPost("upgrade")]
        public IActionResult UpgradeMembership([FromBody] UpgradeRequest request)
        {
            var user = UserStore.GetUserByEmail(request.Email);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            if (user.MembershipTier == "premium")
            {
                return BadRequest(new { message = "User is already a Premium member" });
            }
            user.MembershipTier = "Premium";

            return Ok(new
            {
                message = "Membership upgraded to Premium",
                username = user.UserName,
                newTier = user.MembershipTier,
            }
                );
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);

            }
        }
    }
}
