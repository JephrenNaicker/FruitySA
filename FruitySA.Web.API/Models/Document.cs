using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FruitySA.Web.API.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }
        [Display(Name = "Excel Document")]
        [Required(ErrorMessage = "Please Upload Excel Document")]
        public string ExcelDocumentPath { get; set; } = string.Empty;

        [Display(Name = "User Uploaded")]
        public string UserUploaded { get; set; }=string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Date Uploaded")]
        public DateTime ExcelDateupload { get; set; } = DateTime.Now;


    }
}
