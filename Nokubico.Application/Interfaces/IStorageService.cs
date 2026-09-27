using System.Threading.Tasks;

namespace Nokubico.Application.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadAsync(byte[] content, StorageFolder folder, string fileName, string contentType);

        Task DeleteAsync(string url);
    }
}