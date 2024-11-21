using Backend.Application.Interface.ReportRepository;
using Backend.Infrastructure.Database;
using Backend.Reporting.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services.ReportRepository
{
    public class ProductReportService : IProductReportService
    {
        private readonly IReportService _iReportService;
        private readonly ApplicationDBContext _dbContext;
        public ProductReportService(IReportService iReportService, ApplicationDBContext dbContext)
        {
            _iReportService = iReportService;
            _dbContext = dbContext;
        }
        public async Task<byte[]> generateSummary()
        {
            //get data from the Database
            var dataSource = _dbContext.Products.AsNoTracking()
                                                .AsQueryable();

            var reportName = "ProcductSummary.rdlc";
            var reportPath= Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "RDLC", reportName);

            byte[] file= await _iReportService.GenerateReport(reportPath, "DataSet1", dataSource.ToList(), null, "pdf");
            
            return file;
        }
    }
}
