using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entityes.Identity;
using WebStore.Domain.Entityes.Orders;
using WebStore.Infrastucture.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Infrastucture.Services.InSQL
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _UserManager;

        public SqlOrderService(WebStoreDB db, UserManager<User> UserManager)
        {
            _db = db;
            _UserManager = UserManager;
        }

        public async Task<Order> CreateOrder(string UserName, CartViewModel Cart, OrderViewModel OrderModel)
        {
            var user = await _UserManager.FindByNameAsync(UserName);
            if (user is null)
                throw new InvalidOperationException($"User {UserName} is not found in the DB");

            await using var transaction = await _db.Database.BeginTransactionAsync();

            var order = new Order
            {
                Name = OrderModel.Name,
                Address = OrderModel.Address,
                Phone = OrderModel.Phone,
                User = user,
                Date = DateTime.Now,
            };

            foreach(var (product_model, quantity)in Cart.Items)
            {
                var product = await _db.Products.FindAsync(product_model.Id);
                if (product is null) continue;

                var order_item = new OrderItem
                {
                    Order = order,
                    Price = product.Price,
                    Quantity = quantity,
                    Product = product,
                };
                order.Items.Add(order_item);
            }
            await _db.Orders.AddAsync(order);

            await _db.SaveChangesAsync();

            await transaction.CommitAsync();

            return order;
        }


        public async Task<Order> GetOrderById(int id) => await _db.Orders
            .Include(order => order.Address)
            .Include(order => order.Items)
            .FirstOrDefaultAsync(order => order.Id == id);

        public async Task<IEnumerable<Order>> GetUserOrders(string UserName) =>await _db.Orders
            .Include(order => order.Address)
            .Include(order => order.Items)
            .Where(order => order.User.UserName == UserName)
            .ToArrayAsync();
    }
}
