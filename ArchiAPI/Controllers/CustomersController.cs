using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArchiAPI.Data;
using ArchiAPI.Models;
using ArchiLibrary.Controllers;

namespace ArchiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : BaseController<ArchiDbContext, Customer>
    {
        public CustomersController(ArchiDbContext c) : base(c)
        {
        }

        // GET: api/customer/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Customer>>> SearchModel([FromQuery] string search)
        {
            var query = from m in _context.Set<Customer>() select m;

            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Firstname.Contains(search));
            }

            return await query.ToListAsync();
        }
    }
}