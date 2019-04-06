using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BasketApp.Common.Contracts;
using BasketApp.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasketApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController: ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET api/products
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(_mapper.Map<List<ProductModel>>(await _context.Products.ToListAsync()));
        }
    }
}