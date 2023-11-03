using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OMSBlazor.Northwind.OrderAggregate.Exceptions
{
    public class DependentReporterExistException : BusinessException
    {
        public DependentReporterExistException(int id, int reporterId)
            : base(message: 
                  $"Employee with id - {reporterId} reports to employee that you want to delete. Id of employee that you want to delete is - {id}")
        {
            
        }
    }
}
