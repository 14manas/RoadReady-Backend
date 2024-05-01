using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadReady.DTO;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class CarImageRepository : ICarImageRepository
    {
        private readonly RoadReadyContext _context;
        private readonly IMapper _mapper;

        public CarImageRepository(RoadReadyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CarImageDTO>> GetAllAsync()
        {
            try
            {
                var carImages = await _context.CarImages.ToListAsync();
                return _mapper.Map<IEnumerable<CarImageDTO>>(carImages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CarImageDTO?> GetByIdAsync(int id)
        {
            try
            {
                var carImage = await _context.CarImages.FindAsync(id);
                return _mapper.Map<CarImageDTO>(carImage);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CarImageDTO> CreateAsync(CarImageDTO carImageDto)
        {
            try
            {
                var carImage = _mapper.Map<CarImage>(carImageDto);
                _context.CarImages.Add(carImage);
                await _context.SaveChangesAsync();
                return _mapper.Map<CarImageDTO>(carImage);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CarImageDTO> UpdateAsync(int id, CarImageDTO carImageDto)
        {
            try
            {
                var carImage = await _context.CarImages.FindAsync(id);
                if (carImage == null) throw new KeyNotFoundException("CarImage not found.");

                _mapper.Map(carImageDto, carImage);
                await _context.SaveChangesAsync();
                return _mapper.Map<CarImageDTO>(carImage);
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
                var carImage = await _context.CarImages.FindAsync(id);
                if (carImage == null) return false;

                _context.CarImages.Remove(carImage);
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
