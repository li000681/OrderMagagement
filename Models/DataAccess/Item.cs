using System;
using System.Collections.Generic;



namespace FinalProject.Models.DataAccess
{

    public partial class Item
    {
        public Item()
        {
            OrderItems = new HashSet<OrderItem>();
        }

       
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}
