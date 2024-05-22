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
        public async Task<Users> Login(string Email, string Password)
        {
            try
            {
                Users u = await UsersDTO.Login(Email, Password  );
                if(u != null && u.IsAdmin)
                { 
                    return u;
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
                return await AdminDTO.GetAllAdmins();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new List<Users>();
            }
        }

        
    }
}
