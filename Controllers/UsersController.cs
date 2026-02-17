using Microsoft.AspNetCore.Mvc;
using TaskWebApi.Common;
using TaskWebApi.DTOs;
using TaskWebApi.Services.Interfaces;
using static TaskWebApi.Common.Enum;

namespace TaskWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var result = await _userService.GetAllAsync();

            if (!result.Success)
                return HandleFailure(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            var result = await _userService.CreateAsync(dto);

            if (!result.Success)
                return HandleFailure(result);

            return Created(string.Empty, result);
        }

        private IActionResult HandleFailure<T>(ServiceResult<T> result)
        {
            return result.ErrorType switch
            {
                ServiceErrorType.Validation => BadRequest(result.Message), // 400
                ServiceErrorType.NotFound => NotFound(result.Message),   // 404
                ServiceErrorType.Conflict => Conflict(result.Message),   // 409
                ServiceErrorType.Exception => StatusCode(500, result.Message), // 500
                _ => StatusCode(500, Messages.UnexpectedError)
            };
        }
    }
}
