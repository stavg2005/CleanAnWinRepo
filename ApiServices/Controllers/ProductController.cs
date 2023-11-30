﻿using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiServices.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAllProducts")]
        public async Task<List<Product>> GetAllProducts()
        {
            return await (ProductDTO.GetAllProducts());
        }

        [HttpGet("GetProuctFromIndex")]
        public async Task<Product> GetProuctFromIndex(int index)
        {
            return await ProductDTO.GetProuctFromIndex(index);
        }
    }
}