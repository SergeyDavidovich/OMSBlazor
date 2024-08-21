namespace OMSBlazor.Services.DarkModeService
{
    public static class DarkModeState
    {
        private static bool _isDarkMode;

        public static void SetIsDarkMode(bool isDarkMode)
        {
            _isDarkMode = isDarkMode;
        }

        public static bool GetIsDarkMode()
        {
            return _isDarkMode;
        }
    }
}
