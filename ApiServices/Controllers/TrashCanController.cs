﻿using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services;
using System.Net.Http;

namespace ApiServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrashCanController : ControllerBase
    {
        private readonly TrashCanDTO t = new TrashCanDTO(); 
        private readonly ReportCleanDTO r = new ReportCleanDTO();
        [HttpPost ("ReportClean")]
        public async Task<IActionResult> ReportClean([FromBody] ReportClean r)
        {
            try
            {
                await (t.ReportClean(r.Userid, r.weight, r.TrashCanId));
                return Ok("ReportClean operation completed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("GetAllReports")]
        public async Task<List<ReportClean>> GetAllReports()
        {
            try
            {
                return await r.SelectAll();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new List<ReportClean>();
            }
        }
        [HttpGet("GetAllTrashCanLocations")]
        public async Task<IActionResult> GetAllTrashCanLocation()
        {


            try
            {
                List<TrashCan> l = await t.SelectAll();
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
        public async Task<IActionResult> InsertTrashCan([FromBody]TrashCan tr)
        {
            try
            {
                await t.Insert(tr); return Ok("Post operation completed successfully.");
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
                await t.Delete(id); return Ok("Delete operation completed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("UpdateTrashCan")]
        public async Task<IActionResult> UpdateTrashCan([FromBody]  TrashCan tr)
        {
            try
            {
                await t.Update(tr); return Ok("Update Operation completed succefuly");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("AddWeight")]
        public async Task<IActionResult> AddWeight([FromBody] WeightEntry weightEntry)
        {
            try
            {
                TrashCanDTO t = new TrashCanDTO();
                await t.AddWeightAsync(weightEntry.TrashCanID, weightEntry.Weight);
                return Ok("Added Weight succefuly");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("CheckForNewEntryAsync")]
        public async Task<float> CheckForNewEntryAsync(int TrashCanID)
        {
            try
            {
                TrashCanDTO t = new TrashCanDTO();
                return await t.CheckForNewEntryAsync(TrashCanID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }
    }
}
