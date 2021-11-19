using Microsoft.AspNetCore.Mvc;
using ArchiAPI.Data;
using ArchiAPI.Models;
using ArchiLibrary.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ArchiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasController : BaseController<ArchiDbContext, Pizza>
    {
        public PizzasController(ArchiDbContext c) : base(c)
        {
        }

        // GET: api/[Model]/Filtre
        [HttpGet("Filtre")]
        public async Task<ActionResult<IEnumerable<Pizza>>> FilterPizza([FromQuery] string attr, [FromQuery] string valeur)
        {
            if (!String.IsNullOrEmpty(attr) && !String.IsNullOrEmpty(valeur))
            { 
                if (attr == "Name")
                {
                    return await _context.Set<Pizza>().Where(x => x.Active == true && x.Name == valeur).ToListAsync();
                }
                else if (attr == "Price")
                {
                    decimal valeurD = Decimal.Parse(valeur);
                    var source = _context.Set<Pizza>().Where(x => x.Active == true && x.Price == valeurD);
                    return await source.ToListAsync();
                }
                else if (attr == "Topping")
                {
                    return await _context.Set<Pizza>().Where(x => x.Active == true && x.Topping == valeur).ToListAsync();
                }
                return null;
            }
            else
            {
                return await _context.Set<Pizza>().Where(x => x.Active == true).ToListAsync();
            }
        }
    }
}
