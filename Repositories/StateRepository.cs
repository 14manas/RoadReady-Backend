using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadReady.DTO;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class StateRepository : IStateRepository
    {
        private readonly RoadReadyContext _context;
        private readonly IMapper _mapper;

        public StateRepository(RoadReadyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StateDTO>> GetAllAsync()
        {
            try
            {
                var states = await _context.States.ToListAsync();
                return _mapper.Map<IEnumerable<StateDTO>>(states);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<StateDTO?> GetByIdAsync(int id)
        {
            try
            {
                var state = await _context.States.FindAsync(id);
                return _mapper.Map<StateDTO>(state);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<StateDTO> CreateAsync(StateDTO stateDto)
        {
            try
            {
                var state = _mapper.Map<State>(stateDto);
                _context.States.Add(state);
                await _context.SaveChangesAsync();
                return _mapper.Map<StateDTO>(state);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<StateDTO> UpdateAsync(int id, StateDTO stateDto)
        {
            try
            {
                var state = await _context.States.FindAsync(id);
                if (state == null) throw new KeyNotFoundException("State not found.");

                _mapper.Map(stateDto, state);
                await _context.SaveChangesAsync();
                return _mapper.Map<StateDTO>(state);
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
                var state = await _context.States.FindAsync(id);
                if (state == null) return false;

                _context.States.Remove(state);
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
