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
    }
}