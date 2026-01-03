
using Microsoft.EntityFrameworkCore;
using PolyglotShop.Infrastructure.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Tilføj services til container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Hold JSON clean
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Registrer SQL Context (fra U36/U45)
var sqlConnectionString = builder.Configuration.GetConnectionString("SqlDb");
builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseSqlServer(sqlConnectionString));

// 2. Registrer Mongo Context (U40/U46)
builder.Services.AddSingleton<MongoContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();