using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadReady.DTO;
using RoadReady.Models;
using RoadReady.Repositories;

namespace RoadReady.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarImagesController : ControllerBase
    {
        public class CarImageRepository : ICarImageRepository
        {
            private readonly RoadReadyContext _context;
            private readonly IMapper _mapper;
            private readonly ILogger<CarImagesController> _logger;

            public CarImageRepository(RoadReadyContext context, IMapper mapper, ILogger<CarImagesController> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<IEnumerable<CarImageDTO>> GetAllAsync()
            {
                try
                {
                    _logger.LogInformation("Retrieving all car images.");
                    var carImages = await _context.CarImages.ToListAsync();
                    return _mapper.Map<IEnumerable<CarImageDTO>>(carImages);
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to retrieve all car images.");
                    throw;
                }
            }

            public async Task<CarImageDTO?> GetByIdAsync(int id)
            {
                try
                {
                    _logger.LogInformation($"Retrieving car image with ID: {id}");
                    var carImage = await _context.CarImages.FindAsync(id);
                    return _mapper.Map<CarImageDTO>(carImage);
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to retrieve car image with ID: {id}");
                    throw;
                }
            }
            [Authorize(Roles = "admin, caragent")]
            public async Task<CarImageDTO> CreateAsync(CarImageDTO carImageDto)
            {
                try
                {
                    _logger.LogInformation("Creating new car image.");
                    var carImage = _mapper.Map<CarImage>(carImageDto);
                    _context.CarImages.Add(carImage);
                    await _context.SaveChangesAsync();
                    return _mapper.Map<CarImageDTO>(carImage);
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to create new car image.");
                    throw;
                }
            }
            [Authorize(Roles = "admin, caragent")]
            public async Task<CarImageDTO> UpdateAsync(int id, CarImageDTO carImageDto)
            {
                try
                {
                    _logger.LogInformation($"Updating car image with ID: {id}");
                    var carImage = await _context.CarImages.FindAsync(id);
                    if (carImage == null) throw new KeyNotFoundException("CarImage not found.");

                    _mapper.Map(carImageDto, carImage);
                    await _context.SaveChangesAsync();
                    return _mapper.Map<CarImageDTO>(carImage);
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to update car image with ID: {id}");
                    throw;
                }
            }
            [Authorize(Roles = "admin, caragent")]
            public async Task<bool> DeleteAsync(int id)
            {
                try
                {
                    _logger.LogInformation($"Deleting car image with ID: {id}");
                    var carImage = await _context.CarImages.FindAsync(id);
                    if (carImage == null) return false;

                    _context.CarImages.Remove(carImage);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to delete car image with ID: {id}");
                    throw;
                }
            }
        }
    }
}
