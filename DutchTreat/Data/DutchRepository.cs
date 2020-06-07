using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext context;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger  )
        {
            this.context = context;
            this._logger = logger;
        }

        public void AddEntity(object model)
        {
            context.Add(model);
        }

        public void AddOrder(Order newOrder)
        {
            //Convert new products to loookup of product
            //This code is to check whether the item i.e rpoduct which is been selected by the user 
            // exist in the store or not.  The user can only select items which exist in the 
            //database or in the store.

            foreach(var items in newOrder.Items){

                items.Product = context.Products.Find(items.Product.Id);
            }
            context.Add(newOrder);
        }

        public IEnumerable<Order> GetAllOrders(bool IncludeItems)
        {
            try
            {
                if (IncludeItems)
                {
                    _logger.LogInformation("All Orders were retrieved");

                    return context.Orders.Include(o => o.Items).ThenInclude(i => i.Product).ToList();
                }
            
            else {

                return context.Orders.ToList();
                  }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get the Orders - {ex}");
                return null;
            }
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            try
            {
                if (includeItems)
                {
                    _logger.LogInformation("All Orders were retrieved");

                    return context.Orders
                        .Where(o => o.User.UserName == username)
                        .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                        .ToList();
                }

                else
                {
                    
                    return context.Orders
                        .Where(o => o.User.UserName == username)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get the Orders - {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try {
                _logger.LogInformation("All products were retrieved");

                return context.Products.OrderBy(p => p.Title).ToList();
            }
            catch (Exception ex) {
                _logger.LogError($"Failed to get the products-{ex}");
                return null;
            }
           
        }

        public Order GetOrderById(string name, int id)
        {

            return context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.Id == id && o.User.UserName == name)
                .FirstOrDefault();
     
        }

        public IEnumerable<Product> GetProductsByCategory(string Category)
        {

            return context.Products.Where(p => p.Category == Category).ToList();
        }

        public bool SaveAll() {
            return context.SaveChanges() > 0;
        }


    }
}
