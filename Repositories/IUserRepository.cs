using RoadReady.DTO;

namespace RoadReady.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO?> GetByIdAsync(int id);
        Task<UserDTO> CreateAsync(UserDTO userDto);
        Task<UserDTO> UpdateAsync(int id, UserDTO userDto);
        Task<bool> DeleteAsync(int id);
    }
}
