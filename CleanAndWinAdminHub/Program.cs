using CleanAndWinAdminHub;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<Admin_Service>();
builder.Services.AddScoped<Task_Service>();
builder.Services.AddScoped<TablesService>();



var app = builder.Build();


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
