﻿using DataLayer;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

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

        [HttpGet("GetAllTrashCanLocations")]

        public async Task<IActionResult> GetAllTrashCanLocation()
        {


            try
            {
                List<Tuple<string, string>> l = await TrashCanDTO.GetAlltrashCanLocations();
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
    }
}
