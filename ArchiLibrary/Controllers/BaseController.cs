﻿using ArchiLibrary.Models;
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
        public async Task<ActionResult<IEnumerable<TModel>>> GetAll()
        {
            return await _context.Set<TModel>().Where(x => x.Active == true).ToListAsync();
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
        // GET: api/[Model]/Tri
        [HttpGet("Tri")]
        public async Task<ActionResult<IEnumerable<TModel>>> TriTModel([FromQuery] string sort)
        {
            var source = _context.Set<TModel>().Where(x => x.Active);


            var parameter = Expression.Parameter(typeof(TModel), "x");
            Expression property = Expression.Property(parameter, sort);
            var lambda = Expression.Lambda(property, parameter);


            var orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
            var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TModel), property.Type);
            var result = orderByGeneric.Invoke(null, new object[] { source, lambda });

            return await ((IOrderedQueryable<TModel>)result).ToListAsync();



        }

        private bool ModelExists(int id)
        {
            return _context.Set<TModel>().Any(e => e.ID == id);
        }
    }
}