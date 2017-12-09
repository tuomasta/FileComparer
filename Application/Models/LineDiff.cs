using DiffPlex.DiffBuilder.Model;

namespace Interfaces.Models
{
    public class LineDiff
    {
        public LineDiff(string newLine, string oldline, int lineNumber, ChangeType change)
        {
            New = newLine;
            Old = oldline;
            LineNumber = lineNumber;
            Change = change;
        }

        public int LineNumber { get; }
        public ChangeType Change { get; }

        /// <summary>
        /// Line in the new file
        /// </summary>
        public string New { get; }

        /// <summary>
        /// Line in the old file
        /// </summary>
        public string Old { get; }
    }
}