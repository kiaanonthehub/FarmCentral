using System;
using System.ComponentModel.DataAnnotations;

namespace FarmCentralWebApp.ViewModels
{
    public class StoreProduct
    {
        [Required(ErrorMessage = "A valid product name is required")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Enter an amount, else enter 0")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "A valid type/category is required")]
        public string ProductType { get; set; }
        [Required(ErrorMessage = "Please track or back-track a date for current product to be added")]
        public DateTime ProductDate { get; set; }
    }
}
