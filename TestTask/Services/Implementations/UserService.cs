using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser()
        {
            var users = await _context.Users.ToListAsync();
            var orders = await _context.Orders.Where(o => o.CreatedAt.Year == 2003 && o.Status == OrderStatus.Delivered).ToListAsync();
            // Count total amount of product for each user
            var userOrderAmounts = users.ToDictionary(user => user.Id, user => 0);
            foreach (var order in orders)
            {
                if (userOrderAmounts.ContainsKey(order.UserId))
                {
                    userOrderAmounts[order.UserId] += order.Price * order.Quantity;
                }
            }
            // Get user with max product amount in 2003
            var topUser = users.OrderByDescending(user => userOrderAmounts[user.Id]).FirstOrDefault();

            return topUser;
        }
        public async Task<List<User>> GetUsers()
        {
            var orders = await _context.Orders.Where(o => o.CreatedAt.Year == 2010 && o.Status == OrderStatus.Paid).ToListAsync();
            // Get Ids list users with paid orders in 2010  
            var userIds = orders.Select(order => order.UserId).Distinct().ToList();
            // Get users 
            var users = await _context.Users.Where(user => userIds.Contains(user.Id)).ToListAsync();

            return users;
        }
    }
}
