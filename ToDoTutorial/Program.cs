using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using ToDoTutorial;
using ToDoTutorial.Core.Logic;
using Microsoft.Extensions.Logging;
using ToDoTutorial.Core.Framework;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<ToDoManager>();
builder.Services.AddScoped<LocalStorageContext>();

builder.Services.AddFluentUIComponents();
builder.Services.AddDataGridEntityFrameworkAdapter();
builder.Services.AddBlazoredLocalStorage();

builder.Logging.SetMinimumLevel(LogLevel.Information);

await builder.Build().RunAsync();
