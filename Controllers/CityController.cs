using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadReady.DTO;
using RoadReady.Exceptions;
using RoadReady.Models;
using RoadReady.Repositories;

namespace RoadReady.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        public class CityRepository : ICityRepository
        {
            private readonly RoadReadyContext _context;
            private readonly IMapper _mapper;
            private readonly ILogger<CityController> _logger;

            public CityRepository(RoadReadyContext context, IMapper mapper, ILogger<CityController> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }
            [HttpGet]
            public async Task<IEnumerable<CityDTO>> GetAllAsync()
            {
                try
                {
                    _logger.LogInformation("Retrieving all cities.");
                    var cities = await _context.Cities.ToListAsync();
                    return _mapper.Map<IEnumerable<CityDTO>>(cities);
                }
                catch (CityNotisFoundException)
                {
                    _logger.LogError("Failed to retrieve all cities.");
                    throw;
                }
            }
            [HttpGet("{id}")]
            public async Task<CityDTO?> GetByIdAsync(int id)
            {
                try
                {
                    _logger.LogInformation($"Retrieving city with ID: {id}");
                    var city = await _context.Cities.FindAsync(id);
                    return _mapper.Map<CityDTO>(city);
                }
                catch (CityNotisFoundException)
                {
                    _logger.LogError($"Failed to retrieve city with ID: {id}");
                    throw;
                }
            }
            [HttpPost]
            [Authorize(Roles = "admin")]
            public async Task<CityDTO> CreateAsync(CityDTO cityDto)
            {
                try
                {
                    _logger.LogInformation("Creating new city.");
                    var city = _mapper.Map<City>(cityDto);
                    _context.Cities.Add(city);
                    await _context.SaveChangesAsync();
                    return _mapper.Map<CityDTO>(city);
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to create new city.");
                    throw;
                }
            }
            [HttpPut("{id}")]
            [Authorize(Roles = "admin")]
            public async Task<CityDTO> UpdateAsync(int id, CityDTO cityDto)
            {
                try
                {
                    _logger.LogInformation($"Updating city with ID: {id}");
                    var city = await _context.Cities.FindAsync(id);
                    if (city == null) throw new KeyNotFoundException("City not found.");

                    _mapper.Map(cityDto, city);
                    await _context.SaveChangesAsync();
                    return _mapper.Map<CityDTO>(city);
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to update city with ID: {id}");
                    throw;
                }
            }
            [HttpDelete(("{id}"))]
            [Authorize(Roles = "admin")]
            public async Task<bool> DeleteAsync(int id)
            {
                try
                {
                    _logger.LogInformation($"Deleting city with ID: {id}");
                    var city = await _context.Cities.FindAsync(id);
                    if (city == null) return false;

                    _context.Cities.Remove(city);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to delete city with ID: {id}");
                    throw;
                }
            }
        }
    }

}