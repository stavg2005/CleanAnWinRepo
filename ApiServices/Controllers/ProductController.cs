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

        ProductDTO p = new ProductDTO();
        [HttpGet("GetAllProducts")]
        public async Task<List<Product>> GetAllProducts()
        {
            return await (p.SelectAll());
        }



        [HttpGet("GetProuctFromIndex")]
        public async Task<Product> GetProuctFromIndex(int index)
        {
            return await p.GetByPK(index);
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody]Product pr)
        {
            try
            {
                await p.Update(pr);
                return Ok("Update Operation completed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("InsertNewProduct")]
        public async Task<IActionResult> InsertNewProduct([FromBody] Product pr)
        {
            try
            {
                await p.Insert(pr);
                return Ok("Insert Operation completed successfully. ");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
