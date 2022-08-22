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
    public class OrderItemsController : Controller
    {
        private readonly CustomerRecordContext _context;

        public OrderItemsController(CustomerRecordContext context)
        {
            _context = context;
        }

        // GET: OrderItems
        public async Task<IActionResult> Index()
        {
            var customerRecordContext = _context.OrderItems.Include(o => o.ItemCodeNavigation).Include(o => o.Order);
            return View(await customerRecordContext.ToListAsync());
        }

        // GET: OrderItems/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.OrderItems == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .Include(o => o.ItemCodeNavigation)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // GET: OrderItems/Create
        public IActionResult Create()
        {
            ViewData["ItemCode"] = new SelectList(_context.Items, "Code", "ItemName");
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrId", "OrId");
            return View();
        }

        // POST: OrderItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemCode,OrderId,Quantity")] OrderItem orderItem)
        {
            
            if (_context.OrderItems.Any(e => e.ItemCode == orderItem.ItemCode &&  e.OrderId == orderItem.OrderId))
            {
                ModelState.AddModelError("ItemCode", "This Item already exists in this order!");
            }
            if (ModelState.IsValid)
            {
                _context.Add(orderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemCode"] = new SelectList(_context.Items, "Code", "ItemName", orderItem.ItemName);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrId", "OrId", orderItem.OrderId);
            return View(orderItem);
        }

        // GET: OrderItems/Edit/5
        public async Task<IActionResult> Edit(string code,string id)
        {
            if (id == null || _context.OrderItems == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems.FindAsync(id, code);
            orderItem.ItemCodeNavigation= await _context.Items.FindAsync(code);
            orderItem.Order= await _context.Orders.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            ViewData["ItemCode"] = new SelectList(_context.Items, "Code", "Code", orderItem.ItemCode);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrId", "OrId", orderItem.OrderId);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderItem orderItem)
        {
            
            
            orderItem.ItemCodeNavigation = await _context.Items.FindAsync(orderItem.ItemCode);
            orderItem.Order = await _context.Orders.FindAsync(orderItem.OrderId);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.ItemCode,orderItem.OrderId))
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
            ViewData["ItemCode"] = new SelectList(_context.Items, "Code", "Code", orderItem.ItemCode);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrId", "OrId", orderItem.OrderId);
            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        public async Task<IActionResult> Delete(string code,string id)
        {
            if (id == null || _context.OrderItems == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .Include(o => o.ItemCodeNavigation)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.OrderId == id && m.ItemCode==code);
            orderItem.ItemCodeNavigation = await _context.Items.FindAsync(code);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string ItemCode, string OrderId)
        {
            if (_context.OrderItems == null)
            {
                return Problem("Entity set 'CustomerRecordContext.OrderItems'  is null.");
            }
            var orderItem = await _context.OrderItems.FindAsync(OrderId, ItemCode);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(string code,string id)
        {
          return (_context.OrderItems?.Any(e => e.OrderId == id && e.ItemCode==code)).GetValueOrDefault();
        }
    }
}
