using Microsoft.AspNetCore.Mvc;
using ArchiAPI.Data;
using ArchiAPI.Models;
using ArchiLibrary.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace ArchiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasController : BaseController<ArchiDbContext, Pizza>
    {
        public PizzasController(ArchiDbContext c) : base(c)
        {
        }

        // GET: api/pizza/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Pizza>>> SearchModel([FromQuery] string search)
        {
            var query = from m in _context.Set<Pizza>() select m;

            if (!String.IsNullOrEmpty(search))
            {
                switch(search)
                {
                    case "name":
                        query = query.Where(x => x.Name.Contains(search));
                        break;
                    case "topping":
                        query = query.Where(x => x.Topping.Contains(search));
                        break;
                    default:
                        break;
                }
            }

            return await query.ToListAsync();
        }
    }
}
