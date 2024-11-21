

namespace Backend.Application.Core
{
    public class ResponseResult
    {

        public ResponseResult(object data,int? statusCode, string message,bool isSucess)
        {
            Data = data;
            StatusCode = statusCode;
            IsSucess = isSucess;
            Message = message;
           
           // NumberOfRecords = numberOfRecords;

        }
        public object Data { get; set; }
        public int? StatusCode { get; set; }
        public string Message { get; set; }
        public bool IsSucess { get; set; }

        // public int? NumberOfRecords { get; set; }


    }
}
