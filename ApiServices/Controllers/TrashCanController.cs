﻿using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Net.Http;

namespace ApiServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrashCanController : ControllerBase
    {
        [HttpPost ("ReportClean")]
        public async Task<IActionResult> ReportClean([FromBody] ReportClean r)
        {
            try
            {
                await (TrashCanDTO.ReportClean(r.Userid, r.weight, r.TrashCanId));
                return Ok("ReportClean operation completed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("GetAllTrashCanLocations")]

        public async Task<IActionResult> GetAllTrashCanLocation()
        {


            try
            {
                List<TrashCan> l = await TrashCanDTO.GetAlltrashCanLocations();
                l = l;
                return Ok(l);
            }
            catch (Exception e)
            {
                List<Tuple<string, string>> error = new List<Tuple<string, string>>();
                error.Add(new Tuple<string, string>("error", e.Message));
                return Ok(error);
            }

        }

        [HttpPost("InsertTrashCan")]
        public async Task<IActionResult> InsertTrashCan([FromBody]TrashCan t)
        {
            try
            {
                await TrashCanDTO.InsertTrashCan(t); return Ok("Post operation completed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpPost("DeleteTrashCan")]
        public async Task<IActionResult> DeleteTrashCan([FromBody] int id)
        {
            try
            {
                await TrashCanDTO.DeleteTrashCan(id); return Ok("Delete operation completed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
