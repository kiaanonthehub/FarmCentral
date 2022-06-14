using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmCentralWebApp.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
