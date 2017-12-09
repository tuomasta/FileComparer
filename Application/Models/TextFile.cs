namespace Interfaces.Models
{
    /// <summary>
    /// Specifies a text file
    /// </summary>
    public class TextFile
    {
        /// <summary>
        ///  Id of the file
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Data / content of the file
        /// </summary>
        public string Data { get; set; }
    }
}
