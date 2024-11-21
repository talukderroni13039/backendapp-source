using AspNetCore.Reporting;
using AspNetCore.Reporting.ReportExecutionService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Reporting.Service
{
    public interface IReportService
    {
        Task<byte[]> GenerateReport<T>(string reportPath, string datasetName, T datasetSource, Dictionary<string, string> parameter, string reportType);
    }

    public class ReportService : IReportService
    {
        public ReportService()
        {

        }
        public async Task<byte[]> GenerateReport<T>(string reportPath, string datasetName, T datasetSource, Dictionary<string, string> parameters, string reportType)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding.GetEncoding("windows-1252");
                LocalReport report = new LocalReport(reportPath);

                // Add dataset if provided
                if (datasetSource != null && !string.IsNullOrEmpty(datasetName))
                {
                    report.AddDataSource(datasetName, datasetSource);
                }
                // Execute report rendering
                var result = await Task.Run(() => report.Execute(GetRenderType(reportType), 1, parameters));

                return result.MainStream;

            }
            catch (Exception ex)
            {
                throw ex;
            }
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



    }
}
