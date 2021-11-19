using Microsoft.AspNetCore.Mvc;
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
    }
}