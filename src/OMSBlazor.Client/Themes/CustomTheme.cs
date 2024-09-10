using MudBlazor;

namespace OMSBlazor.Client.Themes
{
    public static class MyCustomTheme
    {
        public static MudTheme MudTheme = new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = "#3459e6",
                Secondary = "#ffffff",
                SecondaryContrastText = "#000000",
                TableHover = "#EEEEEE", // Also applied to data grids
                AppbarBackground = "#3459e6",
            },
            PaletteDark = new PaletteDark()
            {
                Primary = "#3459e6",
                TableHover = "#9E9E9E", // Also applied to data grids
                Secondary = "#ffffff",
                SecondaryContrastText = "#000000",
                AppbarBackground = "#3459e6",
                AppbarText = "#FFFFFF"
            }
        };
    }
}
