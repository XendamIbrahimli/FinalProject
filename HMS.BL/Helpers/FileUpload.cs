using HMS.BL.Services;
using HMS.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.BL.Helpers
{
    public static class FileUpload
    {
        public static async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            string _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
            if (!Directory.Exists(_uploadFolderPath))
            {
                Directory.CreateDirectory(_uploadFolderPath);
            }
            if (!file.ContentType.StartsWith("image"))
                throw new FileUploadException("File type must be image");
            else if (file == null || file.Length == 0)
                throw new FileUploadException("Invalid file");
            else if (file.Length > 600 * 1024)
                throw new FileUploadException("File length must be less than 600kb");

            string fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            using (Stream stream = new FileStream(Path.Combine(_uploadFolderPath, fileName), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"{folderName}/{fileName}";
        }
    }
}
