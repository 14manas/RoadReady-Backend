using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RoadReady.DTO;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RoadReadyContext _context;
        private readonly IMapper _mapper;

        public UserRepository(RoadReadyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            try
            {
                var users = await _context.Users.Include(u => u.Usertype).ToListAsync();
                return _mapper.Map<IEnumerable<UserDTO>>(users);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Usertype).FirstOrDefaultAsync(u => u.UserId == id);
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        public async Task<UserDTO> CreateAsync(UserDTO userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                user.Usertype = null;
                user.Usertypeid = userDto.Usertypeid;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<UserDTO> UpdateAsync(int id, UserDTO userDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null) throw new KeyNotFoundException("User not found.");

                // Assuming UserDTO contains a UsertypeId that should  updated
                // Save the UsertypeId (if present) before mapping, then reassign it to avoid altering Usertype
                var usertypeId = userDto.Usertypeid;

                // Maping userDto to user, excluding Usertype to avoid conflicts
                _mapper.Map(userDto, user);

                // Reassign UsertypeId if it's meant to be updated
                if (usertypeId.HasValue) // Assuming UsertypeId is nullable; adjust as per your actual model
                    user.Usertypeid = usertypeId;

                await _context.SaveChangesAsync();
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task<UserDTO> UpdateAsync(int id, UserDTO userDto)
        //{
        //    try
        //    {
        //        var user = await _context.Users.FindAsync(id);
        //        user.Usertype = null;
        //        if (user == null) throw new KeyNotFoundException("User not found.");

        //        _mapper.Map(userDto, user);
        //        await _context.SaveChangesAsync();
        //        return _mapper.Map<UserDTO>(user);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null) return false;

                _context.Users.Remove(user);
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
