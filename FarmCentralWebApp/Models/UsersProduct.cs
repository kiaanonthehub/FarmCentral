using System;

namespace FarmCentralWebApp.Models
{
    public class UsersProduct
    {
        public int UsersProductId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public int? Quantity { get; set; }
        public string ProductType { get; set; }
        public DateTime ProductDate { get; set; }
    }
}
