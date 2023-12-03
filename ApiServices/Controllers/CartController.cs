using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace ApiServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartControllercs : ControllerBase
    {
        [HttpGet($"GetAllProducts")]
        public IActionResult GetAllProducts(int id)
        {
            return Ok(UsersDTO.GetCart(id));
        }

        [HttpPost("AddProductTocart")]
        public async Task<IActionResult> AddProductToCart(int userid, int productid)
        {
            try
            {
                await ProductDTO.AddTocart(userid, productid);
                return Ok("Cart operation completed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}