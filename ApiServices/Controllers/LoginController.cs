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

        [HttpGet("Register")]
        public async Task<IActionResult> Register(string username,string password,string email,int location,int id)
        {
            return Ok(UsersDTO.Register(username, password, email, location, id));
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            return Ok(UsersDTO.GetUserByID(id));
        }

        [HttpPost ("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(int id,string password)
        {
            try
            {
                await (UsersDTO.UpdatePassword(id,password));
                return Ok("operation completed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost ("UpdateUserName")]
        public async Task<IActionResult> UpdateUserPassword(int id, string username)
        {
            try
            {
                await (UsersDTO.UpdateUserName(id,username));
                return Ok("operation completed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, ex.Message);
            }
        }

    }
}