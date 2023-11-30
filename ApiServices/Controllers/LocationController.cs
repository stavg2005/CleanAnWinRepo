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
                return Json(locations);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetAllLocations: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
