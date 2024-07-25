using ApiCore;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog();

Log.Logger = new LoggerConfiguration().
             MinimumLevel.Debug().
             WriteTo.File(@"Mylog/apilog").
             CreateLogger();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    var connectionString = builder.Configuration.GetConnectionString("DbConnection");

    opt.UseSqlServer(connectionString);

}
);
builder.Services.AddControllers(
    option => option.ReturnHttpNotAcceptable = true).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
