using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Http;
using System.IO;
using Applications;
using Interfaces.Models;

namespace FileComparer.Controllers
{
    [Route("api/v1/files")]
    public class FilesController : Controller
    {
        readonly IStoreText _fileRepository;

        public FilesController(IStoreText fileRepository)
        {
            _fileRepository = fileRepository;
        }

        // GET api/v1/files
        [HttpGet]
        public async Task<IEnumerable<TextFile>> GetAsync()
        {
            return await _fileRepository.GetFilesAsync();
        }

        // GET api/v1/files/5
        [HttpGet("{id}")]
        public async Task<TextFile> GetAsync(string id)
        {
            return await _fileRepository.GetFileAsync(id);
        }

        // POST api/v1/files/5
        [SwaggerOperation(Tags = new[] { "FileUpload" })]
        [HttpPost("{id}")]
        public async Task<IActionResult> PostAsync(string id, IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var reader = new StreamReader(stream);
                var data = await reader.ReadToEndAsync();
                var textFile = new TextFile {
                    Id = id,
                    Data = data
                };

                await _fileRepository.SaveFileAsync(textFile);

                return Created(@"api/v1/files/{id}", textFile);
            } 
        }

        // DELETE api/v1/files/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _fileRepository.DeleteFileAsync(id);
            return Ok();
        }
    }
}
