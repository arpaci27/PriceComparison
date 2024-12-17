using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

public class PriceFetcher
{
    public static IWebDriver _driver;
    public static WebDriverWait _wait;

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
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(0))
        {
            PollingInterval = TimeSpan.FromMilliseconds(0)
        };
    }

    public static List<(string name, string price, string image, string link, string shopImage)> FetchTopProducts(string category, string productName)
    {
        string baseUrl = "https://www.cimri.com/{0}?q={1}";
        string searchUrl = string.Format(baseUrl, category, productName);

        try
        {
            Console.WriteLine(searchUrl);
            _driver.Navigate().GoToUrl(searchUrl);

            // İlk 10 ürünü listelemek için XPath seçiciler
            var products = new List<(string name, string price, string image, string link, string shopImage)>();

            for (int i = 1; i <= 10; i++)
            {
                try
                {
                    string nameXPath = $"/html/body/div[1]/main/div[1]/div[4]/div[2]/div[2]/div/div[1]/div/article[{i}]/a/div[2]/h3";
                    string imageXPath = $"/html/body/div[1]/main/div[1]/div[4]/div[2]/div[2]/div/div[1]/div/article[{i}]/a/div[1]/img";
                    string priceXPath = $"/html/body/div[1]/main/div[1]/div[4]/div[2]/div[2]/div/div[1]/div/article[{i}]/div/div/div[1]/div/p";
                    string linkXPath = $"/html/body/div[1]/main/div[1]/div[4]/div[2]/div[2]/div/div[1]/div/article[{i}]/a";
                    string shopImageXPath = $"/html/body/div[1]/main/div[1]/div[4]/div[2]/div[2]/div/div[1]/div/article[{i}]/div/div/div[1]/div/div/img";

                    string name = _wait.Until(drv => drv.FindElement(By.XPath(nameXPath))).Text;
                    string image = _wait.Until(drv => drv.FindElement(By.XPath(imageXPath))).GetAttribute("src");
                    string price = _wait.Until(drv => drv.FindElement(By.XPath(priceXPath))).Text;
                    string link = _wait.Until(drv => drv.FindElement(By.XPath(linkXPath))).GetAttribute("href");
                    string shopImage = _wait.Until(drv => drv.FindElement(By.XPath(shopImageXPath))).GetAttribute("src");

                    products.Add((name, price, image, link, shopImage));
                }
                catch (NoSuchElementException ex)
                {
                    Console.WriteLine($"Element not found: {ex.Message}");
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    break;
                }
            }

            return products;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Page load error: {ex.Message}");
            return new List<(string name, string price, string image, string link, string shopImage)>();
        }
    }
    public static List<(string name, string price, string shopImage, string productImage)> FetchProductDetailsFromLink(string productLink)
    {
        try
        {
            _driver.Navigate().GoToUrl(productLink);
            _wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

            var productDetails = new List<(string name, string price, string shopImage, string productImage)>();

            for (int i = 1; ; i++) // Belirtilen XPath'teki tüm ürünleri bulana kadar devam eder
            {
                try
                {
                    string shopImageXPath = $"/html/body/div[2]/main/div[1]/section[2]/div[3]/div[{i}]/div[2]/div/div[1]/div[1]/img";
                    string nameXPath = $"/html/body/div[2]/main/div[1]/section[2]/div[3]/div[{i}]/div[2]/div/div[1]/div[2]/div";
                    string priceXPath = $"/html/body/div[2]/main/div[1]/section[2]/div[3]/div[{i}]/div[2]/div/div[2]/div[1]/div";
                    string productImageXPath = $"/html/body/div[2]/main/div[1]/section[1]/div/div[1]/div[1]/div[1]/div[1]/img";
                    string shopImage = _wait.Until(drv => drv.FindElement(By.XPath(shopImageXPath))).GetAttribute("src");
                    string name = _wait.Until(drv => drv.FindElement(By.XPath(nameXPath))).Text;
                    string price = _wait.Until(drv => drv.FindElement(By.XPath(priceXPath))).Text;
                    string productImage = _wait.Until(drv => drv.FindElement(By.XPath(productImageXPath))).GetAttribute("src");
                    productDetails.Add((name, price, shopImage, productImage));
                }
                catch (NoSuchElementException)
                {
                    break; // Eleman bulunamazsa döngüyü kır
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    break;
                }
            }

            return productDetails;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Sayfa yükleme hatası: {ex.Message}");
            return new List<(string name, string price, string shopImage, string productImage)>();
        }
    }
    public static string GetFinalProductLink(int j)
    {
        string xpath = $"/html/body/div[2]/main/div[1]/section[2]/div[3]/div[{j+1}]/div[2]/button";
                   
        // Find and click the button using XPath
        IWebElement element = _driver.FindElement(By.XPath(xpath));
        element.Click();

        // Wait for the page to load after clicking
        _wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

        // Switch to the newly opened tab or window
        string originalWindow = _driver.CurrentWindowHandle;
        string newWindow = _driver.WindowHandles.First(handle => handle != originalWindow);
        _driver.SwitchTo().Window(newWindow);

        // Wait for the final redirection to complete
        string previousUrl = string.Empty;
        string currentUrl = _driver.Url;

        // Keep checking until the URL stops changing
        for (int i = 0; i < 10; i++) // Retry up to 10 times
        {
            Thread.Sleep(1000); // Wait for 1 second
            currentUrl = _driver.Url;

            if (currentUrl == previousUrl)
            {
                break; // URL has stabilized
            }

            previousUrl = currentUrl;
        }

        // Capture the final URL
        string finalUrl = currentUrl;

        // Optionally, close the new tab and return to the original window
        _driver.Close();
        _driver.SwitchTo().Window(originalWindow);
        _driver.Navigate().Refresh();
        // Wait for the original page to reload completely
        _wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

        return finalUrl;
    }



    public static void CloseDriver()
    {
        if (_driver != null)
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}