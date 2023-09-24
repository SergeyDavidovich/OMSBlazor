using OMSBlazor.Northwind.CustomerAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.CustomerAggregate
{
    public class Customer: AggregateRoot<string>
    {
        private Customer()
        {

        }

        internal Customer(string id, string companyName):
            base(id)
        {
            SetCompanyName(companyName);
        }

        public string? CompanyName { get; private set; }

        public string? ContactName { get; set; }

        public string? ContactTitle { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Region { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        public string? Phone { get; set; }

        public string? Fax { get; set; }

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
