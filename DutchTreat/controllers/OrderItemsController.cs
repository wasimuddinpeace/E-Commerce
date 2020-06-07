﻿using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.controllers
{
    [Route("/api/orders/{orderid}/items")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class OrderItemsController : Controller
    {
        private readonly IDutchRepository dutchRepository;
        private readonly ILogger<OrderItemsController> logger;
        private readonly IMapper mapper;

        public OrderItemsController(IDutchRepository dutchRepository, ILogger<OrderItemsController> logger, IMapper mapper)
        {
            this.dutchRepository = dutchRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult get(int orderId) {
            var order = this.dutchRepository.GetOrderById(User.Identity.Name,orderId);
            if (order != null) {
                return Ok(mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
            }
            return BadRequest($"Order Id not  found");
        }

        [HttpGet("{id}")]
        public IActionResult get(int orderId, int id)
        {
            var order = dutchRepository.GetOrderById(User.Identity.Name,orderId);
            if (order != null)
            {
                var item = order.Items.Where(I => I.Id == id).FirstOrDefault();
                if (item != null)
                {
                    return Ok(mapper.Map<OrderItem,OrderItemViewModel>(item));
                }
            }
            return BadRequest($"Item Id not  found");
        }
    }
}
