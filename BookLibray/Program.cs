using BookLibray.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// adding ApplicationDbContext to DI Container
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// serilog config
builder.Host.UseSerilog((context,loggingConf)=>loggingConf.WriteTo.Console().ReadFrom.Configuration(context.Configuration));


// CORS POLICY
builder.Services.AddCors(options =>
{
    options.AddPolicy(
    "AllowAll", opt => opt.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()) ; 
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Applying CORS POLICY
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
