using RoadReady.DTO;

namespace RoadReady.Repositories
{
    public interface ILocationRepository
    {
        Task<IEnumerable<LocationDTO>> GetAllAsync();
        Task<LocationDTO?> GetByIdAsync(int id);
        Task<LocationDTO> CreateAsync(LocationDTO locationDto);
        Task<LocationDTO> UpdateAsync(int id, LocationDTO locationDto);
        Task<bool> DeleteAsync(int id);
    }
}

