using ArchiLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArchiAPI.Models
{
    public class Pizza : ModelBase
    {
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Topping { get; set; }
}
}
