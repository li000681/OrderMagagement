using System.ComponentModel.DataAnnotations;
using System.Linq;

using FinalProject.Models.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Models.DataAccess
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }
        [Required(ErrorMessage = "Please enter a order Id")]
        [Display(Name = "Order Id")]

        [RegularExpression(@"[0-9]{4}", ErrorMessage = "Must be four digits")]
        public string OrId { get; set; } = null!;
        [Required(ErrorMessage = "Please enter a customer Id")]
        [Display(Name = "Customer Id")]

        [RegularExpression(@"[C][0-9]{4}", ErrorMessage = "Must begin with 'C' followed by four digits")]
        public string CustomerId { get; set; } = null!;



        public virtual Customer? Customer { get; set; } = null!;
        public virtual ICollection<OrderItem>? OrderItems { get; set; }


        public string Cost
        {
            get
            {
                decimal? cost = 0;
                using (CustomerRecordContext context = new CustomerRecordContext())
                {
                    

                    foreach (var item in OrderItems)
                    {
                        cost += (from i in context.Items where i.Code == item.ItemCode select i).FirstOrDefault<Item>().Price * item.Quantity;
                    }

                }
                return cost.GetValueOrDefault().ToString("c");
            }
        }
    }
}
