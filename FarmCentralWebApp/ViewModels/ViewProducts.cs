using System;

namespace FarmCentralWebApp.ViewModels
{
    public class ViewProducts
    {
        public int UserId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string ProductType { get; set; }
        public DateTime ProductDate { get; set; }
    }
}
