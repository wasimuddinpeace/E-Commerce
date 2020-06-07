using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IDutchRepository _dutchRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IDutchRepository dutchRepository, ILogger<ProductsController> logger)
        {
            this._dutchRepository = dutchRepository;
            this._logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
            //The Only Action return type will help tools like swagger, to documents apis appropritely
        {
            try
            {
                return Ok(_dutchRepository.GetAllProducts());
            }
            catch (Exception ex) {
                _logger.LogError($"Failed to fetch the Products-{ex}");
                return BadRequest("Failed to get the products");
            }
        }
    }

}