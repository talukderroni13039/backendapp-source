using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;


namespace Backend.Common.FileUploadHelper
{
    public static class FileUploader
    {
        public static UploadResponseModel Upload(IFormFile file, string subfolder = "Submodule", string fileName=null)
        {
            var response = new UploadResponseModel();
            
            try
            {
                var folderName = Path.Combine("Files", subfolder);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                bool exists = Directory.Exists(pathToSave);

                if (!exists)
                    Directory.CreateDirectory(pathToSave);

                if (file.Length > 0)
                {
                    
                    fileName = fileName + "-" + Common.RandomString(8) + Path.GetExtension(file.FileName);
        
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    response.Status = true;
                    response.Message = "File Upload Successfully";
                    response.FilePath = dbPath;
                }
                else
                {
                    response.Status = false;
                    response.Message = "File Upload failed";
                    response.FilePath = string.Empty;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                response.FilePath = string.Empty;
            }

            return response;
        }
    }
}