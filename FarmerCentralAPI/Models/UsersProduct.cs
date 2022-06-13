using System;
using System.Collections.Generic;

#nullable disable

namespace FarmerCentralAPI.Models
{
    public partial class UsersProduct
    {
        public int UsersProductId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public int? Quantity { get; set; }
        public string ProductType { get; set; }
        public DateTime ProductDate { get; set; }

        public virtual Product Product { get; set; }
        public virtual ProductType ProductTypeNavigation { get; set; }
        public virtual User User { get; set; }
    }
}
