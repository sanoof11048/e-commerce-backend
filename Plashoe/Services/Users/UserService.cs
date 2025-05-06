using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Plashoe.Data;
using Plashoe.DTOs;
using Plashoe.Model;
using Plashoe.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Text;

namespace Plashoe.Services.Users
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserRepository _userRepo;

        public UserService(AppDbContext context,UserRepository repository, IMapper mapper, ILogger<UserService> logger, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _userRepo= repository;
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            var users = await _userRepo.GetAllUsers();
            //var userDTOs new List<UserDTO>
            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO?> GetUserById(int id)
        {
                var user = await _userRepo.GetUser(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<ResultDTO> Login(LoginDTO loginDTO)
        {
            try
            {
                _logger.LogInformation("Logging in user");
                var user = await _userRepo.CheckUser(loginDTO.Email);
                if (user == null)
                    return new ResultDTO { Error = "User not found" };

                if (user.IsBlocked)
                    return new ResultDTO { Error = "User is blocked" };

                if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
                    return new ResultDTO { Error = "Invalid password" };

                var token = GenerateToken(user);
                return new ResultDTO
                {
                    Token = token,
                    Email = user.Email,
                    Id = user.Id,
                    Name = user.Name
                };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Login error: {ex.Message}");
                return new ResultDTO { Error = "Internal error during login" };
            }
        }

        public async Task<ApiResponse<bool>> Register(RegisterDTO newUser)
        {
            try
            {
                var existingUser = await _userRepo.CheckUser(newUser.Email);
                if (existingUser != null)
                {
                    return new ApiResponse<bool>(409, "User already exists", false);
                }

                newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                var user = _mapper.Map<User>(newUser);
                await _userRepo.Register(user);
                return new ApiResponse<bool>(201, "SignUp successfull", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(500, "Registration failed", false, ex.Message);
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ApiResponse<bool>> ToggleBlockUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return new ApiResponse<bool>(
                    statuscode: 404,
                    message: "User not found",
                    data: false,
                    error: "No user exists with the provided ID."
                );
            }

            user.IsBlocked = !user.IsBlocked;
            await _context.SaveChangesAsync();

            string statusMessage = user.IsBlocked ? "User blocked" : "User unblocked";

            return new ApiResponse<bool>(
                statuscode: 200,
                message: statusMessage,
                data: user.IsBlocked
            );
        }


        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = "YourIssuer",
                Audience = "YourAudience",
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
