using RoadReady.DTO;

namespace RoadReady.Repositories
{
    public interface IStateRepository
    {
        Task<IEnumerable<StateDTO>> GetAllAsync();
        Task<StateDTO?> GetByIdAsync(int id);
        Task<StateDTO> CreateAsync(StateDTO stateDto);
        Task<StateDTO> UpdateAsync(int id, StateDTO stateDto);
        Task<bool> DeleteAsync(int id);
    }
}
