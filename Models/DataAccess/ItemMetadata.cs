using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FinalProject.Models.DataAccess
{
    public partial class Item
    {
        [Required(ErrorMessage = "Please enter a item Id")]
        [Display(Name = "Item Id")]

        [RegularExpression(@"[T][0-9]{4}", ErrorMessage = "Must begin with 'T' followed by four digits")]
        public string Code { get; set; } = null!;
        [Required(ErrorMessage = "Please enter a product name")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = "Please enter a product Description")]
        public string? Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Please enter a positive price")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        public string ItemName
        {
            get
            {
                return Code + "-" + Title;
            }
        }
    }
    
}
