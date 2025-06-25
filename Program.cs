using Microsoft.EntityFrameworkCore;
using ShoppingApi.Data;
using ShoppingApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// שלב 1: הגדרת מחרוזת החיבור ל-SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

// שלב 2: רישום השירות שלך ל־DI
builder.Services.AddScoped<CategoryService>();

var app = builder.Build();
app.UseCors("AllowAll");

// שלב 3: קריאה לשירות שלך כדי להוסיף את הנתונים לדוגמה
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<CategoryService>();
    //await service.AddSampleDataAsync();
}

app.MapGet("/", () => "OK");

// שלב 4: דוגמה ל-API שמחזיר את הקטגוריות עם המוצרים
app.MapGet("/categories", async (CategoryService service) =>
    await service.GetCategoriesDtoAsync());

app.Run();
