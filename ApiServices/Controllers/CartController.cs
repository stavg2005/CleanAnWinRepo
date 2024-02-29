using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartControllercs : ControllerBase
    {
        [HttpGet($"GetAllProducts")]
        public async Task<List<Product>> GetAllProducts(int id)
        {
            return  await UsersDTO.GetCart(id);
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
        public async Task<IActionResult> DeleteProductFromUserCart(Tuple<int,int> userIDproductID)
        {
            try
            {
                await UsersDTO.DeleteProductFromUserCart(userIDproductID.Item1, userIDproductID.Item2);
                return Ok("Cart operation completed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}