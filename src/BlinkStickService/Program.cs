using HidLibrary;
using Microsoft.Extensions.Hosting.WindowsServices;

var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService()
        ? AppContext.BaseDirectory
        : default
};

var builder = WebApplication.CreateBuilder(options);

builder.Host.UseWindowsService();

var app = builder.Build();


const int VendorID = 0X20A0; // 8352
const int ProductID = 0x41E5; // 16969

using var stick = HidDevices.Enumerate(VendorID, ProductID).FirstOrDefault();

app.MapGet("/{r}/{g}/{b}", (byte r, byte g, byte b) =>
{
    if (stick == null) return Results.NotFound();

    stick.WriteFeatureData(new byte[] { 1, r, g, b });

    return Results.Ok();
});

app.Run();

stick?.WriteFeatureData(new byte[] { 1, 0, 0, 0 });