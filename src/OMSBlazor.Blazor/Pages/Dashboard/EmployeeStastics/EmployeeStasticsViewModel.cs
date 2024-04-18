using DynamicData;
using OMSBlazor.Application.Contracts.Interfaces;
using OMSBlazor.Dto.Employee.Stastics;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
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
                .Bind(out _salesByEmployee)
                .Subscribe();
        }

        public async Task OnNavigatedTo()
        {
            var salesByEmployees = await _employeeApplicationService.GetSalesByEmployees();

            _salesByEmployeeSource.AddOrUpdate(salesByEmployees);
        }

        public ReadOnlyObservableCollection<SalesByEmployeeDto> SalesByEmployees => _salesByEmployee;
    }
}
