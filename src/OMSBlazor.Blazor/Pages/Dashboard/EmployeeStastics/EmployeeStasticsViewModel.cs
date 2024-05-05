using DynamicData;
using DynamicData.Binding;
using OMSBlazor.Dto.Customer.Stastics;
using OMSBlazor.Dto.Employee.Stastics;
using OMSBlazor.Interfaces.ApplicationServices;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OMSBlazor.Blazor.Pages.Dashboard.EmployeeStastics
{
    public class EmployeeStasticsViewModel : ReactiveObject
    {
        private readonly IEmployeeApplicationService _employeeApplicationService;

        private readonly ReadOnlyObservableCollection<SalesByEmployeeDto> _salesByEmployee;

        private readonly SourceCache<SalesByEmployeeDto, int> _salesByEmployeeSource;

        public EmployeeStasticsViewModel(IEmployeeApplicationService employeeApplicationService)
        {
            _employeeApplicationService = employeeApplicationService;

            _salesByEmployeeSource = new(x => x.ID);

            _salesByEmployeeSource
                .Connect()
                .Sort(SortExpressionComparer<SalesByEmployeeDto>.Descending(x => x.Sales))
                .Bind(out _salesByEmployee)
                .Subscribe();
        }

        public async Task OnNavigatedTo()
        {
            var salesByEmployees = await _employeeApplicationService.GetSalesByEmployees();

            _salesByEmployeeSource.AddOrUpdate(salesByEmployees);
        }

        public async Task UpdateStastics()
        {
            var newSalesByEmployee = await _employeeApplicationService.GetSalesByEmployees();

            foreach (var saleByEmployee in _salesByEmployeeSource.Items.ToList())
            {
                saleByEmployee.Sales = newSalesByEmployee.Single(x => x.ID == saleByEmployee.ID).Sales;
                _salesByEmployeeSource.Refresh(saleByEmployee);
            }
        }

        public ReadOnlyObservableCollection<SalesByEmployeeDto> SalesByEmployees => _salesByEmployee;
    }
}
