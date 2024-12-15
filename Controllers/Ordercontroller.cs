using FoodOrderAPI.Data;
using FoodOrderAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly FoodDbContext _context;

        public OrderController(FoodDbContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .ToListAsync();
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/Order
        [HttpPost]
        public async Task<IActionResult> PostOrder(Order order)
        {
            try
            {
                // 確保不重複追蹤 Customer 實體
                if (order.Customer != null)
                {
                    _context.Entry(order.Customer).State = EntityState.Unchanged;
                }

                // 計算訂單總金額
                decimal totalAmount = 0;
                foreach (var orderItem in order.OrderItems)
                {
                    // 確保不重複追蹤 MenuItem 實體
                    if (orderItem.MenuItem != null)
                    {
                        _context.Entry(orderItem.MenuItem).State = EntityState.Unchanged;
                    }
                    else
                    {
                        // 從資料庫加載 MenuItem
                        var menuItem = await _context.MenuItems.FindAsync(orderItem.MenuItemID);
                        if (menuItem == null)
                        {
                            return BadRequest($"MenuItem with ID {orderItem.MenuItemID} does not exist.");
                        }
                        orderItem.MenuItem = menuItem;
                    }

                    orderItem.SubTotal = orderItem.MenuItem.Price * orderItem.Quantity;
                    totalAmount += orderItem.SubTotal;
                }

                order.TotalAmount = totalAmount;
                order.OrderDate = DateTime.Now;
                order.Status = "Pending";

                // 保存訂單
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetOrder), new { id = order.OrderID }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        // PUT: api/Order/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderID)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
