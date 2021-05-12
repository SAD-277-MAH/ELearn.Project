using ELearn.Data.Dtos.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Services.Upload.Interface
{
    public interface IUploadService
    {
        Task<FileForUploadDto> UploadFile(IFormFile file, string url, string path);
        Task<FileForUploadDto> UploadFileToLocal(IFormFile file, string url, string path);
        FileForUploadDto RemoveFileFromLocal(string filePath);
    }
}
