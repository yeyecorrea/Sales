using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sales.Web;
using Sales.Web.Repositories;
using Sales.WEB.Repositories;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Se une el Back con el Front
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7040/") });

// Inyectamos el IRepository
/*
 El AddScope = se usa cuando queremos que se cree una instancia por inyeccion
 El Singleton = se crea una instancia por una inyeccion y lo deja en memoria
 El Trances = se crea una instancia solo una vez
 */
builder.Services.AddScoped<IRepository, Repository>();

await builder.Build().RunAsync();
