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
                switch (search)
                {
                    case "firstname":
                        query = query.Where(x => x.Firstname.Contains(search));
                        break;
                    case "lastname":
                        query = query.Where(x => x.Lastname.Contains(search));
                        break;
                    case "phone":
                        query = query.Where(x => x.Phone.Contains(search));
                        break;
                    case "email":
                        query = query.Where(x => x.Email.Contains(search));
                        break;
                    default:
                        break;
                }
            }

            return await query.ToListAsync();
        }
    }
}