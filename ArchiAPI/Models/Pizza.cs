using ArchiLibrary.Models;

namespace ArchiAPI.Models
{
    public class Pizza : ModelBase
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Topping { get; set; }
}
}
