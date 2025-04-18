using Asp.Versioning;
using DeviceManager.Application.WebApi.Extensions;
using DeviceManager.Domain.Services.Implementations;
using DeviceManager.Domain.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var basePath = typeof(Program).Assembly.GetDirectoryName();

builder.Configuration
	.SetBasePath(basePath)
	.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
	// .AddJsonFile("appsettings.database.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables();

builder.Services.AddScoped<IDeviceService, DeviceService>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddApiVersioning(options =>
{
	options.ReportApiVersions = true;
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.DefaultApiVersion = new ApiVersion(1, 0);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UsePathBase("/device-manager");

app.Run();