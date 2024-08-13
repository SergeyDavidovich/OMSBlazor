using OMSBlazor.Client.Services;

namespace OMSBlazor.Services
{
    public class ServerDarkModeService : IDarkModeService
    {
        public async Task<bool> GetIsDarkMode()
        {
            await Task.CompletedTask;

            return DarkModeState.GetIsDarkMode();
        }

        public async Task SetIsDarkMode(bool isDarkMode)
        {
            await Task.CompletedTask;

            DarkModeState.SetIsDarkMode(isDarkMode);
        }
    }
}
