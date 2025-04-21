// Services/Address/AddressService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Plashoe.Data;
using Plashoe.DTOs;
using Plashoe.Model;

namespace Plashoe.Services.Addresses
{
    public class AddressService : IAddressService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AddressService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AddressDTO>> GetByUserId(int userId)
        {
            var addresses = await _context.Address
                .Include(a => a.User)
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return _mapper.Map<List<AddressDTO>>(addresses);
        }

        public async Task<AddressDTO> GetById(int id)
        {
            var address = await _context.Address
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.AddressId == id);

            return _mapper.Map<AddressDTO>(address);
        }

        public async Task<bool> Add(int id,AddressDTO dto)
        {
            try
            {
                var address = _mapper.Map<Address>(dto);

                if (address.IsDefault)
                {
                    address.UserId = id;
                    var existingDefaults = await _context.Address
                        .Where(a => a.UserId == address.UserId && a.IsDefault)
                        .ToListAsync();

                    foreach (var existingDefault in existingDefaults)
                    {
                        existingDefault.IsDefault = false;
                    }
                }

                _context.Address.Add(address);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var address = await _context.Address.FindAsync(id);
                if (address == null) return false;

                _context.Address.Remove(address);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

       
    }
}
