using Plashoe.DTOs;
using Plashoe.Model;

namespace Plashoe.Services.Users
{
    public interface IUserService
    {
        Task<ResultDTO> Login(LoginDTO loginDTO);
        Task<ApiResponse<bool>> Register(RegisterDTO registerDTO);
        Task<List<UserDTO>> GetUsers();
        Task<UserDTO?> GetUserById(int id);
        Task<bool> DeleteUser(int id);
        Task<ApiResponse<bool>> ToggleBlockUser(int id);
    }
}
