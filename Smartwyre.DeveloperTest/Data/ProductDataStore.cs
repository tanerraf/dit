using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore : IProductDataStore
{
    private readonly HashSet<Product> _products;

    public ProductDataStore()
    {
        _products = GetProducts();

        HashSet<Product> GetProducts()
        {
            var json = System.IO.File.ReadAllText(@"products.json");
            return new HashSet<Product>(System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Product>>(json));
        }
    }

    public Product? GetProduct(string productIdentifier)
    {
        return _products.FirstOrDefault(h => h.Identifier == productIdentifier);
    }
}
