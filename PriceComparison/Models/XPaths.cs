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
            { "hepsiburada_name", "/html/body/div[1]/div/div/main/div/div/div/div/div[2]/div[5]/div/div[2]/div/div/div/div/div/div/ul/li[1]/div/a/div[2]/div[2]/h3" },
            { "hepsiburada_price", "/html/body/div[1]/div/div/main/div/div/div/div/div[2]/div[5]/div/div[2]/div/div/div/div/div/div/ul/li[1]/div/a/div[2]/div[4]" }
        };

            return xpaths;
        }
    }
}
