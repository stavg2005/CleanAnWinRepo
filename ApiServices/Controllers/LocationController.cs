using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiServices.Controllers
{
    public class LocationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly LocationsDTO l = new LocationsDTO();

        [HttpGet ("GetAllLocation")]
        public async Task<IActionResult> GetAllLocations()
        {
            try
            {
                var locations = await l.SelectAll();
                return Ok(locations);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetAllLocations: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet ("GetLocationFromPK")]
        public async Task<IActionResult> GetLocationFromPK(int id)
        {
            try
            {
                var locations = await l.GetByPK(id);
                return  Ok(locations);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetAllLocations: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost ("UpdateLocation")]
        public async Task<IActionResult> UpdateLocation([FromBody] Locations lo)
        {
            try
            {
                await l.Update(lo);
                return Ok("operation completed successfully.");
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost ("AddNewLocation")]
        public async Task<IActionResult> AddNewLocation([FromBody] Locations lo)
        {
            try
            {
                await l.Insert(lo); return Ok("operation completed successfully.");
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
