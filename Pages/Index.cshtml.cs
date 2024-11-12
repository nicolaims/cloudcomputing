using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

public class IndexModel : PageModel
{
    public required List<MenuItem> MenuItems { get; set; }

    public void OnGet()
    {
        var tableClient = new TableClient("DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=kantinestorage;AccountKey=c5eYXG6dgGxG7L4xUtAJ02pz4FoGOsiYisQNTuD3G4rYaS2xQ1LmgX1re/RnUlRBtkrJtRY70jTv+ASthJdyZA==;BlobEndpoint=https://kantinestorage.blob.core.windows.net/;FileEndpoint=https://kantinestorage.file.core.windows.net/;QueueEndpoint=https://kantinestorage.queue.core.windows.net/;TableEndpoint=https://kantinestorage.table.core.windows.net/", "Menu");
        
        // Querying the data from the table
        MenuItems = tableClient.Query<TableEntity>()
            .Select(e => new MenuItem
            {
                RowKey = e.RowKey,
                ColdDish = e.GetString("ColdDish"),
                HotDish = e.GetString("HotDish")
            }).ToList();

        // Define an array for the days of the week in the correct order
        var daysOfWeek = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        // Sort MenuItems based on the day of the week, using RowKey or another property to match the day
        MenuItems = MenuItems.OrderBy(item => Array.IndexOf(daysOfWeek, item.RowKey)).ToList();
    }
}

public class MenuItem
{
    public string? RowKey { get; set; }
    public string? ColdDish { get; set; }
    public string? HotDish { get; set; }
}
