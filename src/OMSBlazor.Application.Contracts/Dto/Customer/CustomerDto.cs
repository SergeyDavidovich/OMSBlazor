using OMSBlazor.Dto.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace OMSBlazor.Dto.Customer
{
    public class CustomerDto
    {
        public string CompanyName { get; private set; }

        public string ContactName { get; set; }

        public string ContactTitle { get; set; }

        public AddressObjectDto Address { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }
    }
}
