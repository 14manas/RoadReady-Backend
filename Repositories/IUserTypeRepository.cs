using RoadReady.DTO;

namespace RoadReady.Repositories
{
    public interface IUserTypeRepository
    {
        Task<IEnumerable<UserTypeDTO>> GetAllAsync();
        Task<UserTypeDTO?> GetByIdAsync(int id);
        Task<UserTypeDTO> CreateAsync(UserTypeDTO userTypeDto);
        Task<UserTypeDTO> UpdateAsync(int id, UserTypeDTO userTypeDto);
        Task<bool> DeleteAsync(int id);
    }
}
