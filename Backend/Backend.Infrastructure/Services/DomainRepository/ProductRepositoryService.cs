using Backend.Domain.Entites;
using Backend.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Backend.Application.Core;
using Microsoft.AspNetCore.Identity;
using Backend.Application.Interface.DomainRepository;
using Backend.Application.DTO.Entity;

namespace Backend.Infrastructure.Services.DomainRepository
{
    public class ProductRepositoryService : IProductRepository
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IDatabaseHelper _databaseHelper;

        public ProductRepositoryService(ApplicationDBContext dbContext, IDatabaseHelper databaseHelper)
        {
            _dbContext = dbContext;
            this._databaseHelper = databaseHelper;
        }
        public async Task<Product> CreateProduct(Product product)
        {
            var existingProduct = await _dbContext.Products.FindAsync(product.ProductID);
            if (existingProduct != null)
            {
                // Update the existing product
                _dbContext.Entry(existingProduct).CurrentValues.SetValues(product);
            }
            await _dbContext.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return product;
        }
        public async Task<List<ProductDetail>> UpsertProductDetils(List<ProductDetailDTO> productDetils)
        {

            var result = new List<ProductDetail>();

            foreach (var detailDto in productDetils)
            {
                // Convert DTO to entity model using mappper
                var productDetail = new ProductDetail
                {
                    DetailID = detailDto.DetailID,
                    ProductID = detailDto.ProductID,
                    Description = detailDto.Description,
                    // map other properties as needed
                };

                switch (detailDto.tag)
                {
                    case 0:  // No action needed for tag=0 (unchanged)

                        continue;

                    case 1:  // Add new product detail (tag=1)

                        await _dbContext.ProductDetails.AddAsync(productDetail);
                        result.Add(productDetail);
                        break;

                    case 2:     // Update existing product detail (tag=2)
                        var existingDetail = await _dbContext.ProductDetails.FirstOrDefaultAsync(x => x.DetailID == detailDto.DetailID);

                        if (existingDetail != null)
                        {
                            _dbContext.Entry(existingDetail).CurrentValues.SetValues(productDetail);
                            result.Add(existingDetail);
                        }
                        break;

                    case 3: // Delete product detail (tag=3)
                        var detailToDelete = await _dbContext.ProductDetails.FirstOrDefaultAsync(x => x.DetailID == detailDto.DetailID);

                        if (detailToDelete != null)
                        {
                            _dbContext.ProductDetails.Remove(detailToDelete);
                        }
                        break;

                    default:  // Handle unexpected tags if necessary
                        break;
                }
            }

            await _dbContext.SaveChangesAsync();
            return result;


          //  return null;
        }
        public Task<bool> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<string>> GetAllProducts()
        {
            SqlParameter[] parameters = {
                                            new SqlParameter("@Id", DBNull.Value),
                                        };

            var result = await _databaseHelper.ExecuteStoredProcedureAsync("GetProductById", parameters);

            // Serialize the DataTable to a JSON string
            string jsonResult = JsonConvert.SerializeObject(result);
  
            if (jsonResult==null)
            {
                return Result<string>.Failure("There is no Data available. Please create some data for this operation.");
            }
            return Result<string>.Success(jsonResult);

        }

        public Task<bool> GetProductById()
        {
            throw new NotImplementedException();
        }


        public Task<bool> UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> CheckduplicateProductByName(string name) //validator
        {
            var istrue = await _dbContext.Products.Where(x => x.Name.ToUpper()==name.ToUpper()).AnyAsync();

            return istrue;
        }
    }
}
