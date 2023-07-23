using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace FruitySA.Web.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string? ProductCode { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [StringLength(100, ErrorMessage = "Too Long Message")]
        public string? Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Enter amount > 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set;}

        [Display(Name = "Image")]
        public string? ImagePath { get; set;} = string.Empty;

        //[Required(ErrorMessage = "Select a Category")]
        [Display(Name = "Category")]
        public int? CategoryId { get; set;}

        [Display(Name = "Created Date")]
     
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Created By")]
     
        public string CreatedBy { get; set; } = string.Empty;

        public virtual Category? Category { get; set; }


    }
}
