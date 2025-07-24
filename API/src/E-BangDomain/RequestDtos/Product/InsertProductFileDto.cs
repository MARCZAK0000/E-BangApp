using Microsoft.AspNetCore.Http;

namespace E_BangDomain.RequestDtos.Product
{
    public class InsertProductFileDto
    {
        public string ProductID { get; set; }
        public IFormFile File { get; set; }
    }
}
