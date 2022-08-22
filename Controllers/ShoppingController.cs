using FinalProject.Models.DataAccess;
using FinalProject.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;

namespace FinalProject.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly CustomerRecordContext _context;

        public ShoppingController(CustomerRecordContext context)
        {
            _context = context;
        }
        

        public IActionResult Index()
        {
            OrderItemSelection orderItemSelection = new OrderItemSelection();
            List<Order> orders = _context.Orders.ToList<Order>();
            List<int> orderId=new List<int>();
            foreach(Order order in orders)
            {
                orderId.Add(Int32.Parse(order.OrId));
            }
            string temp = (orderId.Max() + 1).ToString();
            while (temp.ToCharArray().Length < 4)
            {
                temp = "0" + temp;
            }
            orderItemSelection.Order.OrId=temp;


            return _context.Items != null ?
                        View(orderItemSelection) :
                        Problem("Entity set 'CustomerRecordContext.Items'  is null.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( OrderItemSelection orderItemSelection)
        {

            List<ItemSelection> itemSelections = new List<ItemSelection>();
            foreach (var item in orderItemSelection.ItemSelections)
            {
                if (item.Quantity > 0)
                {
                    item.Item = _context.Items.FirstOrDefault(i => i.Code == item.Item.Code);
                    itemSelections.Add(item);
                }
            }

            orderItemSelection.ItemSelections = itemSelections;

            if (orderItemSelection.ItemSelections.Count() == 0)
            {
                TempData["ErrorMessage"] = "You must select at least one item by changing the quantity  to be more than 0";
                return RedirectToAction(nameof(Index));
            }

            return View(orderItemSelection);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(OrderItemSelection orderItemSelection)
        {
            if (orderItemSelection.ItemSelections.Count() == 0)
            {
                return NotFound();
            }
            if ( _context.Orders.Any(o => o.OrId== orderItemSelection.Order.OrId)){
                return BadRequest();
            }
            //get customer id
            List<Customer> customers = _context.Customers.ToList<Customer>();
            List<int> customerId = new List<int>();
            foreach (var customer in customers)
            {
                string id = Regex.Match(customer.Id, @"\d+").Value;
                int i = Int32.Parse(id);
                customerId.Add(i);
            }
            string temp = (customerId.Max() + 1).ToString();
            while(temp.ToCharArray().Length < 4)
            {
                temp = "0" + temp;
            }
            orderItemSelection.Customer.Id = "C" + temp;
            orderItemSelection.Order.CustomerId= "C" + temp;
            //get item
 
            foreach (var iss in orderItemSelection.ItemSelections)
            {
                
                iss.Item = _context.Items.FirstOrDefault(i => i.Code == iss.Item.Code);
  
                
            }
 

            _context.Add(orderItemSelection.Customer);
                await _context.SaveChangesAsync();
                _context.Add(orderItemSelection.Order);
                await _context.SaveChangesAsync();

                foreach (var items in orderItemSelection.ItemSelections)
                {
                    OrderItem orderItem = new OrderItem();
                    orderItem.Quantity = items.Quantity;
                    orderItem.OrderId = orderItemSelection.Order.OrId;
                    orderItem.ItemCode = items.Item.Code;
                var errors = ModelState.Values.SelectMany(v => v.Errors);
   
                    _context.Add(orderItem);
                    await _context.SaveChangesAsync();
  
                }
                return View(orderItemSelection); ;
            

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel()
        {
            return RedirectToAction(nameof(Index));
        }
      }
}
