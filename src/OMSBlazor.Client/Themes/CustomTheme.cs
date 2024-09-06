using MudBlazor;

namespace OMSBlazor.Client.Themes
{
    public static class MyCustomTheme
    {
        public static MudTheme MudTheme = new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = "#78c2ad",
                Secondary = "#f3969a",
                TableHover = "#f3969a", // Also applied to data grids
                AppbarBackground = "#78c2ad",
            },
            PaletteDark = new PaletteDark()
            {
                Primary = "#78c2ad",
                TableHover = "#f3969a", // Also applied to data grids
                Secondary = "#f3969a",
            },
        };
    }
}
