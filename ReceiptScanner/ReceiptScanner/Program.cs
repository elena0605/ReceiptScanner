using ReceiptScanner;
using System.Text.Json;

async Task ExecuteReceiptScanner()
{
    try
    {
        using (HttpClient httpClient = new HttpClient())
        {
            HttpResponseMessage response = await httpClient.GetAsync("https://interview-task-api.mca.dev/qr-scanner-codes/alpha-qr-gFpwhsQ8fkY1");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<Product>? products = JsonSerializer.Deserialize<List<Product>>(json, options);
               
                if (products != null)
                {
                    List<Product> domesticProducts = products.Where(p => p.Domestic).OrderBy(p => p.Name).ToList();
                    List<Product> importedProducts = products.Where(p => !p.Domestic).OrderBy(p => p.Name).ToList();

                    Console.WriteLine("DOMESTIC PRODUCTS");
                    PrintProducts(domesticProducts);

                    Console.WriteLine("IMPORTED PRODUCTS");
                    PrintProducts(importedProducts);
                }
                else
                {
                    Console.WriteLine("Failed to deserialize the JSON response into List of Products.");
                }
            }
            else
            {
                Console.WriteLine($"HTTP request failed with status code: {response.StatusCode}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

void PrintProducts(List<Product> products)
{
    decimal totalCost = 0;
    int totalCount = 0;

    foreach (Product product in products)
    {
        string truncatedDescription = product.Description.Length > 10
            ? product.Description.Substring(0, 10) + "..."
            : product.Description;

        string weight = product.Weight.HasValue ? $"{product.Weight}g" : "N/A";

        Console.WriteLine($"Name: {product.Name}");
        Console.WriteLine($"Description: {truncatedDescription}");
        Console.WriteLine($"Weight: {weight}");
        Console.WriteLine($"Price: ${product.Price:F2}");

        totalCost += product.Price;
        totalCount++;
        Console.WriteLine();
    }

    string groupKey = products.FirstOrDefault()?.Domestic == true ? "domestic" : "imported";
    Console.WriteLine($"Total cost of {groupKey} products: ${totalCost:F2}");
    Console.WriteLine($"Total number of purchased {groupKey} products: {totalCount}");
    Console.WriteLine();
}

await ExecuteReceiptScanner();


