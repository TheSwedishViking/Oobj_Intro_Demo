using Microsoft.EntityFrameworkCore;
using Oobjs.Lesson1.Core.Data;
using Oobjs.Lesson1.Core.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("OobjsDatabase");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string 'OobjsDatabase' is missing.");
}

builder.Services.AddDbContext<OobjsDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<OobjService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();