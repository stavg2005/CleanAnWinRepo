﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public static  class EmailService
    {
        const string secretKey = "d9193d0bb43c542d132d6ad0bfd9321e";
        const string APIKey = "330c93f7f9e505b53a33e40b184301a4";
        const string apiUrl = "https://api.mailjet.com/v3.1/send";
        
        public static async Task<bool> SendEmailAsync(string recipientEmail, string subject, string body)
        {
            using (HttpClient client = new HttpClient())
            {
                // Set the API credentials
                var byteArray = Encoding.ASCII.GetBytes($"{APIKey}:{secretKey}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                // Create the email content
                var messageObject = new
                {
                    Messages = new[]
            {
                new
                {
                    From = new
                    {
                        Email = "cleanandwin05@gmail.com",
                        Name = "CleanAndWin"
                    },
                    To = new[]
                    {
                        new { Email = recipientEmail }
                    },
                    Subject = subject,
                    TextPart = body
                }
            }
                };// Serialize the object to JSON
                string emailContent = JsonSerializer.Serialize(messageObject, new JsonSerializerOptions
                {
                    WriteIndented = true // Optional: Makes the JSON string more readable
                });
                // Convert the email content to a StringContent object
                var content = new StringContent(emailContent, Encoding.UTF8, "application/json");

                // Make the API request
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                // Check the response
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Email sent successfully!");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to send email. Status code: {response.StatusCode}");
                    return false;
                }
            }




        }
    }
}
