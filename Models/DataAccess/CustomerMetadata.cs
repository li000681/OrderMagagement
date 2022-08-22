using System.ComponentModel.DataAnnotations;
using System.Linq;
using FinalProject.Models.DataAccess;
namespace FinalProject.Models.DataAccess
{
    public partial class Customer
    {
        [Required(ErrorMessage = "Please enter a customer Id")]
        [Display(Name = "Customer Id")]

        [RegularExpression(@"[C][0-9]{4}", ErrorMessage = "Must begin with 'C' followed by four digits")]
        public string Id { get; set; } = null!;
        [Required(ErrorMessage = "Please enter a customer name")]
        [Display(Name = "Customer Name")]

        [RegularExpression(@"[a-zA-Z]+\s+[a-zA-Z]+", ErrorMessage = "Must be in the form of first name followed by last name")]
        public string Name { get; set; } = null!;
        public string CombineTitle { get
            {
                return Id + "-" + Name;
            } }
        public Customer()
        {
            Orders = new HashSet<Order>();
        }
        public virtual ICollection<Order>? Orders { get; set; }



        public int? OrdersNumber
        {
            get
            {
                return Orders.Count();
            }
        } 
        public string TotalCost { get
            {
                decimal? total = 0;
                
                using (CustomerRecordContext context = new CustomerRecordContext())
                {
                    List<OrderItem> OrderItems = new List<OrderItem>();
                    List<Item> Items = new List<Item>();
                    foreach (var item in Orders)
                    {
                        List<OrderItem> OI = new List<OrderItem>();
                        OI = (from oi in context.OrderItems where oi.OrderId == item.OrId select oi).ToList<OrderItem>();
                        OrderItems.AddRange(OI);
                    }
                    foreach(var item in OrderItems)
                    {
                        total+=(from i in context.Items where i.Code==item.ItemCode select i).FirstOrDefault<Item>().Price*item.Quantity;
                    }
                }
                decimal total1 = total.GetValueOrDefault();
                return total1.ToString("c");    
            } 
        }

    }
}
