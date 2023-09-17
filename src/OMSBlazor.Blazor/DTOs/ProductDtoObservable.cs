using OMSBlazor.Dto.Product;
using System.ComponentModel;

namespace OMSBlazor.Blazor.DTOs
{
    public class ProductDtoObservable : ProductDto, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public override int UnitsInStock 
        {
            get 
            { 
                return base.UnitsInStock; 
            }
            set 
            { 
                base.UnitsInStock = value; 
                NotifyPropertyChanged(nameof(UnitsInStock));
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged is not null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
