using AutoMapper;
using Backend.Application.Core;
using Backend.Application.DTO.Entity;
using Backend.Application.Interface.DomainRepository;
using Backend.Common.FileUploadHelper;
using Backend.Domain.Entites;
using Microsoft.Extensions.Configuration;
using System.Reflection;


namespace Backend.Application.Services
{
    public class ProductsPayloadHandlerService
    {
        private ProductDTO productDTO = null;
        private Product product = null;
        public ResponseResult responseResult = null;

        private readonly IConfiguration _configuration;
        private readonly IProductRepository _iproductRepository;
        private readonly IMapper _mapper;
        public ProductsPayloadHandlerService(IConfiguration configuration, IMapper mapper, IProductRepository iproductRepository)
        {
            _configuration = configuration;
            _mapper = mapper;
            _iproductRepository = iproductRepository;
        }
        public ProductsPayloadHandlerService Extract(ProductDTO payLoads) // mapp the data
        {
            productDTO = payLoads;
            // get the data from Data points

            product =  _mapper.Map<Product>(payLoads);
            return this;
        }
        public ProductsPayloadHandlerService Transform() // add something on this data 
        {
            // Use the JUST.net library to perform the transformation
           
            return  this;
        }
        public ProductsPayloadHandlerService UploadFile()
        {

            if (productDTO.FileUrl == null)// check invalidator from form validation
            {
                return this;
            }
            // For example:
            var file = productDTO.FileUrl;
            var folderName =typeof( Product).Name;

            var uploadResponse = FileUploader.Upload(file, folderName, file.FileName);
            // You can perform any file uploading logic here based on your requirements

            return this;
        }
        public async Task<ProductsPayloadHandlerService> Load()
        {
            product = await  _iproductRepository.CreateProduct(product);
            return this;
        }
        public ProductDTO GetLoadedPayload() // Return Api Response
        {
            // return api response
            productDTO.ProductID=product.ProductID;

            return productDTO;
        }
    }
}
