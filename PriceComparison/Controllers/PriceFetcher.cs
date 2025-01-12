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
        options.AddArgument("--enable-unsafe-swiftshader");

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
        string baseurl2 = "https://www.hepsiburada.com/{0}";
        string baseurl3 = "https://www.epey.com/{0}?q={1}";
        string baseurl4 = "https://www.amazon.com/{0}/s?k={1}";
        string baseurl5 = "https://www.pttavm.com/{0}?q={1}";
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

            for (int i = 1; ; i++)
            {
                try
                {
                    string baseXPath = $"/html/body/div[2]/main/div[1]/section[2]/div[3]/div[{i}]";

                    bool elementExists = _driver.FindElements(By.XPath(baseXPath)).Count > 0;
                    if (!elementExists)
                    {
                        Console.WriteLine($"XPath'te eleman tükendi. Döngü {i}. elemanda sona eriyor.");
                        break;
                    }

                    string shopImage = _driver.FindElement(By.XPath($"{baseXPath}/div[2]/div/div[1]/div[1]/img"))?.GetAttribute("src") ?? "";
                    string name = _driver.FindElement(By.XPath($"{baseXPath}/div[2]/div/div[1]/div[2]/div"))?.Text ?? "";
                    string price = _driver.FindElement(By.XPath($"{baseXPath}/div[2]/div/div[2]/div[1]/div"))?.Text ?? "";
                    string productImage = _driver.FindElement(By.XPath($"/html/body/div[2]/main/div[1]/section[1]/div/div[1]/div[1]/div[1]/div[1]/img"))?.GetAttribute("src") ?? "";

                    if (string.IsNullOrEmpty(shopImage) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(price) || string.IsNullOrEmpty(productImage))
                    {
                        Console.WriteLine($"{i}. öğe atlandı: Eksik bilgi.");
                        continue;
                    }

                    productDetails.Add((name, price, shopImage, productImage));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu (Öğe {i}): {ex.Message}");
                    continue;
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
        try
        {
            // Eğer j 3 ise bir artırarak 4 yap
            if (j == 3)
            {
                Console.WriteLine($"j = {j}, 4 olarak güncellendi.");
                j = 4;
            }

            // Mevcut ürün sayısını kontrol et
            int productCount = _driver.FindElements(By.XPath("/html/body/div[2]/main/div[1]/section[2]/div[3]/div")).Count;
            Console.WriteLine($"Mevcut ürün sayısı: {productCount}");

            // j değerini mevcut ürün sayısına göre sınırla
            if (j > productCount)
            {
                Console.WriteLine($"j değeri ({j}) mevcut ürün sayısını ({productCount}) aşıyor. Güncelleniyor.");
                j = productCount;
            }

            int index = (j > 3) ? (j + 2) : (j + 1);
            string xpath = $"/html/body/div[2]/main/div[1]/section[2]/div[3]/div[{index}]/div[2]/button";

            // XPath'in mevcut olup olmadığını kontrol et
            var elements = _driver.FindElements(By.XPath(xpath));
            if (elements.Count == 0)
            {
                Console.WriteLine($"Eleman bulunamadı: {xpath}");
                return string.Empty; // Eğer eleman yoksa boş string döner
            }

            // Elemanı bul ve tıkla
            IWebElement element = elements.First();
            element.Click();

            // Sayfa yüklemesini bekle
            _wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

            // Yeni sekmeye geç
            string originalWindow = _driver.CurrentWindowHandle;
            string newWindow = _driver.WindowHandles.First(handle => handle != originalWindow);
            _driver.SwitchTo().Window(newWindow);

            // URL'nin stabilize olmasını bekle
            string previousUrl = string.Empty;
            string currentUrl = _driver.Url;

            for (int i = 0; i < 10; i++) // En fazla 10 kez dene
            {
                Thread.Sleep(1000); // 1 saniye bekle
                currentUrl = _driver.Url;

                if (currentUrl == previousUrl)
                {
                    break; // URL değişmedi, stabilize oldu
                }

                previousUrl = currentUrl;
            }

            // Nihai URL'yi al
            string finalUrl = currentUrl;

            // Yeni sekmeyi kapat ve orijinal sekmeye dön
            _driver.Close();
            _driver.SwitchTo().Window(originalWindow);
            _driver.Navigate().Refresh();

            // Orijinal sayfanın tamamen yüklenmesini bekle
            _wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

            return finalUrl;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Bir hata oluştu: {ex.Message}");
            return string.Empty; // Hata durumunda boş string döner
        }
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