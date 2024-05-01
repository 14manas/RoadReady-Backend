using RoadReady.DTO;

namespace RoadReady.Repositories
{
    public interface ICarReviewRepository
    {
        Task<IEnumerable<CarReviewDTO>> GetAllAsync();
        Task<CarReviewDTO?> GetByIdAsync(int id);
        Task<CarReviewDTO> CreateAsync(CarReviewDTO carReviewDto);
        Task<CarReviewDTO> UpdateAsync(int id, CarReviewDTO carReviewDto);
        Task<bool> DeleteAsync(int id);
    }
}
