using System;
using System.Collections.Generic;

#nullable disable

namespace FarmerCentralAPI.Models
{
    public partial class Product
    {
        public Product()
        {
            UsersProducts = new HashSet<UsersProduct>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public virtual ICollection<UsersProduct> UsersProducts { get; set; }
    }
}
