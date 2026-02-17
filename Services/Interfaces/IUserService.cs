using TaskWebApi.Common;
using TaskWebApi.DTOs;
using TaskWebApi.Models;

namespace TaskWebApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<List<UserDto>>> GetAllAsync();
        Task<ServiceResult<UserDto>> CreateAsync(CreateUserDto dto);
    }
}
