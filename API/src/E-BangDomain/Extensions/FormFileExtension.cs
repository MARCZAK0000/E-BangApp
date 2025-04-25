using Microsoft.AspNetCore.Http;

namespace E_BangDomain.Extensions
{
    public static class FormFileExtension
    {
        public static async Task<byte[]> GetBytesAsync(this IFormFile file)
        {
            await using var memoryStrem = new MemoryStream();
            await file.CopyToAsync(memoryStrem);
            return memoryStrem.ToArray();
        }
    }
}
