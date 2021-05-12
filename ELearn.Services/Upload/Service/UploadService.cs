using ELearn.Data.Context;
using ELearn.Data.Dtos.Services;
using ELearn.Repo.Infrastructure;
using ELearn.Services.Upload.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Upload.Service
{
    public class UploadService : IUploadService
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadService(IUnitOfWork<DatabaseContext> db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<FileForUploadDto> UploadFile(IFormFile file, string url, string path)
        {
            return await UploadFileToLocal(file, url, path);
        }

        public async Task<FileForUploadDto> UploadFileToLocal(IFormFile file, string url, string path)
        {
            if (file.Length > 0)
            {
                try
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string fileExtension = Path.GetExtension(fileName);
                    string fileNewName = string.Format("{0}{1}", Guid.NewGuid().ToString(), fileExtension);
                    string filePath = Path.Combine(_hostingEnvironment.WebRootPath, path);
                    string fullPath = Path.Combine(filePath, fileNewName);
                    Directory.CreateDirectory(filePath);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);

                        return new FileForUploadDto()
                        {
                            Status = true,
                            Message = "فایل با موفقیت در فضای ابری آپلود شد",
                            Url = string.Format("{0}/{1}", url, "wwwroot/" + path.Replace('\\', '/') + "/" + fileNewName)
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new FileForUploadDto()
                    {
                        Status = false,
                        Message = ex.Message
                    };
                }
            }
            else
            {
                return new FileForUploadDto()
                {
                    Status = false,
                    Message = "فایلی برای آپلود یافت نشد"
                };
            }
        }

        public FileForUploadDto RemoveFileFromLocal(string filePath)
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, filePath);

            if (File.Exists(path))
            {
                File.Delete(path);

                return new FileForUploadDto()
                {
                    Status = true,
                    Message = "فایل با موفقیت از فضای ابری حذف شد"
                };
            }
            else
            {
                return new FileForUploadDto()
                {
                    Status = false,
                    Message = "فایل یافت نشد"
                };
            }
        }
    }
}
