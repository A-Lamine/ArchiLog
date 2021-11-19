using ArchiLibrary.Models;

namespace ArchiAPI.Models
{
    public class Customer : ModelBase
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
    }
}
