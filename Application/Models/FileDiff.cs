using System.Collections.Generic;
using System.Linq;

namespace Interfaces.Models
{
    public enum DiffStatus
    {
        /// <summary>
        /// Status is not known
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Files are not equal
        /// </summary>
        NotEqual,

        /// <summary>
        /// Files are equal
        /// </summary>
        Equal
    }

    /// <summary>
    /// Specifies difference between files
    /// </summary>
    public class FileDiff
    {
        public FileDiff(IEnumerable<LineDiff> diff)
        {
            Changes = diff?.OrderBy(line => line.LineNumber)?.ToArray();
            Status = Changes.Any() ? DiffStatus.NotEqual : DiffStatus.Equal;
        }

        public DiffStatus Status { get; }
        public string Summary { get; set; }
        public IReadOnlyCollection<LineDiff> Changes { get; } = new List<LineDiff>();
    }


}