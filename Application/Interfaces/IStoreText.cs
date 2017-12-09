using Interfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Applications
{
    /// <summary>
    /// Contract to store text files
    /// </summary>
    public interface IStoreText
    {
        Task<IEnumerable<TextFile>> GetFilesAsync();
        Task<TextFile> GetFileAsync(string id);
        Task SaveFileAsync(TextFile file);
        Task DeleteFileAsync(string id);
    }
}