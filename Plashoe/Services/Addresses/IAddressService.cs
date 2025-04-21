// Services/Address/IAddressService.cs
using Plashoe.DTOs;
using Plashoe.Model;

namespace Plashoe.Services.Addresses
{
    public interface IAddressService
    {
        Task<List<AddressDTO>> GetByUserId(int userId);
        Task<AddressDTO> GetById(int id);
        Task<bool> Add(int id,AddressDTO dto);
        Task<bool> Delete(int id);
    }
}
