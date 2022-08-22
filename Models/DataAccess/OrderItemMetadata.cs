using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.DataAccess
{
    public partial class OrderItem
    {
        public string ItemCode { get; set; } = null!;
        public string OrderId { get; set; } = null!;
        [Required(ErrorMessage = "Please enter a quantity")]
        [Display(Name = "quantity")]

        [Range(1,int.MaxValue, ErrorMessage = "Must great than or equal to 1")]
        public int? Quantity { get; set; }
        public string ItemName
        {
            get
            {
                string itemName = "";
                using (CustomerRecordContext context = new CustomerRecordContext())
                {
                   itemName = ItemCode + "-"+ (from i in context.Items where i.Code == ItemCode select i).FirstOrDefault<Item>().Title;
                }
                return itemName;
            }
        }

        public virtual Item? ItemCodeNavigation { get; set; } = null!;
        public virtual Order? Order { get; set; } = null!;
        
    }
}
