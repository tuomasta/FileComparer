using Interfaces.Models;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Integrations.Tests
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostFileAsync(this HttpClient client, TextFile file)
        {
            using (var content = new MultipartFormDataContent())
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(file.Data);
                //var encoded = Convert.ToBase64String(plainTextBytes);
                //var bytes = Convert.FromBase64String(encoded);
                content.Add(new StreamContent(new MemoryStream(plainTextBytes)), "file", "file.txt");
                return await client.PostAsync($"api/v1/files/{file.Id}", content);
            }
        }
    }
}
