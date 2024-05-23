using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartControllercs : ControllerBase
    {
        private readonly UsersDTO u;
        [HttpGet($"GetCart")]
        public async Task<List<Product>> GetCart(int id)
        {
            return  await u.GetUserCart(id);
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

        [HttpPost("DeleteProductFromUserCart")]
        public async Task<IActionResult> DeleteProductFromUserCart([FromBody] Tuple<int,int> userIDproductID)
        {
            try
            {
                await u.DeleteProductFromUserCart(userIDproductID.Item1, userIDproductID.Item2);
                return Ok("Cart operation completed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}