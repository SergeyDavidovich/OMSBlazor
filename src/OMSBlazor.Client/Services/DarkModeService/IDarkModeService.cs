namespace OMSBlazor.Client.Services.DarkModeService
{
    public interface IDarkModeService
    {
        public Task SetIsDarkMode(bool isDarkMode);

        public Task<bool> GetIsDarkMode();
    }
}
