using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadReady.DTO;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly RoadReadyContext _context;
        private readonly IMapper _mapper;

        public CityRepository(RoadReadyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityDTO>> GetAllAsync()
        {
            try
            {
                var cities = await _context.Cities.ToListAsync();
                return _mapper.Map<IEnumerable<CityDTO>>(cities);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CityDTO?> GetByIdAsync(int id)
        {
            try
            {
                var city = await _context.Cities.FindAsync(id);
                return _mapper.Map<CityDTO>(city);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CityDTO> CreateAsync(CityDTO cityDto)
        {
            try
            {
                var city = _mapper.Map<City>(cityDto);
                _context.Cities.Add(city);
                await _context.SaveChangesAsync();
                return _mapper.Map<CityDTO>(city);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CityDTO> UpdateAsync(int id, CityDTO cityDto)
        {
            try
            {
                var city = await _context.Cities.FindAsync(id);
                if (city == null) throw new KeyNotFoundException("City not found.");

                _mapper.Map(cityDto, city);
                await _context.SaveChangesAsync();
                return _mapper.Map<CityDTO>(city);
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
                var city = await _context.Cities.FindAsync(id);
                if (city == null) return false;

                _context.Cities.Remove(city);
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
