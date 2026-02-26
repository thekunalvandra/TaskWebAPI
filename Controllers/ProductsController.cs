using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskWebApi.Common;
using TaskWebApi.DTOs;
using TaskWebApi.Services;
using TaskWebApi.Services.Interfaces;
using static TaskWebApi.Common.Enum;

namespace TaskWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var result = await _productService.GetAllAsync();

            if (!result.Success)
                return HandleFailure(result);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(long id)
        {
            var result = await _productService.GetByIdAsync(id);

            if (!result.Success)
                return HandleFailure(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto dto)
        {
            var result = await _productService.CreateAsync(dto);

            if (!result.Success)
                return HandleFailure(result);

            return Created(string.Empty, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(long id, UpdateProductDto dto)
        {
            var result = await _productService.UpdateAsync(id, dto);

            if (!result.Success)
                return HandleFailure(result);

            return Ok(result);
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
