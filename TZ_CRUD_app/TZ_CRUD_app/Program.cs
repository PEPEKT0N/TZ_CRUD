using TZ_CRUD_app.Model;

var builder = WebApplication.CreateBuilder(args);

// добавление сервисов приложения 
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<SpaceObject>();
builder.Services.AddTransient<Category>();

var app = builder.Build();

// конфигурация приложения
app.MapControllers();

app.Run();
