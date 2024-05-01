using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadReady.DTO;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class CarDetailRepository : ICarDetailRepository
    {
        private readonly RoadReadyContext _context;
        private readonly IMapper _mapper;

        public CarDetailRepository(RoadReadyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CarDetailDTO>> GetAllAsync()
        {
            try
            {
                var carDetails = await _context.CarDetails.ToListAsync();
                return _mapper.Map<IEnumerable<CarDetailDTO>>(carDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CarDetailDTO?> GetByIdAsync(int id)
        {
            try
            {
                var carDetail = await _context.CarDetails.FindAsync(id);
                return _mapper.Map<CarDetailDTO>(carDetail);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CarDetailDTO> CreateAsync(CarDetailDTO carDetailDto)
        {
            try
            {
                var carDetail = _mapper.Map<CarDetail>(carDetailDto);
                _context.CarDetails.Add(carDetail);
                await _context.SaveChangesAsync();
                return _mapper.Map<CarDetailDTO>(carDetail);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CarDetailDTO> UpdateAsync(int id, CarDetailDTO carDetailDto)
        {
            try
            {
                var carDetail = await _context.CarDetails.FindAsync(id);
                if (carDetail == null) throw new KeyNotFoundException("CarDetail not found.");

                _mapper.Map(carDetailDto, carDetail);
                await _context.SaveChangesAsync();
                return _mapper.Map<CarDetailDTO>(carDetail);
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
                var carDetail = await _context.CarDetails.FindAsync(id);
                if (carDetail == null) return false;

                _context.CarDetails.Remove(carDetail);
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
