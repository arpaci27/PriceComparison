namespace PriceComparison.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BaseSearchUrl { get; set; }
        public string XPathName { get; set; }
        public string XPathImage { get; set; }
        public string XPathPrice { get; set; }
    }
}
