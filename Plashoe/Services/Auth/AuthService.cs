using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Plashoe.Data;
using Plashoe.DTOs;
using Plashoe.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Plashoe.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        public AuthService(AppDbContext context, IMapper mapper, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

       
    }
}
