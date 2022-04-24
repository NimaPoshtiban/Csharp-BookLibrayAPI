using BookLibray.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// adding ApplicationDbContext to DI Container


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
