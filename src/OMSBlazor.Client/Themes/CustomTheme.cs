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
                Info = "#d4e5f0",
                InfoContrastText= "#103148"
            },
            PaletteDark = new PaletteDark()
            {
                Primary = "#3459e6",
                TableHover = "#9E9E9E", // Also applied to data grids
                Secondary = "#ffffff",
                SecondaryContrastText = "#000000",
                AppbarBackground = "#3459e6",
                Info= "#081924",
                InfoContrastText= "#7eb0d3",
                AppbarText = "#FFFFFF"
            }
        };
    }
}
