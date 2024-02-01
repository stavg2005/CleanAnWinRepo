using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiServices.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAllProducts")]
        public async Task<List<Product>> GetAllProducts()
        {
            return await (ProductDTO.GetAllProducts());
        }



        [HttpGet("GetProuctFromIndex")]
        public async Task<Product> GetProuctFromIndex(int index)
        {
            return await ProductDTO.GetProuctFromIndex(index);
        }

        [HttpPost("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(Product p)
        {
            try
            {
                await ProductDTO.UpdateProduct(p);
                return Ok("Update Operation completed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
