using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadReady.DTO;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class UserTypeRepository : IUserTypeRepository
    {
        private readonly RoadReadyContext _context;
        private readonly IMapper _mapper;

        public UserTypeRepository(RoadReadyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserTypeDTO>> GetAllAsync()
        {
            try
            {
                var userTypes = await _context.Usertypes.ToListAsync();
                return _mapper.Map<IEnumerable<UserTypeDTO>>(userTypes);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserTypeDTO?> GetByIdAsync(int id)
        {
            try
            {
                var userType = await _context.Usertypes.FindAsync(id);
                return _mapper.Map<UserTypeDTO>(userType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserTypeDTO> CreateAsync(UserTypeDTO userTypeDto)
        {
            try
            {
                var userType = _mapper.Map<Usertype>(userTypeDto);
                _context.Usertypes.Add(userType);
                await _context.SaveChangesAsync();
                return _mapper.Map<UserTypeDTO>(userType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserTypeDTO> UpdateAsync(int id, UserTypeDTO userTypeDto)
        {
            try
            {
                var userType = await _context.Usertypes.FindAsync(id);
                if (userType == null) throw new KeyNotFoundException("UserType not found.");

                _mapper.Map(userTypeDto, userType);
                await _context.SaveChangesAsync();
                return _mapper.Map<UserTypeDTO>(userType);
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
                var userType = await _context.Usertypes.FindAsync(id);
                if (userType == null) return false;

                _context.Usertypes.Remove(userType);
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
