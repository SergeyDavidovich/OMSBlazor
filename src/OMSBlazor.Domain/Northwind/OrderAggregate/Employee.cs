using OMSBlazor.Northwind.OrderAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.OrderAggregate
{
    public class Employee : Entity<int>
    {
        public Employee(int id, string firstName, string lastName)
            : base(id)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
        }

        public string? LastName { get; private set; }

        public string? FirstName { get; private set; }

        public string? Title { get; set; }

        public string? TitleOfCourtesy { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime HireDate { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Region { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        public string? HomePhone { get; set; }

        public string? Extension { get; set; }

        public string? Notes { get; set; }

        public int? ReportsTo { get; set; }

        public string? PhotoPath { get; set; }

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
