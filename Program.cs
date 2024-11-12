using Azure.Identity;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Set up TableClient
var storageAccountUrl = new Uri("https://kantinestorage.table.core.windows.net");
var tableName = "Menu";
var credential = new DefaultAzureCredential();
var tableClient = new TableClient(storageAccountUrl, tableName, credential);

// Register TableClient as a singleton
builder.Services.AddSingleton(tableClient);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();