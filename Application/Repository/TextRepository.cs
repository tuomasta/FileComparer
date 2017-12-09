using Application.Repository;
using Interfaces.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Applications.Repository
{
    /// <summary>
    /// Repository implementation
    /// </summary>
    public class TextRepository : IStoreText
    {
        private readonly Context _context;
        public TextRepository(Context context)
        {
            _context = context;
        }

        public async Task DeleteFileAsync(string id)
        {
            var file = await _context.Files.FindAsync(id);
            if (file == null) return;
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TextFile>> GetFilesAsync()
        {
            return await _context.Files.ToListAsync();
        }

        public async Task<TextFile> GetFileAsync(string id)
        {
            return await _context.Files.FindAsync(id);
        }

        public async Task SaveFileAsync(TextFile file)
        {
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
        }
    }
}