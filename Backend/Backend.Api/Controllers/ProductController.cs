
using Backend.Application.Core;
using Backend.Application.DTO.Entity;
using Backend.Application.Services;
using Backend.Application.Validation;
using Backend.Common;
using Backend.Common.Models;
using Backend.Infrastructure.Database;
using Backend.Infrastructure.Interface.Email;
using Backend.Infrastructure.Interface.OTP;
using Backend.Infrastructure.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.Data.SqlClient;
using System.Text;
using AspNetCore.Reporting;
using Backend.Application.Interface.ReportRepository;
using Backend.Application.Interface.DomainRepository;

namespace Backend.Api.Controllers
{

    //  [ApiVersion("1.0")]
    public class ProductController : BaseApiController
    {
        private readonly ApplicationDBContext _applicationDBContext;
        private readonly IDatabaseHelper _iDatabaseHelper;
        private readonly ProductValidator _productValidator;
        private readonly ProductsPayloadHandlerService _productPayloadHandler;
        private readonly IEmailService _iEmailService;
        private readonly IProductRepository _iProductRepository;
        private readonly IOTPService _iOTPService;

        private readonly IProductReportService _iProductReportService;

        //private readonly IReportService<T> _iReportService;
        public ProductController(IProductRepository iProductRepository, IOTPService OTPService, IEmailService iEmailService, ApplicationDBContext applicationDBContext, IDatabaseHelper iDatabaseHelper, ProductsPayloadHandlerService productPayloadHandler, ProductValidator productValidator, IProductReportService iProductReportService)
        {
            _applicationDBContext = applicationDBContext;
            _iDatabaseHelper = iDatabaseHelper;
            _productPayloadHandler = productPayloadHandler;
            _productValidator = productValidator;
            _iProductRepository = iProductRepository;

            _iOTPService = OTPService;
            _iEmailService = iEmailService;
            _iProductReportService = iProductReportService;
        }

        [ApiVersion("1.0")]
        [HttpPost]
        [Route("SendEmail")]
        public async Task<ActionResult>  SendEmail()   //[FromQuery] ProductDTO productDTO  //if model not match gives 400 err
        {
            EmailInfo emailInfo = new EmailInfo();
            emailInfo.Subject = "test";
            emailInfo.Body = "test body";
            emailInfo.To = "talukder.roni.ict@gmail.com";

            var files = "E:\\Talukder\\CleanArchitecture\\Projets\\Backend.Api\\Files\\Product\\BGDDV5B87524KH931215.pdf-TCVSSNNJ.pdf";
           
            emailInfo.Files =new List<string> { files };
            var isSent = false;

            //for (int i = 0; i < 500; i++)
            //{
            //    isSent = await _iEmailService.SendEmail(emailInfo);

            //}

           var isSenassat= await _iOTPService.SendOTP("+8801515279243",null);

            return Ok(isSenassat);
        }

        [AllowAnonymous]
        [ApiVersion("1.0")]
        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<ActionResult> GetAllProducts()   //[FromQuery] ProductDTO productDTO  //if model not match gives 400 err
        {
            var result= await _iProductRepository.GetAllProducts();
            return Ok(result);
        }

        [AllowAnonymous]
        [ApiVersion("1.0")]
        [HttpGet]
        [Route("getproducts")]
        public async Task<ActionResult> Get()   //[FromQuery] ProductDTO productDTO  //if model not match gives 400 err
        {
            var Module = "HR";
            var Page = "Create Employee";

            Log.Error("Before logging information");
            Log.Error("Get method called two module {Module} on page {Page}", Module, Page);

            // Log.Information("After logging information");
            // call stored procedure 

                SqlParameter[] parameters =
                                        {
                                            new SqlParameter("@Id", DBNull.Value),
                                        };
                var result = await _iDatabaseHelper.ExecuteStoredProcedureAsync("GetProductById", parameters);

                    // Serialize the DataTable to a JSON string
                string jsonResult = JsonConvert.SerializeObject(result, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                // Print the JSON string
                Console.WriteLine(jsonResult);

                var s = new PageRequest();
                var data=  await _applicationDBContext.Products.AsNoTracking()
                                                        .PageBy(s)
                                                        .ToListAsync();

                if (data.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    var response=new ResponseResult(jsonResult, 200,"Sucess",true);
                    return Ok(response);
                }
            
        }


      
        [HttpGet]
        [Route("GetProductsV2")]
        [ApiVersion("2.0")]
        public async Task<ActionResult> GetProductsV2()   //[FromQuery] ProductDTO productDTO  //if model not match gives 400 err
        {
            var Module = "HR";
            var Page = "Create Employee";

            Log.Error("Before logging information");
            Log.Error("Get method called two module {Module} on page {Page}", Module, Page);

            // Log.Information("After logging information");
            // call stored procedure 

            SqlParameter[] parameters =
                                    {
                                            new SqlParameter("@Id", DBNull.Value),
                                        };
            var result = await _iDatabaseHelper.ExecuteStoredProcedureAsync("GetProductById", parameters);

            // Serialize the DataTable to a JSON string
            string jsonResult = JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            // Print the JSON string
            Console.WriteLine(jsonResult);

            var s = new PageRequest();
            var data = await _applicationDBContext.Products.AsNoTracking().PageBy(s)
            .ToListAsync();

            if (data.Count == 0)
            {
                return NotFound();
            }
            else
            {
                var response = new ResponseResult(jsonResult, 200, "Sucess", true);
                return Ok(response);
            }

        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductController>
       // [ApiController]
     
        [ApiVersion("1.0")]
        [HttpPost("PostAsync")]
        public async Task<IActionResult> PostAsync([FromBody] ProductDTO productDTO)
       {
            ValidationResult validationResult = await _productValidator.ValidateAsync(productDTO);

            if (!validationResult.IsValid)
            {
                // Return validation errors
                return HandleBadRequest(validationResult);
            }

            await _productPayloadHandler.Extract(productDTO)
                                        .Transform()
                                        .Load();

            //finalyy return dto with id 
            var data = _productPayloadHandler.GetLoadedPayload();

            return HandleResult(data);
       }
        [ApiVersion("1.0")]
        [HttpPost("SaveProductMasterDetils")]
        public async Task<IActionResult> SaveProductMasterDetils([FromBody] ProductMasterDetailDTO productMasterDetailDTO)
        {
            //// ValidationResult validationResult = await _productValidator.ValidateAsync(productDTO);

            // if (!validationResult.IsValid)
            // {
            //     // Return validation errors
            //     return HandleBadRequest(validationResult);
            // }


            var productDTO = await _iProductRepository.CreateProduct(productMasterDetailDTO.productDTO);
            var productDetilsDTO = await _iProductRepository.UpsertProductDetils(productMasterDetailDTO.productDetilsDTO);

            return HandleResult(productDTO);
        }




        [HttpPost("upload")]
        public async Task<IActionResult> PostFileAsync([FromForm] ProductDTO productDTO)
        {
            ValidationResult validationResult = await _productValidator.ValidateAsync(productDTO);

            //if (!validationResult.IsValid)
            //{
            //    // Return validation errors
            //    return HandleBadRequest(validationResult);
            //}

            await _productPayloadHandler.Extract(productDTO)
                                        .Transform()
                                        .UploadFile()
                                        .Load();

            //finalyy return dto with id 
            var data = _productPayloadHandler.GetLoadedPayload();
            return HandleResult(data);
        }


        [AllowAnonymous]
        [ApiVersion("1.0")]
        [HttpGet]
        [Route("GeneratePDf")]
        public async Task<ActionResult> GeneratePDf()
        {

            var file=  await _iProductReportService.generateSummary();
            //to open in browser Instead of download
            Response.Headers["Content-Disposition"] = "inline; filename=" + "demo" + ".pdf";

            //return Excel
            return File(file, "application/msexcel", "Export.xls");

            //Return PDf
           // return File(file, "application/pdf");


            //return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".pdf");
        }
        private byte[] GenerateReportAsync(string reportName)
        {
            // string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("ReportAPI.dll", string.Empty);

            string rdlcFilePath = "D:\\Talukder_GitHub\\ApiTemplate\\Backend\\Backend.Reporting\\RDLC\\Report1.rdlc";


            //string rdlcFilePath = string.Format("{0}ReportFiles\\{1}.rdlc", fileDirPath, reportName);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);
    
            var result = report.Execute(GetRenderType("pdf"), 1, parameters);
            return result.MainStream;
        }

        private RenderType GetRenderType(string reportType)
        {
            var renderType = RenderType.Pdf;
            switch (reportType.ToLower())
            {
                default:
                case "pdf":
                    renderType = RenderType.Pdf;
                    break;
                case "word":
                    renderType = RenderType.Word;
                    break;
                case "excel":
                    renderType = RenderType.Excel;
                    break;
            }

            return renderType;
        }




        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

      
        
        
        
        
        
        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
