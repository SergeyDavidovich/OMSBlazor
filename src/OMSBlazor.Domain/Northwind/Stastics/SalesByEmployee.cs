using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.Stastics
{
    public class SalesByEmployee : Entity<int>
    {
        private SalesByEmployee() { }

        public SalesByEmployee(int id, string lastName)
        {
            ID = id;
            LastName = lastName;
        }
        
        public int ID { get; private set; }
        public string LastName { get; private set; }
        public decimal Sales { get; set; }
    }
}
