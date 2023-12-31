using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace ApiServices.Controllers
{
    public class LocationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet ("GetAllLocation")]
        public async Task<IActionResult> GetAllLocations()
        {
            try
            {
                var locations = await LocationsDTO.GetAllLocations();
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
                var locations = await LocationsDTO.GetLocationFromPK(id);
                return  Ok(locations);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetAllLocations: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
