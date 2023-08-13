using OMSBlazor.Northwind.Common;
using OMSBlazor.Northwind.EmployeeAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.EmployeeAggregate
{
    public class Employee : AggregateRoot<int>
    {
        public Employee(int id, string firstName, string lastName)
            :base(id)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
        }

        public string LastName { get; private set; }

        public string FirstName { get; private set; }

        public string Title { get; set; }

        public string TitleOfCoursery { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime HireDate { get; set; }

        public AddressObject Address { get; set; }

        public string HomePhone { get; set; }

        public string Extension { get; set; }

        public string Notes { get; set; }

        public int ReportsTo { get; set; }

        public string PhotoPath { get; set; }

        public void SetLastName(string lastName)
        {
            if (lastName == null) 
            { 
                throw new EmptyLastNameException(); 
            }

            LastName = lastName;
        }

        public void SetFirstName(string firstName)
        {
            if (firstName == null)
            {
                throw new EmptyFirstNameException();
            }

            FirstName = firstName;
        }
    }
}
