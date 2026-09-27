using System;
using System.IO;
using System.Threading.Tasks;
using Nokubico.Application.Interfaces;

namespace Nokubico.Infra.Services.Storage
{
    public class StorageService : IStorageService
    {
        private readonly string _baseDirectory;

        public StorageService(string baseDirectory)
        {
            _baseDirectory = string.IsNullOrEmpty(baseDirectory) ? "uploads" : baseDirectory;
        }

        public Task<string> UploadAsync(byte[] content, StorageFolder folder, string fileName, string contentType)
        {
            return Task.Run(() =>
            {
                var safeName = SanitizeFileName(fileName);
                var relativePath = FolderName(folder) + "/" + safeName;
                var directory = Path.Combine(_baseDirectory, FolderName(folder));

                Directory.CreateDirectory(directory);
                File.WriteAllBytes(Path.Combine(directory, safeName), content);

                return "/" + relativePath;
            });
        }

        public Task DeleteAsync(string url)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(url))
                {
                    return;
                }

                var relative = url.TrimStart('/').Replace("/", "\\");
                var fullPath = Path.Combine(_baseDirectory, relative);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            });
        }

        private string FolderName(StorageFolder folder)
        {
            return folder.ToString().ToLowerInvariant();
        }

        private string SanitizeFileName(string fileName)
        {
            var name = string.IsNullOrEmpty(fileName) ? "file" : fileName;
            name = name.Replace("..", "").Replace("/", "_").Replace("\\", "_");
            var prefix = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
            return prefix + "_" + name;
        }
    }
}