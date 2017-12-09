using Interfaces.Models;

namespace Applications
{
    /// <summary>
    /// Contract to compare files
    /// </summary>
    public interface ICompareFiles
    {
        FileDiff Diff(TextFile rightFile, TextFile leftFile);
    }
}