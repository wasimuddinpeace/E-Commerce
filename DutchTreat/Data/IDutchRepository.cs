using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string Category);
        
        IEnumerable<Order> GetAllOrders(bool IncludeItems);
        Order GetOrderById(string name, int id);


        bool SaveAll();
        void AddEntity(object model);
        IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);
        void AddOrder(Order newOrder);
    }
}