using System.ComponentModel.DataAnnotations;

namespace FarmCentralWebApp.Models
{
    public class ProductType
    {
        [Key]
        public int ProductTypeId { get; set; }
        public string ProductType1 { get; set; }
    }
}
