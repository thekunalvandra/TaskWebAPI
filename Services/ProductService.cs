using Microsoft.EntityFrameworkCore;
using TaskWebApi.Common;
using TaskWebApi.Data;
using TaskWebApi.DTOs;
using TaskWebApi.Models;
using TaskWebApi.Services.Interfaces;
using static TaskWebApi.Common.Enum;

namespace TaskWebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        #region GetAllAsync
        public async Task<ServiceResult<List<ProductDto>>> GetAllAsync()
        {
            try
            {
                var products = await _context.Products.Where(x => x.IsActive).ToListAsync();

                if (!products.Any())
                    return ServiceResult<List<ProductDto>>.Fail(String.Format(Messages.FieldNotFound, "Product"), ServiceErrorType.NotFound);

                var result = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt
                }).ToList();

                return ServiceResult<List<ProductDto>>.Ok(result, Messages.Success);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<ProductDto>>.Fail(ex.Message, ServiceErrorType.Exception);
            }
        }
        #endregion

        #region GetByIdAsync
        public async Task<ServiceResult<ProductDto>> GetByIdAsync(long id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id && x.IsActive); 

                if (product == null)
                    return ServiceResult<ProductDto>.Fail(String.Format(Messages.FieldNotFound, "Product"), ServiceErrorType.NotFound);

                var result = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    IsActive = product.IsActive,
                    CreatedAt = product.CreatedAt
                };

                return ServiceResult<ProductDto>.Ok(result, Messages.Success);
            }
            catch (Exception ex)
            {
                return ServiceResult<ProductDto>.Fail(ex.Message, ServiceErrorType.Exception);
            }
        }
        #endregion

        #region CreateAsync
        public async Task<ServiceResult<ProductDto>> CreateAsync(CreateProductDto dto)
        {
            try
            {
                var product = new Product
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var result = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    IsActive = product.IsActive,
                    CreatedAt = product.CreatedAt
                };

                return ServiceResult<ProductDto>.Ok(result, String.Format(Messages.FieldCreated, "Product"));
            }
            catch (Exception ex)
            {
                return ServiceResult<ProductDto>.Fail(ex.Message, ServiceErrorType.Exception);
            }
        }
        #endregion

        #region UpdateAsync
        public async Task<ServiceResult<ProductDto>> UpdateAsync(long id, UpdateProductDto dto)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                    return ServiceResult<ProductDto>.Fail(String.Format(Messages.FieldNotFound, "Product"), ServiceErrorType.NotFound);

                product.Name = dto.Name;
                product.Price = dto.Price;
                product.IsActive = dto.IsActive;

                await _context.SaveChangesAsync();

                var result = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    IsActive = product.IsActive,
                    CreatedAt = product.CreatedAt
                };

                if (result.IsActive == true)
                {
                    return ServiceResult<ProductDto>.Ok(result, String.Format(Messages.FieldUpdated, "Product"));
                }
                else
                {
                    return ServiceResult<ProductDto>.Ok(result, String.Format(Messages.FieldDeleted, "Product"));
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<ProductDto>.Fail(ex.Message, ServiceErrorType.Exception);
            }
        }
        #endregion
    }
}
