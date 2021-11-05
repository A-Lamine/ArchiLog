using Microsoft.AspNetCore.Mvc;
using ArchiAPI.Data;
using ArchiAPI.Models;
using ArchiLibrary.Controllers;

namespace ArchiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasController : BaseController<ArchiDbContext, Pizza>
    {
        public PizzasController(ArchiDbContext c) : base(c)
        {
        }

        // GET: api/order
        [HttpGet("order")]
        public IActionResult GetRange()
        {
            // Request.Headers.
            Response.Headers.Add("Content-Range","0-7");
            Response.Headers.Add("Accept-Range","7");
            Response.Headers.Add("rel","first");

            return Ok();
        }
}
}
