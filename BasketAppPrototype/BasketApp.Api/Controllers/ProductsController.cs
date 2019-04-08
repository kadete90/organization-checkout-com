using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BasketApp.Api.Utils;
using BasketApp.Common.Contracts;
using BasketApp.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BasketApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController: ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public ProductsController(ApplicationDbContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        // GET api/products
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var cacheProducts = await _cache.GetOrCreateAsync(CacheKeys.Products, async cache =>
                {
                    cache.SlidingExpiration = TimeSpan.FromSeconds(3);
                    return _mapper.Map<List<ProductModel>>(await _context.Products.ToListAsync());
                });

            return Ok(cacheProducts);
        }
    }
}