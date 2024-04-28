using OMSBlazor.Dto.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace OMSBlazor.Interfaces.Services
{
    public interface IStasticsRecalculator
    {
        public Task RecalculateStastics(OrderDto order);
    }
}
