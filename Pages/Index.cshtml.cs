using Azure.Identity;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;

public class IndexModel : PageModel
{
    public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

    private readonly TableClient _tableClient;

    public IndexModel(TableClient tableClient)
    {
        _tableClient = tableClient;
    }

    public async Task OnGetAsync()
    {
        try
        {
            List<TableEntity> results = new List<TableEntity>();
            AsyncPageable<TableEntity> queryResults = _tableClient.QueryAsync<TableEntity>();

            await foreach (TableEntity entity in queryResults)
            {
                // Log each entity retrieved
                Console.WriteLine($"Retrieved entity: {entity.RowKey}");
                results.Add(entity);
            }

            foreach (var entity in results)
            {
                MenuItems.Add(new MenuItem
                {
                    RowKey = entity.GetString("RowKey"),
                    ColdDish = entity.GetString("ColdDish"),
                    HotDish = entity.GetString("HotDish")
                });
            }

            // Log the number of items retrieved
            Console.WriteLine($"Retrieved {MenuItems.Count} menu items.");
            foreach (var item in MenuItems)
            {
                Console.WriteLine($"RowKey: {item.RowKey}, ColdDish: {item.ColdDish}, HotDish: {item.HotDish}");
            }
        }
        catch (Exception ex)
        {
            // Log any exceptions
            Console.WriteLine($"Error retrieving data: {ex.Message}");
        }
    }
}

public class MenuItem
{
    public string? RowKey { get; set; }
    public string? ColdDish { get; set; }
    public string? HotDish { get; set; }
}