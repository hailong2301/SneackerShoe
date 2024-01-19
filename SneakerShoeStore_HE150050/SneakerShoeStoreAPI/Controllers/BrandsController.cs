using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using DataAccess.Repository.Products;
using Microsoft.AspNetCore.OData.Query;
using DataAccess.Repository.Brands;

namespace SneakerShoeStoreAPI.Controllers
{

    public class BrandsController : ODataController
    {
        //private readonly SneakerShoeStoreContext _context;

        //public BrandsController(SneakerShoeStoreContext context)
        //{
        //    _context = context;
        //}
        private IBrandRepository repository = new BrandRepository();


        [EnableQuery]
        [HttpGet]
        public ActionResult<IEnumerable<Brand>> Get()
        {

            return repository.GetBrands().AsQueryable().ToList();
        }


        //// GET: api/Brands
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        //{
        //  if (_context.Brands == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.Brands.ToListAsync();
        //}

        //// GET: api/Brands/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Brand>> GetBrand(int id)
        //{
        //  if (_context.Brands == null)
        //  {
        //      return NotFound();
        //  }
        //    var brand = await _context.Brands.FindAsync(id);

        //    if (brand == null)
        //    {
        //        return NotFound();
        //    }

        //    return brand;
        //}

        //// PUT: api/Brands/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBrand(int id, Brand brand)
        //{
        //    if (id != brand.BrandId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(brand).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BrandExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Brands
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        //{
        //  if (_context.Brands == null)
        //  {
        //      return Problem("Entity set 'SneakerShoeStoreContext.Brands'  is null.");
        //  }
        //    _context.Brands.Add(brand);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetBrand", new { id = brand.BrandId }, brand);
        //}

        //// DELETE: api/Brands/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteBrand(int id)
        //{
        //    if (_context.Brands == null)
        //    {
        //        return NotFound();
        //    }
        //    var brand = await _context.Brands.FindAsync(id);
        //    if (brand == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Brands.Remove(brand);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool BrandExists(int id)
        //{
        //    return (_context.Brands?.Any(e => e.BrandId == id)).GetValueOrDefault();
        //}
    }
}
