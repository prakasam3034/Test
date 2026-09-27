using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Threading.RateLimiting;
using Test.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddRateLimiter(options =>
//{
//    options.AddFixedWindowLimiter("Rate", options =>
//    {
//        options.PermitLimit = 2;
//        options.Window=TimeSpan.FromMinutes(1);
//        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//        options.QueueLimit = 2;
//    });
//    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
//});
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode=StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.CreateChained<HttpContext>(
        PartitionedRateLimiter.Create<HttpContext, string>(HttpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: HttpContext.User.Identity?.Name ?? HttpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 5,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            })),
        PartitionedRateLimiter.Create<HttpContext, string>(HttpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: HttpContext.User.Identity?.Name ?? HttpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 1000,
                QueueLimit = 0,
                Window = TimeSpan.FromHours(1)
            })));

});
builder.Services.AddScoped<IRepository, Repository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers().RequireRateLimiting("Rate");

app.Run();
