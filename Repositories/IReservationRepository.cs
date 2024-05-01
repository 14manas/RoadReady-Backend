using RoadReady.DTO;

namespace RoadReady.Repositories
{
    public interface IReservationRepository
    {
        Task<IEnumerable<ReservationDTO>> GetAllAsync();
        Task<ReservationDTO?> GetByIdAsync(int id);
        Task<ReservationDTO> CreateAsync(ReservationDTO reservationDto);
        Task<ReservationDTO> UpdateAsync(int id, ReservationDTO reservationDto);
        Task<bool> DeleteAsync(int id);
        Task<string> BookReservationAsync(BookedDTO booked, int userId);

        Task<List<CarDTO>> SearchAvailableCarsAsync(SearchDTO search);
    }
}
