using System;

namespace ArchiLibrary.Models
{
    public abstract class ModelBase
    {
        public int ID { get; set; }
        public bool Active { get; set; }
        public DateTime createdat { get; set; }
    }
}
