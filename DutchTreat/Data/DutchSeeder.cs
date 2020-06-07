using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext ctx;
        private readonly IHostEnvironment hosting;
        private readonly UserManager<StoreUser> userManager;

        public DutchSeeder(DutchContext _ctx,IHostEnvironment _hosting,UserManager<StoreUser> UserManager)
        {
            ctx = _ctx;
            hosting = _hosting;
            userManager = UserManager;
        }

        public async Task SeedAsync() {

            ctx.Database.EnsureCreated();
            
            //Identity code for a user

            StoreUser user =  await userManager.FindByEmailAsync("waseem.uddin@hcl.com");
            
            if (user == null) {
                
                user = new StoreUser()
                {
                    Firstame = "Waseem",
                    LastName = "Uddin",
                    Email = "waseem.uddin@hcl.com",
                    UserName = "Waseem584"
                };
                
                var result = await userManager.CreateAsync(user,"P@ssw0rd!");

                if (result != IdentityResult.Success) {

                    throw new InvalidOperationException("Could not create new user in the seeder");
                }
            }
            if (!ctx.Products.Any()) {
                //Need to create sample data
               
                var filePath = Path.Combine(hosting.ContentRootPath,"Data/art.json");
                
                var json = File.ReadAllText(filePath);
                
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                
                ctx.Products.AddRange(products);

                var order = ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();
                
                if (order != null)
                {
                    //This code will make sure that the user is the authenticated user update the order to be own by 
                    // this user
                    order.User = user;
                   
                    order.Items = new List<OrderItem>() {
                    new OrderItem()
                    {
                        Product = products.First(),
                        Quantity = 5,
                        UnitPrice = products.First().Price
                    }
                };
                }
                ctx.SaveChanges();

            }
        }
    }
}
