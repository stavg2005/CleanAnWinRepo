using Microsoft.AspNetCore.Mvc;
using DataLayer;
using Model;
namespace AdminAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAPI : ControllerBase
    {
        [HttpGet("Login")]
        public async Task<Admin> Login(string Email,string Password)
        {
            try
            {
                Admin a = await AdminDTO.Login(Email, Password);
                return a;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
