using Microsoft.AspNetCore.Mvc;
using PriceComparison.Models;
using System.Linq;
using System.Collections.Generic;

public class PriceComparisonController : Controller
{
    Context _db = new Context();

    public IActionResult Search()
    {
        return View();
    }

    public IActionResult UITry()
    {
        return View();
    }

    public IActionResult ComparePrices(string category, string searchTerm)
    {
        // Fetch top products using the updated PriceFetcher method
        var topProducts = PriceFetcher.FetchTopProducts(category, searchTerm);

        // Passing the product list to the view
        return View(topProducts);
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
    public IActionResult ProductDetails(string productLink)
    {
        if (string.IsNullOrEmpty(productLink))
        {
            ViewBag.ErrorMessage = "Ürün bağlantısı eksik.";
            return View("Error");
        }

        var productDetails = PriceFetcher.FetchProductDetailsFromLink(productLink);

        if (productDetails == null || !productDetails.Any())
        {
            ViewBag.ErrorMessage = "Ürün detayları bulunamadı.";
            return View("Error");
        }

        return View(productDetails);
    }

}
