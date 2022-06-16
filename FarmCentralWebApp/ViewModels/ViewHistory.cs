using System;

namespace FarmCentralWebApp.ViewModels
{
    public class ViewHistory
    {
        public ViewHistory()
        {

        }

        public ViewHistory(int userProductId, int userId, string productName, int quantity, string productType, DateTime productDate)
        {
            UsersProductId = userProductId;
            UserId = userId;
            ProductName = productName;
            Quantity = quantity;
            ProductType = productType;
            ProductDate = productDate;
        }

        public int UsersProductId { get; set; }
        public int UserId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string ProductType { get; set; }
        public DateTime ProductDate { get; set; }
    }
}
