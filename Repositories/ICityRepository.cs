using RoadReady.DTO;

namespace RoadReady.Repositories
{
    public interface ICityRepository
    {
        Task<IEnumerable<CityDTO>> GetAllAsync();
        Task<CityDTO?> GetByIdAsync(int id);
        Task<CityDTO> CreateAsync(CityDTO cityDto);
        Task<CityDTO> UpdateAsync(int id, CityDTO cityDto);
        Task<bool> DeleteAsync(int id);
    }
}
