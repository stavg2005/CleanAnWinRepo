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

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody]Product p)
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

        [HttpPost("InsertNewProduct")]
        public async Task<IActionResult> InsertNewProduct([FromBody] Product p)
        {
            try
            {
                await ProductDTO.InsertNewProduct(p);
                return Ok("Insert Operation completed successfully. ");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
