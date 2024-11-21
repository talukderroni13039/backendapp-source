using Backend.Application.Core;
using Backend.Application.DTO.Entity;
using Backend.Domain.Entites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Interface.DomainRepository
{
    public interface IProductRepository
    {
        Task<Product> CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int id);
        Task<Result<string>> GetAllProducts();
        Task<bool> GetProductById();
        Task<bool> CheckduplicateProductByName(string name); // used in validator

        Task<List<ProductDetail>> UpsertProductDetils(List<ProductDetailDTO> product);

        //   Task<bool> GenerateProductReport(string name);

    }
}
