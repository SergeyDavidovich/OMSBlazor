using System;
using System.Collections.Generic;
using System.Text;

namespace OMSBlazor.Dto.Product
{
    public class UpdateProductDto
    {
        public int CategoryId { get; set; }

        public string ProductName { get; set; }

        public string QuantityPerUnit { get; set; }

        public double UnitPrice { get; set; }

        public virtual int UnitsInStock { get; set; }

        public int UnitsOnOrder { get; set; }

        public int ReorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }
}
