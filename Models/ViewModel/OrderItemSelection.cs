using FinalProject.Models.DataAccess;
namespace FinalProject.Models.ViewModel
{
    public class OrderItemSelection
    {
        public Order Order { get; set; }
        public Customer Customer { get; set; }
        public List<ItemSelection> ItemSelections { get; set; }
        public OrderItemSelection()
        {
            Order=new Order();
            ItemSelections=new List<ItemSelection>();
            CustomerRecordContext context = new CustomerRecordContext();
            foreach(var item in context.Items)
            {
                ItemSelection itemSelection = new ItemSelection(item);
                ItemSelections.Add(itemSelection);
            }

        }
    }
}
