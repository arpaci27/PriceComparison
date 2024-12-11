namespace PriceComparison.Models
{
    public static class XPaths
    {
        public static Dictionary<string, string> GetXPaths(string siteName, string type)
        {
            var xpaths = new Dictionary<string, string>
        {
            { "trendyol_name", "/html/body/div[1]/div[6]/div/div/div/div/div/div[2]/div[4]/div[1]/div/div[1]/div[1]/div/a[1]/div[1]/div/h3/span[2]" },
            { "trendyol_price", "/html/body/div[1]/div[6]/div/div/div/div/div/div[2]/div[4]/div[1]/div/div[1]/div[1]/div/div/div/div" },
            { "trendyol_image","/html/body/div[1]/div[6]/div/div/div/div/div/div[2]/div[4]/div[1]/div/div[1]/div[1]/a/div/div[1]/div[1]/img" },
            { "hepsiburada_name", "/html/body/div[1]/div/div/main/div/div/div/div/div[2]/div[5]/div/div[2]/div/div/div/div/div/div/ul/li[1]/div/a/div[2]/div[2]/h3" },

            { "hepsiburada_price", "/html/body/div[1]/div/div/main/div/div/div/div/div[2]/div[5]/div/div[2]/div/div/div/div/div/div/ul/li[1]/div/a/div[2]/div[4]" },
            { "hepsiburada_price2", "/html/body/div[1]/div/div/main/div/div/div/div/div[2]/div[5]/div/div[2]/div/div/div/div/div/div/ul/li[1]/div/a/div[2]/div[5]/text()" },
            { "hepsiburada_image", "/html/body/div[1]/div/div/main/div/div/div/div/div[2]/div[5]/div/div[2]/div/div/div/div/div/div/ul/li[1]/div/a/div[1]/div[1]/div[1]/div/picture/img" },
            {"vatan_name","//*[@id=\"productsLoad\"]/div[1]/a/div[2]/div[3]/h3" },
            {"vatan_price", "//*[@id=\"productsLoad\"]/div[1]/a/div[2]/div[4]/div[1]/span[1]" },
            {"vatan_image", "//*[@id=\"productsLoad\"]/div[1]/a/div[1]/div[1]/img" },
            {"mediamarkt_name", "/html/body/div[1]/div[3]/main/div[1]/div/div/div/div[4]/div[1]/div/div/div/a/div/p" },
            {"mediamarkt_price", "/html/body/div[1]/div[3]/main/div[1]/div/div/div/div[4]/div[1]/div/div/div/div[4]/div/div/div[2]/span[1]" },
            {"mediamarkt_image","/html/body/div[1]/div[3]/main/div[1]/div/div/div/div[4]/div[1]/div/div/div/div[2]/a/div/picture/img" }
        };

            return xpaths;
        }
    }
}