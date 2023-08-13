using OMSBlazor.Northwind.Common;
using OMSBlazor.Northwind.CustomerAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.CustomerAggregate
{
    public class Customer: AggregateRoot<int>
    {
        public Customer(int id, string companyName):
            base(id)
        {
            SetCompanyName(companyName);
        }

        public string CompanyName { get; private set; }

        public string ContactName { get; set; }

        public string ContactTitle { get; set; }

        public AddressObject Address { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public void SetCompanyName(string companyName)
        {
            if (companyName == null)
            {
                throw new EmptyCustomerNameException();
            }

            this.CompanyName = companyName;
        }
    }
}
