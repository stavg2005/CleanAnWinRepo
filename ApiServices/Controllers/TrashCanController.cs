using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace ApiServices.Controllers
{
    public class TrashCanController : ControllerBase
    {
        [HttpPost ("ReportClean")]
        public async Task<IActionResult> ReportClean(int userid, int weight, int trashcanid)
        {
            try
            {
                await (TrashCanDTO.ReportClean(userid, weight, trashcanid));
                return Ok("ReportClean operation completed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
