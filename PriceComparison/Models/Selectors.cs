namespace PriceComparison.Models
{
    public static class Selectors
    {
        public static Dictionary<string, string> GetSelectors(string siteName, string type)
        {
            var selectors = new Dictionary<string, string>
            {
                { "trendyol_name", ".prdct-desc-cntnr:first-of-type" },
                { "trendyol_price", ".prc-box-dscntd:first-of-type" },
                { "trendyol_image", ".p-card-img:first-of-type" },
                { "hepsiburada_name", "h3[class*='ProductCard']:first-of-type" },
                { "hepsiburada_price", "span[class*='price']:first-of-type" },
                { "hepsiburada_image", "div[data-test-id=\"product-card\"]:first-of-type" },
                {"vatan_name", "#productsLoad > div:nth-child(1) > a > div.product-list__content > div.product-list__product-name > h3" }
            };

            return selectors;
        }
    }
}
