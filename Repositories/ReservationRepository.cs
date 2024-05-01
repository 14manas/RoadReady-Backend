using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadReady.DTO;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly RoadReadyContext _context;
        private readonly IMapper _mapper;

        public ReservationRepository(RoadReadyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReservationDTO>> GetAllAsync()
        {
            try
            {
                var reservations = await _context.Reservations.Include(r => r.User).Include(r => r.Car).ToListAsync();
                return _mapper.Map<IEnumerable<ReservationDTO>>(reservations);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ReservationDTO?> GetByIdAsync(int id)
        {
            try
            {
                var reservation = await _context.Reservations.Include(r => r.User).Include(r => r.Car).FirstOrDefaultAsync(r => r.ReservationId == id);
                return _mapper.Map<ReservationDTO>(reservation);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ReservationDTO> CreateAsync(ReservationDTO reservationDto)
        {
            try
            {
                reservationDto.Car = null;
                reservationDto.User = null;
                // Initialize a new Reservation entity from the DTO without User and Car object graphs
                var reservation = new Reservation
                {
                    ReservationDateTime = reservationDto.ReservationDateTime,
                    ReturnDateTime = reservationDto.ReturnDateTime,
                    TotalCost = reservationDto.TotalCost,
                    StatusName = reservationDto.StatusName,
                    UserId = reservationDto.UserId, // Directly use the UserId if available in your DTO
                    CarId = reservationDto.CarId // Directly use the CarId if available in your DTO
                };

                // Add the reservation entity. EF Core will not attempt to add new User or Car entities
                // because only their IDs are set on the reservation entity
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                // After saving, map the newly created reservation (including its User and Car references) back to a DTO
                // This step might require fetching the reservation again with .Include() for User and Car if needed
                var createdReservationDto = _mapper.Map<ReservationDTO>(reservation);

                return createdReservationDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ReservationDTO> UpdateAsync(int id, ReservationDTO reservationDto)
        {
            try
            {
                reservationDto.User = null;
                reservationDto.Car = null;
                var reservation = await _context.Reservations.FindAsync(id);
                if (reservation == null) throw new KeyNotFoundException("Reservation not found.");

                _mapper.Map(reservationDto, reservation);
                await _context.SaveChangesAsync();
                return _mapper.Map<ReservationDTO>(reservation);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var reservation = await _context.Reservations.FindAsync(id);
                if (reservation == null) return false;

                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> BookReservationAsync(BookedDTO booked, int userId)
        {
            try
            {
                // Check for conflicting reservations
                var existingReservation = await _context.Reservations
                    .AnyAsync(r => r.CarId == booked.CarId &&
                                   ((booked.StartDate >= r.ReservationDateTime && booked.StartDate <= r.ReturnDateTime) ||
                                    (booked.ReturnDate >= r.ReservationDateTime && booked.ReturnDate <= r.ReturnDateTime)));
                if (existingReservation)
                {
                    return "Car is already booked for the given dates.";
                }

                // Find the car to get the rate per hour
                var car = await _context.Cars.FindAsync(booked.CarId);
                if (car == null)
                {
                    return "Car not found.";
                }

                // Calculate total cost
                var hours = (booked.ReturnDate - booked.StartDate).TotalHours;
                var totalCost = (decimal)hours * car.RatePerHour;

                // Create and save the new reservation
                var reservation = new Reservation
                {
                    UserId = userId,
                    CarId = booked.CarId,
                    ReservationDateTime = booked.StartDate,
                    ReturnDateTime = booked.ReturnDate,
                    TotalCost = totalCost,
                    StatusName = "Booked" // Assuming status names are predefined
                };
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                // Assuming payment is processed separately and PaymentDetail is created later
                return $"Reservation successful with ID: {reservation.ReservationId}. Total cost: {totalCost}";
            }
            catch(Exception ex)
            {
                return "" ;

            }
            
        }
        public async Task<List<CarDTO>> SearchAvailableCarsAsync(SearchDTO search)
        {
            // Include the necessary navigation properties in the initial query
            var location = await _context.Locations
                            .Include(l => l.Cars)
                                .ThenInclude(car => car.CarDetails) // Assuming 'CarDetails' is a collection or property
                            .Include(l => l.Cars)
                                .ThenInclude(car => car.CarImages)  // Assuming 'CarImages' is a collection or property
                            .FirstOrDefaultAsync(l => l.LocationName == search.LocationName);

            if (location == null)
                return new List<CarDTO>(); // No cars available if location not found

            // Convert search start and end to date-only for comparison
            var searchStartDate = search.StartDate.Date;
            var searchEndDate = search.EndDate.Date;

            var availableCars = location.Cars
                .Where(car => !_context.Reservations.Any(r => r.CarId == car.CarId &&
                        (searchStartDate <= r.ReturnDateTime.Date && searchEndDate >= r.ReservationDateTime.Date)))
                .ToList();

            // Map the result to DTOs which should also map details and images if set up in the mapping configuration
            return _mapper.Map<List<CarDTO>>(availableCars);
        }



    }
}

