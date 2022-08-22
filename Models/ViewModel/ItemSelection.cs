using FinalProject.Models.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.ViewModel
{
    public class ItemSelection
    {
       
        public Item Item { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "the item quantity can not be negative")]
        public int Quantity { get; set; }= 0;
        public ItemSelection()
        {
            Item = null;
            Quantity = 0;
        }
        public ItemSelection(Item item, int quanlity=0)
        {
            Item = item;
            this.Quantity = quanlity;
        }
    }
}
