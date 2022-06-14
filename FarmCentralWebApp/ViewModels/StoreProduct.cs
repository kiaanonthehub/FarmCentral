using System;
using System.ComponentModel.DataAnnotations;

namespace FarmCentralWebApp.ViewModels
{
    public class StoreProduct
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string ProductType { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime ProductDate { get; set; }
    }
}
