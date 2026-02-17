using Microsoft.EntityFrameworkCore;
using TaskWebApi.Common;
using TaskWebApi.Data;
using TaskWebApi.DTOs;
using TaskWebApi.Models;
using TaskWebApi.Services.Interfaces;
using static TaskWebApi.Common.Enum;

namespace TaskWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        #region GetAllAsync
        public async Task<ServiceResult<List<UserDto>>> GetAllAsync()
        {
            try
            {
                var users = await _context.Users.ToListAsync();

                if (!users.Any())
                    return ServiceResult<List<UserDto>>.Fail(String.Format(Messages.FieldNotFound,"User"), ServiceErrorType.NotFound);

                var result = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    CreatedAt = u.CreatedAt
                }).ToList();

                return ServiceResult<List<UserDto>>.Ok(result, Messages.Success);
            }
            catch (Exception ex) 
            {
                // log table is not managed. so, returning exception message.
                return ServiceResult<List<UserDto>>.Fail(ex.Message, ServiceErrorType.Exception);
            }
        }
        #endregion

        #region CreateAsync
        public async Task<ServiceResult<UserDto>> CreateAsync(CreateUserDto dto)
        {
            try
            {
                var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);

                if (emailExists)
                {
                    return ServiceResult<UserDto>.Fail(String.Format(Messages.FieldAlreadyExists, dto.Email), ServiceErrorType.Conflict);
                }

                var user = new User
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var result = new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt
                };

                return ServiceResult<UserDto>.Ok(result, String.Format(Messages.FieldCreated,"User"));
            }
            catch (Exception ex)
            {
                // log table is not managed. so, returning exception message.
                return ServiceResult<UserDto>.Fail(ex.Message, ServiceErrorType.Exception);
            }
        }
        #endregion
    }
}
