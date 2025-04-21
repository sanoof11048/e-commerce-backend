using Microsoft.EntityFrameworkCore;
using Plashoe.Data;
using Plashoe.DTOs;
using Plashoe.Model;

namespace Plashoe.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllUsers() =>
            await _context.Users.ToListAsync();

        public async Task<User> GetUser(int id)=>
            await _context.Users.FindAsync(id);

        public async Task<User> CheckUser(string email)=>
            await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

        public async Task Register(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
