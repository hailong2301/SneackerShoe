using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using AutoMapper;
using System.Net.Http.Headers;
using System.Text.Json;
using SneakerShoeStoreClient.Authorization;

namespace SneakerShoeStoreClient.Controllers
{
    [CustomAuthorizationFilter]
    public class ProductSizesController : Controller
    {
        private readonly SneakerShoeStoreContext _context;
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private IMapper _mapper;

        public ProductSizesController(SneakerShoeStoreContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }

        // GET: ProductSizes
        public async Task<IActionResult> Index(int? id)
        {

            //HttpResponseMessage response = await client.GetAsync(ApiPort + "api/ProductSizes/" + id);
            //string strData = await response.Content.ReadAsStringAsync();
            //var options = new JsonSerializerOptions
            //{
            //    PropertyNameCaseInsensitive = true,
            //};
            //var productSizes = JsonSerializer.Deserialize<ProductSize>(strData, options);
            var productSizes = _context.ProductSizes.Where(p => p.ProductId == id).Include(p =>p.Product).Include(s => s.Size).ToList();
            ViewData["productId"] = id;
            //foreach (var item in productSizes)
            //{
            //    item.Size = _context.Sizes.SingleOrDefault(x => x.SizeId == item.SizeId);
            //}
            return View(productSizes);
        }

        // GET: ProductSizes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductSizes == null)
            {
                return NotFound();
            }

            var productSize = await _context.ProductSizes
                .Include(p => p.Product)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.ProductSizeId == id);
            if (productSize == null)
            {
                return NotFound();
            }

            return View(productSize);
        }

        // GET: ProductSizes/Create
        public IActionResult Create(int id)
        {
            ViewData["ProductId"] = id;
            var list = _context.ProductSizes.Where(_p => _p.ProductId == id).ToList();
            var listSize = _context.Sizes.ToList();
            foreach (var product in list)
            {
                foreach (var item in _context.Sizes.ToList())
                {
                    if(product.SizeId == item.SizeId)
                    {
                        listSize.Remove(item);
                    }
                }
            }
            ViewData["SizeId"] = new SelectList(listSize, "SizeId", "SizeId");
            return View();
        }

        // POST: ProductSizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,SizeId,Quantity,ProductSizeId")] ProductSize productSize)
        {
          
                _context.Add(productSize);
                await _context.SaveChangesAsync();
            
            return Redirect($"/ProductSizes/Index/{productSize.ProductId}");
        }

        // GET: ProductSizes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductSizes == null)
            {
                return NotFound();
            }

            var productSize = await _context.ProductSizes.FindAsync(id);
            if (productSize == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = productSize.ProductId;
            ViewData["SizeId"] = productSize.SizeId;
            return View(productSize);
        }

        // POST: ProductSizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,SizeId,Quantity,ProductSizeId")] ProductSize productSize)
        {
            if (id != productSize.ProductSizeId)
            {
                return NotFound();
            }
                try
                {
                    _context.Update(productSize);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSizeExists(productSize.ProductSizeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            
          
            return Redirect($"/ProductSizes/Index/{productSize.ProductId}");
        }

        // GET: ProductSizes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductSizes == null)
            {
                return NotFound();
            }

            var productSize = await _context.ProductSizes
                .Include(p => p.Product)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.ProductSizeId == id);
            if (productSize == null)
            {
                return NotFound();
            }

            return View(productSize);
        }

        // POST: ProductSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductSizes == null)
            {
                return Problem("Entity set 'SneakerShoeStoreContext.ProductSizes'  is null.");
            }
            var productSize = await _context.ProductSizes.FindAsync(id);
            if (productSize != null)
            {
                _context.ProductSizes.Remove(productSize);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductSizeExists(int id)
        {
            return (_context.ProductSizes?.Any(e => e.ProductSizeId == id)).GetValueOrDefault();
        }
    }
}
