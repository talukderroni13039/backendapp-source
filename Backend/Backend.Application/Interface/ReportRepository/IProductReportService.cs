using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Interface.ReportRepository
{
    public interface IProductReportService
    {
       Task<byte[]> generateSummary();
    }
}
