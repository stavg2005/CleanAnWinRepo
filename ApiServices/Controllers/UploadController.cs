using DataLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace ApiServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly string _connectionString = "server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
        public UploadController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        [HttpGet($"GetImageUri")]
        public IActionResult GetImageUri(int id)
        {
            return Ok(UsersDTO.GetProfilePhoto(id));
        }


        [HttpPost("UploadPhoto")]
        public async Task<IActionResult> UploadPhoto(string imagedata,int id)
        {
            try
            {
                // Convert the base64 string to a byte array
                byte[] imageBytes = Convert.FromBase64String(imagedata);
                ImageModel model = new ImageModel();
                model.ImageData = imageBytes;
                // Your code to update the UserPhoto attribute in the database based on the provided imageBytes and id
                await ImageService.UploadImage(model, id);

                return Ok("Image uploaded successfully!");
            }
            catch (Exception ex)
            {
                // Log and handle exceptions
                Console.WriteLine($"Error uploading image: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("uploadImage/{userId}")]
        public async Task<IActionResult> UploadImage(int userId, IFormFile imageData)
        {
            try
            {
                

                // Convert IFormFile to byte array
                using (MemoryStream ms = new MemoryStream())
                {
                    imageData.CopyTo(ms);
                    byte[] imageBytes = ms.ToArray();

                    ImageModel m = new ImageModel();
                    m.ImageData = imageBytes;
                    // Call the ImageService to update the image in the database
                    await ImageService.UploadImage(m,userId);

                    return Ok("Image uploaded successfully");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("UploadeProductImage/{ProductID}")]
        public async Task<IActionResult> UploadProductImage(int ProductId, IFormFile imageData)
        {
            try
            {


                // Convert IFormFile to byte array
                using (MemoryStream ms = new MemoryStream())
                {
                    imageData.CopyTo(ms);
                    byte[] imageBytes = ms.ToArray();

                    ImageModel m = new ImageModel();
                    m.ImageData = imageBytes;
                    // Call the ImageService to update the image in the database
                    await ImageService.UploadProductImage(m, ProductId);

                    return Ok("Image uploaded successfully");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }

}
