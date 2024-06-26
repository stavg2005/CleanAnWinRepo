﻿using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services;

namespace ApiServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly UsersDTO u = new UsersDTO();
        private readonly OrderDTO o = new OrderDTO();
        [HttpPost("AddOrderToUser")]
        public async Task<IActionResult> AddNewOrder([FromBody] OrderRequestModel orderRequest)
        {
            try
            {
                await o.AddNewOrder(orderRequest.UserID,orderRequest.Products, DateTime.Now);
                return Ok("Added Order Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet("GetAllOrdersForUser")]
        public async Task<List<Order>> GetAllOrdersForUser(int userid)
        {
            try
            {
                return (await o.GetOrdersByUserId(userid));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return new List<Order>(0);
            }
        }

        [HttpGet("GetAllOrders")]
        public async Task<List<Order>> GetAllOrders()
        {
            try
            {
                return await o.SelectAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return new List<Order>(0);
            }
        }
    }
}
