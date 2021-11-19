using ArchiAPI.Models;
using ArchiLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArchiAPI.Data
{
    public class ArchiDbContext : DbContext
    {
        public ArchiDbContext(DbContextOptions option):base(option)
        {
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ChangeAddedState();
            ChangeDeletState();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeAddedState();
            ChangeDeletState();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ChangeAddedState()
        {
            var enties = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);
            foreach (var item in enties)
            {
                item.State = EntityState.Added;
                if (item.Entity is ModelBase)
                {   
                    ((ModelBase)item.Entity).Active = true;
                    ((ModelBase)item.Entity).createdat = DateTime.Now;
                }
            }
        }

        private void ChangeDeletState()
        {
            var enties = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);
            foreach (var item in enties)
            {
                item.State = EntityState.Modified;
                if (item.Entity is ModelBase)
                {
                    ((ModelBase)item.Entity).Active = false;
                }
            }

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }

    }
}
