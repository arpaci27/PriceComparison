using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PriceComparison.Models;

namespace PriceComparison.Controllers
{
    public class PriceFetcher : Controller
    {
        public static (string name, string price, string image) GetProductDetails(string baseUrl, string searchTerm, string siteName)
        {
            string searchUrl = string.Format(baseUrl, searchTerm);

            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            IWebDriver driver = new ChromeDriver(options);

            try
            {
                driver.Navigate().GoToUrl(searchUrl);
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); // Explicit wait kullanıyoruz

                var xpaths = XPaths.GetXPaths(siteName, null);

                var productName = wait.Until(drv => drv.FindElement(By.XPath(xpaths[$"{siteName}_name"]))).Text;
                var productPrice = wait.Until(drv => drv.FindElement(By.XPath(xpaths[$"{siteName}_price"]))).Text;
                var productImage = wait.Until(drv => drv.FindElement(By.XPath(xpaths[$"{siteName}_image"]))).GetAttribute("src");

                return (productName, productPrice, productImage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return ("", "", "");
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}
