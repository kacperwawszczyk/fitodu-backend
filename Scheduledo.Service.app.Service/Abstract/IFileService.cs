using System;
using System.IO;
using System.Threading.Tasks;
using Scheduledo.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Scheduledo.Service.Abstract
{
    public interface IFileService
    {
        Task Save(IFormFile file, FolderName folder, Guid fileId, bool isPrivate = true);
        Task<MemoryStream> Get(FolderName folder, Guid fileId, string fileName);
        Task<bool> Exists(FolderName folder, Guid fileId, string fileName);
        Task<bool> Delete(FolderName folder, Guid fileId, string fileName);
    }
}
