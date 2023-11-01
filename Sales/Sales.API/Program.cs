using Microsoft.EntityFrameworkCore;
using Sales.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyectamos el DataContext
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=DefaultConnection"));
// Inyectamos el seeder
builder.Services.AddTransient<SeedDb>();
var app = builder.Build();
SeedData(app);

// Metodo que crea una inyeccion manual para los datos en caso de uq eno existan ya que estos datos no se ingresaran por repositorry
void SeedData(WebApplication app)
{
    IServiceScopeFactory? scopedFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
    using (IServiceScope? scope = scopedFactory!.CreateScope())
    {
        SeedDb? service = scope.ServiceProvider.GetService<SeedDb>();
        service!.SeedAsync().Wait();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Se habilitan los cors para poder conectarnos desde el front

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
