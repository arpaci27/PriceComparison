using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using PriceComparison.Models;

public class PriceFetcher : Controller
{
    private static IWebDriver _driver;
    private static WebDriverWait _wait;

    static PriceFetcher()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--disable-extensions");
        options.AddArgument("--disable-notifications");
        options.AddArgument("--disable-popup-blocking");
        options.AddArgument("--log-level=3");
        options.AddArgument("--ignore-certificate-errors");
        options.AddArgument("--blink-settings=imagesEnabled=false");
        options.AddUserProfilePreference("profile.managed_default_content_settings.stylesheets", 2);
        options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        _driver = new ChromeDriver(options);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(1))
        {
            PollingInterval = TimeSpan.FromMilliseconds(0)
        };
    }

    public static (string name, string price, string image) GetProductDetails(string baseUrl, string searchTerm, string siteName)
    {
        try
        {
            string searchUrl = string.Format(baseUrl, searchTerm);
            _driver.Navigate().GoToUrl(searchUrl);

            string productName, productPrice, productImage;

            if (siteName == "hepsiburada" || siteName == "mediamarkt")
            {
                var xpaths = XPaths.GetXPaths(siteName, null);
                productName = _wait.Until(drv => drv.FindElement(By.XPath(xpaths[$"{siteName}_name"]))).Text;
                productPrice = _wait.Until(drv => drv.FindElement(By.XPath(xpaths[$"{siteName}_price"]))).Text;
                productImage = _wait.Until(drv => drv.FindElement(By.XPath(xpaths[$"{siteName}_image"]))).GetAttribute("src");
            }
            else
            {
                var selectors = Selectors.GetSelectors(siteName, null);
                productName = _wait.Until(drv => drv.FindElement(By.CssSelector(selectors[$"{siteName}_name"]))).Text;
                productPrice = _wait.Until(drv => drv.FindElement(By.CssSelector(selectors[$"{siteName}_price"]))).Text;
                productImage = _wait.Until(drv => drv.FindElement(By.CssSelector(selectors[$"{siteName}_image"]))).GetAttribute("src");
            }

            return (productName, productPrice, productImage);
        }
        catch (NoSuchElementException ex)
        {
            Console.WriteLine($"Element bulunamadı: {ex.Message}");
            return ("", "", "");
        }
        catch (WebDriverTimeoutException ex)
        {
            Console.WriteLine($"Timeout: {ex.Message}");
            return ("", "", "");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
            return ("", "", "");
        }
    }

    public static List<(string name, string price, string image)> GetMultipleProductDetails(List<(string baseUrl, string searchTerm, string siteName)> queries)
    {
        var results = new List<(string name, string price, string image)>();

        foreach (var query in queries)
        {
            var result = GetProductDetails(query.baseUrl, query.searchTerm, query.siteName);
            results.Add(result);
        }

        return results;
    }
}
