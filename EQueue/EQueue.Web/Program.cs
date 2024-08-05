using Blazored.LocalStorage;
using EQueue.Web;
using EQueue.Web.Security;
using EQueue.Web.Services.Implementations;
using EQueue.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddScoped<IHttpService, HttpService>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider
    => provider.GetRequiredService<AuthStateProvider>());

await builder.Build().RunAsync();
