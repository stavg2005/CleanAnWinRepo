using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpGet($"Login")]
        public async Task<Users> GetAllProducts(string email, string password)
        {
            return await (UsersDTO.Login(email, password));
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(string username, string password, string email, int location, int id)
        {
            return Ok(UsersDTO.Register(username, password, email, location, id));
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            return Ok(UsersDTO.GetUserByID(id));
        }

        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(int id, string password)
        {
            try
            {
                await (UsersDTO.UpdatePassword(id, password));
                return Ok("operation completed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("UpdateUserName")]
        public async Task<IActionResult> UpdateUserName(int id, string username)
        {
            try
            {
                await (UsersDTO.UpdateUserName(id, username));
                return Ok("operation completed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("UpdateEmail")]

        public async Task<IActionResult> UpdateUserEmail(int id, string username)
        {
            try
            {
                await (UsersDTO.UpdateUserEmail(id, username));
                return Ok("operation completed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("UpdateUserCoin")]
        public async Task<IActionResult> UpdateUserCoin(int id , int coin)
        {
            try
            {
                await UsersDTO.UpdateUserCoin(id, coin);
                return Ok("Payment successfull");
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpPost("DeleteUserCart")]

        public async Task<IActionResult> DeleteUserCart(int id)
        {
            try
            {
                await UsersDTO.DeleteCart(id);
                return Ok("shii got killed");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetUserCart")]
        public async Task<List<Product>> GetUserCart(int id)
        {
            try
            {
                return await UsersDTO.GetCart(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}