using Microsoft.AspNetCore.Mvc;
using PriceComparison.Controllers;
using PriceComparison.Models;

public class PriceComparisonController : Controller
{
   Context _db = new Context();

    public IActionResult Search()
    {
        return View();
    }
    public IActionResult ComparePrices(string searchTerm)
    {
        var stores = _db.Stores.ToList();
        var results = new List<(string site, string name, string price)>();

        foreach (var store in stores)
        {
            var product = PriceFetcher.GetProductDetails(store.BaseSearchUrl, searchTerm, store.Name);
            results.Add((store.Name, product.name, product.price));
        }

        // Parse prices robustly and find the lowest price
        var lowestPrice = results
            .Where(r => !string.IsNullOrEmpty(r.price)) // Only valid prices
            .OrderBy(r => ParsePrice(r.price)) // Convert prices to decimal and sort
            .FirstOrDefault();

        ViewBag.LowestPrice = lowestPrice;

        return View(results);
    }

    private decimal ParsePrice(string price)
    {
        if (string.IsNullOrWhiteSpace(price))
        {
            return decimal.MaxValue; // Return a high value for invalid prices
        }

        try
        {
            // Remove currency symbol and standardize decimal format
            var cleanedPrice = price
                .Replace("₺", "")
                .Replace("TL", "")
                .Trim();

            // Handle Turkish decimal format
            cleanedPrice = cleanedPrice.Replace(".", "").Replace(",", ".");

            return decimal.Parse(cleanedPrice, System.Globalization.CultureInfo.InvariantCulture);
        }
        catch
        {
            // Log exception and return a high value for invalid prices
            return decimal.MaxValue;
        }
    }

}
