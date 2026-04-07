using Microsoft.EntityFrameworkCore;
using RechargeFunctions.Application.Services;
using RechargeFunctions.Infraestructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseUrls("http://localhost:5144", "http://0.0.0.0:5144");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure();
        }));

builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<RecargaService>();
builder.Services.AddScoped<TarjetaService>();

var app = builder.Build();

//if (app.Environment.IsDevelopment())

    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();




// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}");

app.MapGet("/ping", () => Results.Ok("pong"));

app.Run();