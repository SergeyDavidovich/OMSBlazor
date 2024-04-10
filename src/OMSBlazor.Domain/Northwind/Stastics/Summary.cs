using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.Stastics
{
    public class Summary : Entity<string>
    {
        public string SummaryName { get; set; }

        public double SummaryValue { get; set; }
    }
}
