using System;
using ArchiAPI.Data;
using ArchiAPI.Models;

namespace ArchiTest
{
    public class DummyDataDBInitializer  
    {  
        public DummyDataDBInitializer()  
        {  
        }  
  
        public void Seed(ArchiDbContext context)  
        {  
            context.Database.EnsureDeleted();  
            context.Database.EnsureCreated();

            context.Pizzas.AddRange(
                new Pizza() { Name = "Royal", Price = 13, Topping = "Champignon, Formage" },
                new Pizza() { Name = "Chevre Miel", Price = 11, Topping = "Chevre, Miel" }
            );

            context.SaveChanges();  
        }  
    }  
}
