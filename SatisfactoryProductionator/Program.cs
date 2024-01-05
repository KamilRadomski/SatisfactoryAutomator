using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Hosting;
using SatisfactoryProductionator;
using SatisfactoryProductionator.Services.States;
using SatisfactoryProductionator.Services.Data;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<SettingsState>();
builder.Services.AddSingleton<CodexState>();
builder.Services.AddSingleton<MenuState>();
builder.Services.AddSingleton<CodexModalState>();
builder.Services.AddSingleton<AppModalState>();
builder.Services.AddSingleton<PermModalState>();
builder.Services.AddSingleton<PermState>();

await builder.Build().RunAsync();
