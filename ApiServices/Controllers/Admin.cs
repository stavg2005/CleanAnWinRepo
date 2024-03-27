using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Admin : ControllerBase
    {
        [HttpGet("Login")]
        public async Task<Model.Admin> Login(string Email, string Password)
        {
            try
            {
                Model.Admin a = await AdminDTO.Login(Email, Password);
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
