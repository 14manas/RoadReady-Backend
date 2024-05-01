using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadReady.DTO;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class CarReviewRepository : ICarReviewRepository
        {
            private readonly RoadReadyContext _context;
            private readonly IMapper _mapper;

            public CarReviewRepository(RoadReadyContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<CarReviewDTO>> GetAllAsync()
            {
                try
                {
                    var carReviews = await _context.CarReviews.ToListAsync();
                    return _mapper.Map<IEnumerable<CarReviewDTO>>(carReviews);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public async Task<CarReviewDTO?> GetByIdAsync(int id)
            {
                try
                {
                    var carReview = await _context.CarReviews.FindAsync(id);
                    return _mapper.Map<CarReviewDTO>(carReview);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public async Task<CarReviewDTO> CreateAsync(CarReviewDTO carReviewDto)
            {
                try
                {
                    var carReview = _mapper.Map<CarReview>(carReviewDto);
                    _context.CarReviews.Add(carReview);
                    await _context.SaveChangesAsync();
                    return _mapper.Map<CarReviewDTO>(carReview);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public async Task<CarReviewDTO> UpdateAsync(int id, CarReviewDTO carReviewDto)
            {
                try
                {
                    var carReview = await _context.CarReviews.FindAsync(id);
                    if (carReview == null) throw new KeyNotFoundException("CarReview not found.");

                    _mapper.Map(carReviewDto, carReview);
                    await _context.SaveChangesAsync();
                    return _mapper.Map<CarReviewDTO>(carReview);
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
                    var carReview = await _context.CarReviews.FindAsync(id);
                    if (carReview == null) return false;

                    _context.CarReviews.Remove(carReview);
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

