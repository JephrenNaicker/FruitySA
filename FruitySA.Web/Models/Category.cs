using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FruitySA.Web.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Category Code")]
        [StringLength(6, ErrorMessage = "Category Code Should only be 6 characters")]
        public string CategoryCode { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Active")]
        public bool isActive { get; set; }
        [Display(Name = "Created Date")]
        //[HiddenInput(DisplayValue = false)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Created By")]
        //[HiddenInput(DisplayValue = false)]
        public string CreatedBy { get; set; } = string.Empty;
        public virtual ICollection<Product> Products { get; set; }
        public Category()
        {
            Products = new List<Product>();
        }

    }
}
