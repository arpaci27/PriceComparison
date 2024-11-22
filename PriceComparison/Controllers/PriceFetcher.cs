using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PriceComparison.Models;

namespace PriceComparison.Controllers
{
    public class PriceFetcher : Controller
    {
        public static (string name, string price, string image) GetProductDetails(string baseUrl, string searchTerm, string siteName)
        {
            string searchUrl = string.Format(baseUrl, searchTerm);
            IWebDriver driver = new ChromeDriver();

            try
            {
                driver.Navigate().GoToUrl(searchUrl);
                var xpaths = XPaths.GetXPaths(siteName, null);

                // Ürün adı, fiyat ve resim URL'si bilgisi alınıyor
                var productName = driver.FindElement(By.XPath(xpaths[$"{siteName}_name"])).Text;
                var productPrice = driver.FindElement(By.XPath(xpaths[$"{siteName}_price"])).Text;
                var productImage = driver.FindElement(By.XPath(xpaths[$"{siteName}_image"])).GetAttribute("src");

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
