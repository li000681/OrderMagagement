using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models.DataAccess;

namespace FinalProject.Controllers
{
    public class OrdersController : Controller
    {
        private readonly CustomerRecordContext _context;

        public OrdersController(CustomerRecordContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var customerRecordContext = _context.Orders.Include(o => o.Customer).Include(o=>o.OrderItems);
            return View(await customerRecordContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders
                .Include(o => o.Customer).Include(o => o.OrderItems)
                .FirstOrDefaultAsync(m => m.OrId == id);
            foreach (var item in order.OrderItems)
            {
                item.ItemCodeNavigation = await _context.Items
                .FirstOrDefaultAsync(m => m.Code == item.ItemCode);
  //              cost += item.ItemCodeNavigation.Price * item.Quantity;
            }

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "CombineTitle");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrId,CustomerId")] Order order)
        {
            if (_context.Orders.Any(e => e.OrId == order.OrId))
            {
                ModelState.AddModelError("OrId", "This Id already exists!");
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "CombineTitle", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(o=>o.Customer).FirstOrDefaultAsync(o=>o.OrId==id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "CombineTitle", order.CustomerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("OrId,CustomerId")] Order order)
        {
            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "CombineTitle", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string OrId)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'CustomerRecordContext.Orders'  is null.");
            }
            var order = await _context.Orders.Include(o => o.OrderItems)
                .FirstOrDefaultAsync(m => m.OrId == OrId);
            if (order != null)
            {
                if (order.OrderItems.Count() != 0)
                {
                    foreach (var orderItem in order.OrderItems)
                    {

                        _context.OrderItems.Remove(orderItem);

                    }
                }
                _context.Orders.Remove(order);
            }
            
            
            
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(string id)
        {
          return (_context.Orders?.Any(e => e.OrId == id)).GetValueOrDefault();
        }
    }
}
