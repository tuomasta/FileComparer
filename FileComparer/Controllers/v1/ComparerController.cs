using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Applications;
using Interfaces.Models;

namespace FileComparer.Controllers
{
    [Route("api/v1/diff")]
    public class ComparerController : Controller
    {
        readonly IStoreText _fileRepository;
        readonly ICompareFiles _fileComparer;

        public ComparerController(
            IStoreText fileRepository,
            ICompareFiles fileComparer)
        {
            _fileRepository = fileRepository;
            _fileComparer = fileComparer;
        }
        
        // GET api/v1/diff/right/5/left/5
        [HttpGet("right/{rightId}/left/{leftId}")]
        public async Task<FileDiff> GetAsync(string rightId, string leftId)
        {
            var rightFile = await _fileRepository.GetFileAsync(rightId);
            var leftFile = await _fileRepository.GetFileAsync(leftId);

            var diff = _fileComparer.Diff(rightFile, leftFile);

            return diff;
        }
    }
}
