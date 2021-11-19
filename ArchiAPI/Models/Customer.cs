using ArchiLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
