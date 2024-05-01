using RoadReady.DTO;

namespace RoadReady.Repositories
{
    public interface ICarDetailRepository
    {
        Task<IEnumerable<CarDetailDTO>> GetAllAsync();
        Task<CarDetailDTO?> GetByIdAsync(int id);
        Task<CarDetailDTO> CreateAsync(CarDetailDTO carDetailDto);
        Task<CarDetailDTO> UpdateAsync(int id, CarDetailDTO carDetailDto);
        Task<bool> DeleteAsync(int id);
    }
}
