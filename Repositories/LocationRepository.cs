using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadReady.DTO;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly RoadReadyContext _context;
        private readonly IMapper _mapper;

        public LocationRepository(RoadReadyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LocationDTO>> GetAllAsync()
        {
            try
            {
                var locations = await _context.Locations.Include(l => l.City).ToListAsync();
                return _mapper.Map<IEnumerable<LocationDTO>>(locations);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LocationDTO?> GetByIdAsync(int id)
        {
            try
            {
                var location = await _context.Locations.Include(l => l.City).FirstOrDefaultAsync(l => l.LocationId == id);
                return _mapper.Map<LocationDTO>(location);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LocationDTO> CreateAsync(LocationDTO locationDto)
        {
            try
            {
                var location = _mapper.Map<Location>(locationDto);
                location.City = null;
                _context.Locations.Add(location);
                await _context.SaveChangesAsync();
                return _mapper.Map<LocationDTO>(location);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LocationDTO> UpdateAsync(int id, LocationDTO locationDto)
        {
            try
            {
                var location = await _context.Locations.FindAsync(id);
                if (location == null) throw new KeyNotFoundException("Location not found.");
                //locationDto.City = null;
                _mapper.Map(locationDto, location);
                await _context.SaveChangesAsync();
                return _mapper.Map<LocationDTO>(location);
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
                var location = await _context.Locations.FindAsync(id);
                if (location == null) return false;

                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
