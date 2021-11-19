using ArchiLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArchiLibrary.Controllers
{
   public abstract class BaseController<TContext, TModel> : ControllerBase where TContext : DbContext where TModel : ModelBase
    {
        protected readonly TContext _context;

        public BaseController(TContext context)
        {
            _context = context ;
        }

        // GET: api/[Model]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TModel>>> TriTModel([FromQuery] string asc, string desc)
        {
            var source = _context.Set<TModel>().Where(x => x.Active);

            if (!String.IsNullOrEmpty(asc))
            {
                string[] ascall = asc.Split(',');

                var parameter = Expression.Parameter(typeof(TModel), "x");
                Expression property = Expression.Property(parameter, ascall[0]);
                var lambda = Expression.Lambda(property, parameter);


                var orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
                var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TModel), property.Type);
                var result = orderByGeneric.Invoke(null, new object[] { source, lambda });

                foreach (string i in ascall)
                {
                    var parameter1 = Expression.Parameter(typeof(TModel), "x");
                    Expression property1 = Expression.Property(parameter1, i);
                    var lambda1 = Expression.Lambda(property1, parameter1);

                    orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "ThenBy" && x.GetParameters().Length == 2);
                    orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TModel), property1.Type);
                    result = orderByGeneric.Invoke(null, new object[] { result, lambda1 });
                }
                if (!String.IsNullOrEmpty(desc))
                {
                    string[] descall = desc.Split(',');

                    foreach (string a in descall)
                    {
                        var parameter1 = Expression.Parameter(typeof(TModel), "x");
                        Expression property1 = Expression.Property(parameter1, a);
                        var lambda1 = Expression.Lambda(property1, parameter1);

                        orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "ThenByDescending" && x.GetParameters().Length == 2);
                        orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TModel), property1.Type);
                        result = orderByGeneric.Invoke(null, new object[] { result, lambda1 });
                    }
                }
                return await ((IOrderedQueryable<TModel>)result).ToListAsync();
            }



            if (!String.IsNullOrEmpty(desc))
            {
                string[] descall = desc.Split(',');

                var parameter = Expression.Parameter(typeof(TModel), "x");
                Expression property = Expression.Property(parameter, descall[0]);
                var lambda = Expression.Lambda(property, parameter);


                var orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "OrderByDescending" && x.GetParameters().Length == 2);
                var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TModel), property.Type);
                var result = orderByGeneric.Invoke(null, new object[] { source, lambda });

                foreach (string a in descall)
                {
                    var parameter1 = Expression.Parameter(typeof(TModel), "x");
                    Expression property1 = Expression.Property(parameter1, a);
                    var lambda1 = Expression.Lambda(property1, parameter1);

                    orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "ThenByDescending" && x.GetParameters().Length == 2);
                    orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TModel), property1.Type);
                    result = orderByGeneric.Invoke(null, new object[] { result, lambda1 });
                }
                return await ((IOrderedQueryable<TModel>)result).ToListAsync();
            }
            return await source.ToListAsync();
        }

        // GET: api/[Model]/:id
        [HttpGet("{id}")]
        public async Task<ActionResult<TModel>> GetModelById(int id)
        {
            var model = await _context.Set<TModel>().FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return model;
        }

        // PUT: api/[Model]/:id
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModel(int id, TModel TModel)
        {
            if (id != TModel.ID)
            {
                return BadRequest();
            }

            _context.Entry(TModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/[Model]
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TModel>> PostModel(TModel TModel)
        {
            _context.Set<TModel>().Add(TModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModelById", new { id = TModel.ID }, TModel);
        }

        // DELETE: api/[Model]/:id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModel(int id)
        {
            var model = await _context.Set<TModel>().FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            model.Active = false;
            _context.Entry(model).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/[Model]/orders?range=[rangeMin]-[rangeMax]
       [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<TModel>>> PaginateModel([FromQuery] string range)
        {
            string[] rangeParsed = range.Split('-');
            int rangeMin = Int16.Parse(rangeParsed[0]);
            int rangeMax = Int16.Parse(rangeParsed[1]);

            var url = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            string a = Request.Path;
            string[]  NameModel= a.Split('/');

            var difference = rangeMax - rangeMin;
            var totalCount = _context.Set<TModel>().Where(x => x.Active).Count();

            var first = $"<{url}?range=0-{difference}>; rel='first'";
            var prev = $"<{url}?range={(rangeMin - 1) - difference}-{rangeMin - 1}>; rel='prev'";
            var next = $"<{url}?range={rangeMax + 1}-{rangeMax + 1 + difference}>; rel='next'";
            var last = $"<{url}?range={totalCount - difference}-{totalCount}>; rel='last'";
           
            var query = _context.Set<TModel>().Where(x => x.Active).Skip(rangeMin).Take(rangeMax);
            int countresult = query.Count();

            Response.Headers.Add("Accept-Range", $"{NameModel[2]} {totalCount}");
            Response.Headers.Add("Content-Range", $"{range}/{countresult}");
            Response.Headers.Add("Links", $"{first} , {prev} , {next} , {last}");

           
            return await query.ToListAsync();
        }
        

        

        private bool ModelExists(int id)
        {
            return _context.Set<TModel>().Any(e => e.ID == id);
        }
    }
}