using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace ApiServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        [HttpGet($"GetLocation")]
        public async Task<IActionResult> GetAllLocation (int id)
        {
            return Ok(TrashCanDTO.GetLatLngFromPK(id));
        }
    }
}
