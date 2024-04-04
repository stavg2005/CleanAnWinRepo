using CleanAndWinAdminHub;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<Admin_Service>();
builder.Services.AddSingleton<Task_Service>();
builder.Services.AddSingleton<TablesService>();



var app = builder.Build();


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
