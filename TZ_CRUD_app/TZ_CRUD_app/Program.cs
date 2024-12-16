using TZ_CRUD_app.Model;
using TZ_CRUD_app.Service;

var builder = WebApplication.CreateBuilder(args);

// добавление сервисов приложения 
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<SpaceObjectService>();
builder.Services.AddTransient<CategoryService>();
builder.Services.AddTransient<PagingService>();

var app = builder.Build();

// конфигурация приложения
app.MapControllers();

app.Run();
