using MaybeFinal.Contexts;
using MaybeFinal.Models;
using MaybeFinal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static MaybeFinal.EnumsForApp.Enums; // Assume you have this enum class for roles

namespace MaybeFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MaybeFinalDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly RoleManager<IdentityRole> _roleManager;  // Inject RoleManager for roles

        public AuthController(MaybeFinalDbContext context,
                              IPasswordHasher<User> passwordHasher,
                              IJwtService jwtService,
                              RoleManager<IdentityRole> roleManager)  // Inject RoleManager
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _roleManager = roleManager;  // Initialize RoleManager
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                // Check if the username already exists
                if (await _context.Users.AnyAsync(u => u.UserName == registerDto.Username))  // Accessing UserName (not Username)
                {
                    return BadRequest("Username is already taken.");
                }

                // Hash the password
                var passwordHash = _passwordHasher.HashPassword(new User(), registerDto.Password);

                // Create a new User object
                var user = new User
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    UserName = registerDto.Username,  // Access UserName here
                    PasswordHash = passwordHash
                };

                // Ensure that the roles exist, create if not
                if (!await _roleManager.RoleExistsAsync(UserRole.User.ToString()))  // Check if "User" role exists
                {
                    var role = new IdentityRole(UserRole.User.ToString());
                    await _roleManager.CreateAsync(role);  // Create the role if it doesn't exist
                }

                // Assign role to the user
                user.Role = UserRole.User.ToString();

                // Add the user to the database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Find the user by username
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == loginDto.Username);  // Accessing UserName here

                // Check if the user exists and if the password is correct
                if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password) == PasswordVerificationResult.Failed)
                {
                    return Unauthorized("Invalid credentials.");
                }

                // Generate JWT token for the user
                var token = _jwtService.GenerateToken(user);  // Pass the user to the token generator

                return Ok(new { Token = token, Username = user.UserName, Role = user.Role });  // Accessing UserName here
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}


