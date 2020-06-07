using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace DutchTreat.controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController:Controller
    {
        private readonly IDutchRepository _dutchrepository;
        private readonly ILogger<OrdersController> _ilogger;
        private readonly IMapper _mapper;
        private readonly UserManager<StoreUser> userManager;

        public OrdersController(IDutchRepository dutchrepository, ILogger<OrdersController> Ilogger,IMapper mapper,UserManager<StoreUser> userManager)
        {
            this._dutchrepository = dutchrepository;
            this._ilogger = Ilogger;
            this._mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Order>> Get(bool IncludeItems = true) {

           
         try   {

                var username = User.Identity.Name;

                var results = this._dutchrepository.GetAllOrdersByUser(username,IncludeItems);

                return Ok(_mapper.Map<IEnumerable <Order>, IEnumerable<OrderViewModel> >(results));
            }
            catch (Exception ex) {

                _ilogger.LogError($"Faield to get the orders - {ex}");

                return BadRequest($"Failed to get the orders");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {

            try
            {
                var order = _dutchrepository.GetOrderById(User.Identity.Name,id);
                if (order != null)
                {
                    return Ok(_mapper.Map <Order, OrderViewModel> (order));
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {

                _ilogger.LogError($"Faield to get the orders - {ex}");

                return BadRequest($"Failed to get the orders");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Since entities goes inside the database we are mapping our order enttity to the view model
                    //Since we cannot save view model to the dattabase
                    Order newOrder = _mapper.Map<OrderViewModel, Order>(model);

                    //Minvalue means if the ordate is not specified or it is null
                    if (newOrder.OrderDate == DateTime.MinValue) {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    var currentUser = userManager.FindByNameAsync(User.Identity.Name);
                    newOrder.User = await currentUser;
                    // _dutchrepository.AddEntity(newOrder);

                    _dutchrepository.AddOrder(newOrder);
                    if (_dutchrepository.SaveAll())
                    {
                  
                        //After saveAll() this update the model id and noe we pass the updated model with the updated
                        //modelId to the Postman   
                        return Created($"/api/orders/{newOrder.Id}", _mapper.Map<Order, OrderViewModel>(newOrder));
                    }
                }
                else {

                    return BadRequest(ModelState);
                }
            }

            catch (Exception ex)
            {

                _ilogger.LogError($"Failed to save ordr - {ex}");
            }

            return BadRequest($"Unable to save Order");
        }
    }
}
