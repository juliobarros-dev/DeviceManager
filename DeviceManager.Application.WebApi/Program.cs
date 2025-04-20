using Asp.Versioning;
using DeviceManager.Application.WebApi.Extensions;
using DeviceManager.Application.WebApi.Utils;
using DeviceManager.Domain.Services.Implementations;
using DeviceManager.Domain.Services.Interfaces;
using DeviceManager.Infrastructure.Database.Context;
using DeviceManager.Infrastructure.Database.Implementations;
using DeviceManager.Infrastructure.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var basePath = typeof(Program).Assembly.GetDirectoryName();

builder.Configuration
	.SetBasePath(basePath)
	.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
	.AddJsonFile("appsettings.database.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables();

builder.Services.AddDbContext<DeviceManagerDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDeviceManagerDbContext, DeviceManagerDbContext>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();

builder.Services.AddControllers();


builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
	{
		Title = "Device Manager API",
		Version = "v1",
		Description = "API to manage devices",
	});

	options.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
	options.OperationFilter<RemoveVersionParameterFilter>();
});

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
	app.UseHttpsRedirection();
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "Device Manager API v1");
		c.RoutePrefix = "";
	});
}

app.UseHttpsRedirection();

app.MapControllers();

app.UsePathBase("/device-manager");

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<DeviceManagerDbContext>();
	db.Database.Migrate();
}

app.Run();