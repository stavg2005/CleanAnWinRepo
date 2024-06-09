using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Admin : ControllerBase
    {
        private readonly AdminDTO u =new AdminDTO();
        [HttpGet("Login")]
        public async Task<Users> Login(string Email, string Password)
        {
            try
            {
                Users us = await u.Login(Email, Password);
                if(us != null && us.IsAdmin)
                { 
                    return us;
                }
                else
                {
                    return new Users();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null ;
            }
        }

        [HttpGet("GetAllAdmins")]
        public async Task<List<Users>> GetAllAdmins()
        {
            try
            {
                return await u.GetAllAdmins();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new List<Users>();
            }
        }

        [HttpPut("MakeUserAdmin")]
        public async Task<IActionResult> MakeUserAdmin(int userid)
        {
            try
            {
                return Ok( await u.MakeUserAdmin(userid));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        
    }
}
