using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Dapper;

namespace DataLayer
{
	public class ImageService
	{
		private readonly MySqlConnection _connection = new MySqlConnection("server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog");


		public static async Task<int> UploadImage(ImageModel image,int id)
		{
            MySqlConnection _connection = new MySqlConnection("server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog");
            if (_connection.State == ConnectionState.Closed)
                await _connection.OpenAsync();

            // Update the user's profile picture in the Users table
            var updateUserSql = "UPDATE Users SET ProfilePicture = @ImageData WHERE UserId = @UserId;";
            await _connection.ExecuteAsync(updateUserSql, new { ImageData = image.ImageData, UserId = id });

            return 1;
        }
        public static async Task<int> UploadProductImage(ImageModel image, int id)
        {
            MySqlConnection _connection = new MySqlConnection("server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog");
            if (_connection.State == ConnectionState.Closed)
                await _connection.OpenAsync();

            // Update the Product's picture in the product table
            var updateProductSql = "UPDATE product SET ProductPicture = @ImageData WHERE ProductID = @ProductID;";
            await _connection.ExecuteAsync(updateProductSql, new { ImageData = image.ImageData, ProductID = id });

            return 1;
        }
    }
	}
