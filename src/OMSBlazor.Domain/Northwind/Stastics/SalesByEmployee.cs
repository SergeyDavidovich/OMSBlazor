using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.Northwind.Stastics
{
    public class SalesByEmployee
    {
        private SalesByEmployee() { }

        public SalesByEmployee(int id, string lastName)
        {
            ID = id;
            LastName = lastName;
        }

        public int ID { get; }
        public string LastName { get; }
        public decimal Sales { get; set; }
    }
}
