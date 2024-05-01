using RoadReady.DTO;

namespace RoadReady.Repositories
{
    public interface ICarRepository
    {
        Task<IEnumerable<CarDTO>> GetAllAsync();
        Task<CarDTO?> GetByIdAsync(int id);
        Task<CarDTO> CreateAsync(CarDTO carDto);
        Task<CarDTO> UpdateAsync(int id, CarDTO carDto);
        Task<bool> DeleteAsync(int id);
    }
}
