using TaskWebApi.Common;
using TaskWebApi.DTOs;

namespace TaskWebApi.Services.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult<List<ProductDto>>> GetAllAsync();
        Task<ServiceResult<ProductDto>> GetByIdAsync(long id);
        Task<ServiceResult<ProductDto>> CreateAsync(CreateProductDto dto);
        Task<ServiceResult<ProductDto>> UpdateAsync(long id, UpdateProductDto dto);
    }
}
