namespace OMSBlazor.Client.Services
{
    public interface IDarkModeService
    {
        public Task SetIsDarkMode(bool isDarkMode);

        public Task<bool> GetIsDarkMode();
    }
}
