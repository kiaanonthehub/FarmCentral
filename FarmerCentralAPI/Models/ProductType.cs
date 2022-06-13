using System;
using System.Collections.Generic;

#nullable disable

namespace FarmerCentralAPI.Models
{
    public partial class ProductType
    {
        public ProductType()
        {
            UsersProducts = new HashSet<UsersProduct>();
        }

        public int ProductTypeId { get; set; }
        public string ProductType1 { get; set; }

        public virtual ICollection<UsersProduct> UsersProducts { get; set; }
    }
}
