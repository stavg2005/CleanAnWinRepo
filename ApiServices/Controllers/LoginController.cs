using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Services;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace ApiServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UsersDTO u = new UsersDTO();
        [HttpGet($"Login")]
        public async Task<Users> GetAllProducts(string email, string password)
        {
            
            return await (u.Login(email, password));
        }

        [HttpGet("GetAllUsers")]
        public async Task<List<Users>> GetAllUsers()
        {
            try
            {
                return await u.SelectAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<Users>();
            }
        }



        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UsersDTO user)
        {
            try
            {
                return Ok(await u.Register(user));
            }
            catch (Exception ex)
            {
                // Log the exception
                Debug.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [HttpGet("GetUser")]
        public async Task<Users> GetUser(int id)
        {
            Users us = await u.GetByPK(id);
            return (us);
        }

        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(int id, string password)
        {
            try
            {
                await (u.UpdatePassword( id,password));
                return Ok("operation completed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UsersDTO us)
        {
            try
            {
                await (u.Update(us));
                return Ok("operation completed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("DeleteUserCart")]

        public async Task<IActionResult> DeleteUserCart(int id)
        {
            try
            {
                await u.DeleteCart(id);
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
                return await u.GetUserCart(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetUserIDfromEmail")]
        public async Task<int> GetUserIDfromEmail(string email)
        {
            try
            {
                return await u.GetUserIdFromEmail(email);
            }
            catch(Exception ex)
            {
                return -1;
            }
        }


        [HttpGet("GetTopUsers")]
        public async Task<List<LeaderboardUser>> GetTopUsers()
        {
            try
            {
                return await u.GetTopUsersOfAllTime();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<LeaderboardUser>(0);
            }

        }

        [HttpGet("GetTopUsersToday")]

        public async Task<List<LeaderboardUser>> GetTopUsersToday()
        {
            try
            {
                return await u.GetTopUsersToday();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<LeaderboardUser>(0);
            }

        }

        

        [HttpGet("GetTopUsersThisWeek")]
        public async Task<List<LeaderboardUser>> GetTopUsersThisWeek()
        {
            try
            {
                return await u.GetTopUsersThisWeek();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<LeaderboardUser>(0);
            }
        }

        [HttpGet("GetCoinAndXpValue")]
        public async Task<Tuple<int,int>> GetCoinValue()
        {
            try
            {
                return await u.GetCoinandxpValue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Tuple<int, int>(0, 0);
            }
        }

        [HttpGet("CheakIfUserNameExist")]
        public async Task<bool> CheakIfUserNameExist(string username)
        {
            try
            {
                return await u.CheakIfUserNameExist(username);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}