using OMSBlazor.Dto.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace OMSBlazor.Dto.Customer
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }

        public string CompanyName { get; set; }

        public string ContactName { get; set; }

        public string ContactTitle { get; set; }

        public AddressObjectDto Address { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }
    }
}
