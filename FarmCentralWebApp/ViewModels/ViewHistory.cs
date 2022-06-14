using System;
using System.ComponentModel.DataAnnotations;

namespace FarmCentralWebApp.ViewModels
{
    public class ViewHistory
    {
        public ViewHistory()
        {

        }

        public ViewHistory(int userId, string productName, int quantity, string productType, DateTime productDate)
        {
            UserId = userId;
            ProductName = productName;
            Quantity = quantity;
            ProductType = productType;
            ProductDate = productDate;
        }

        public int UserId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string ProductType { get; set; }
        public DateTime ProductDate { get; set; }
    }
}
