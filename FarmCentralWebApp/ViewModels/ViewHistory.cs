using System;
using System.ComponentModel.DataAnnotations;

namespace FarmCentralWebApp.ViewModels
{
    public class ViewHistory
    {
        public int UserId { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public string ProductType { get; set; }
        public DateTime ProductDate { get; set; }
    }
}
