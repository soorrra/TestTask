using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrder()
        {
            var orders = await _context.Orders.Where(o => o.Quantity > 1).ToListAsync();
            // Sort orders by creation date
            orders.Sort((a, b) => b.CreatedAt.CompareTo(a.CreatedAt));
            // Get the newest order
            var newOrder = orders.FirstOrDefault();

            return newOrder;
        }

        public async Task<List<Order>> GetOrders()
        {
            var users = await _context.Users.Where(u => u.Status == UserStatus.Active).ToListAsync();
            // getting active user Ids
            var userIds = users.Select(user => user.Id).ToList();
            // Get orders from active users
            var orders = await _context.Orders.Where(o => userIds.Contains(o.UserId)).ToListAsync();
            // Sort orders by creation date
            orders.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));

            return orders;
        }

    }
}
