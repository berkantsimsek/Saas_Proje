using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaaSProje.Data;
using SaaSProje.Models;

namespace SaaSProje.Controllers
{
    [Authorize] // Tokunu olmayan (giriş yapmayan) kimseyi içeri almaz
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            // Global Query Filter devrede olduğu için Where(FirmId == id) yazmaya gerek yok!
            var products = _context.Products.ToList();
            return Ok(products);
        }

        [HttpPost]
        public IActionResult AddProduct([FromBody] Product request)
        {
            // FirmId'yi manuel vermiyoruz, DbContext SaveChanges içinde bunu kendisi ekleyecek.
            _context.Products.Add(request);
            _context.SaveChanges();

            return Ok(new { Message = "Ürün başarıyla eklendi." });
        }
    }
}