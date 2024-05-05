using OMSBlazor.Dto.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace OMSBlazor.Interfaces.Services
{
    public interface IStasticsRecalculator : IRemoteService
    {
        public Task RecalculateStastics(int orderId);
    }
}
