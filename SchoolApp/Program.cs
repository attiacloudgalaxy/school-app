using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SchoolApp;
using SchoolApp.Services;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Read API base URL from configuration (supports both local and Docker)
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5178/";

// Configure HttpClient to point to SchoolApi backend
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

// Register SchoolApiService
builder.Services.AddScoped<SchoolApiService>();

await builder.Build().RunAsync();
