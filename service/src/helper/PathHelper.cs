namespace service;

public static class PathHelper {

    private const string DIRECTORY_APP_NAME = "NoiseWallpaperGenerator";
    private const string USER_SETTINGS_FILENAME = "settings.json";
    private const string WALLPAPER_FILENAME = "wallpaper.png";
    private const string LOG_FILENAME = "log.log";

    public static string ApplicationDirectoryFullPath() {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), DIRECTORY_APP_NAME);
    }

    public static string ApplicationSettingsFullPath() {
        string applicationDirectoryFullPath = ApplicationDirectoryFullPath();
        return Path.Combine(applicationDirectoryFullPath, USER_SETTINGS_FILENAME);
    }

    public static string WallpaperFullPath() {
        string applicationDirectoryFullPath = ApplicationDirectoryFullPath();
        return Path.Combine(applicationDirectoryFullPath, WALLPAPER_FILENAME);
    }

    public static string LogFullPath() {
        string applicationDirectoryFullPath = ApplicationDirectoryFullPath();
        return Path.Combine(applicationDirectoryFullPath, LOG_FILENAME);
    }
}