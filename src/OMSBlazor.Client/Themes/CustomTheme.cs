using MudBlazor;

namespace OMSBlazor.Client.Themes
{
    public class CustomTheme : MudTheme
    {
        public CustomTheme()
        {
            LayoutProperties = new LayoutProperties()
            {
                DefaultBorderRadius = "3px"
            };

            PaletteDark = new PaletteDark() 
            { 
                
            };
        }
    }
}
