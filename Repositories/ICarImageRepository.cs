using RoadReady.DTO;

namespace RoadReady.Repositories
{
    public interface ICarImageRepository
    {
        Task<IEnumerable<CarImageDTO>> GetAllAsync();
        Task<CarImageDTO?> GetByIdAsync(int id);
        Task<CarImageDTO> CreateAsync(CarImageDTO carImageDto);
        Task<CarImageDTO> UpdateAsync(int id, CarImageDTO carImageDto);
        Task<bool> DeleteAsync(int id);
    }
}
