using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadReady.DTO;
using RoadReady.Models;
using RoadReady.Repositories;

namespace RoadReady.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly RoadReadyContext _context;
        private readonly IMapper _mapper;

        public CarRepository(RoadReadyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CarDTO>> GetAllAsync()
        {
            try
            {
                // Including CarDetails and CarImages while fetching cars
                var cars = await _context.Cars
                                         .Include(c => c.CarDetails) // Assuming CarDetails is the navigation property in Car
                                         .Include(c => c.CarImages)  // Assuming CarImages is the navigation property in Car
                                         .ToListAsync();

                return _mapper.Map<IEnumerable<CarDTO>>(cars);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw;
            }
        }

        //public async Task<IEnumerable<CarDTO>> GetAllAsync()
        //{
        //    try
        //    {
        //        var cars = await _context.Cars.ToListAsync();
        //        return _mapper.Map<IEnumerable<CarDTO>>(cars);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        throw;
        //    }
        //}

        //public async Task<CarDTO?> GetByIdAsync(int id)
        //{
        //    try
        //    {
        //        var car = await _context.Cars.FindAsync(id);
        //        return _mapper.Map<CarDTO>(car);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        throw;
        //    }
        //}
        public async Task<CarDTO?> GetByIdAsync(int id)
        {
            try
            {
                var car = await _context.Cars
                    .Include(c => c.CarDetails)
                    .Include(c => c.CarImages)
                    .FirstOrDefaultAsync(c => c.CarId == id);

                return _mapper.Map<CarDTO>(car);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw;
            }
        }


        public async Task<CarDTO> CreateAsync(CarDTO carDto)
        {
            try
            {
                var car = _mapper.Map<Car>(carDto);
                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return _mapper.Map<CarDTO>(car);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw;
            }
        }

        public async Task<CarDTO> UpdateAsync(int id, CarDTO carDto)
        {
            try
            {
                var car = await _context.Cars.FindAsync(id);
                if (car == null) throw new KeyNotFoundException("Car not found.");

                _mapper.Map(carDto, car);
                await _context.SaveChangesAsync();
                return _mapper.Map<CarDTO>(car);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var car = await _context.Cars.FindAsync(id);
                if (car == null) return false;

                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw;
            }
        }
    }
}

