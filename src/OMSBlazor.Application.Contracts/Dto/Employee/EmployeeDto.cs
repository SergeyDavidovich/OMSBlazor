using OMSBlazor.Dto.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace OMSBlazor.Dto.Employee
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Title { get; set; }

        public string TitleOfCoursery { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime HireDate { get; set; }

        public AddressObjectDto Address { get; set; }

        public string HomePhone { get; set; }

        public string Extension { get; set; }

        public string Notes { get; set; }

        public int ReportsTo { get; set; }

        public string PhotoPath { get; set; }
    }
}
