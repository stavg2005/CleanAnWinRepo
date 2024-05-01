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

        [HttpGet("GetAllAdmins")]
        public async Task<List<Model.Admin>> GetAllAdmins()
        {
            try
            {
                return await AdminDTO.GetAllAdmins();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new List<Model.Admin>();
            }
        }

        [HttpPost("AddNewTask")]
        public async Task AddTask([FromBody] Project_Task p)
        {
            try
            {
                await Project_TaskDTO.AddNewTask(p);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        [HttpGet("GetAllCurrentTasks")]
        public async Task<List<Project_Task>> GetAllCurrentTasks()
        {
            try
            {
                return await Project_TaskDTO.GetAllCurrentTasks();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new List<Project_Task>();
            }
        }
    }
}
