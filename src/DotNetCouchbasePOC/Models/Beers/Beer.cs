namespace DotNetCouchbasePOC.Models
{
    public class Beer
    {   
        public string name { get; set; }
        public double abv { get; set; }
        public int ibu { get; set; }
        public int srm { get; set; }
        public int upc { get; set; }
        public string type { get; set; }
        public string brewery_id { get; set; }
        public string updated { get; set; }
        public string description { get; set; }
        public string style { get; set; }
        public string category { get; set; }
    }
}