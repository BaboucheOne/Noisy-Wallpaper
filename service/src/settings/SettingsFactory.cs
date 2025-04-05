using System.Text.Json;
using service;

public class SettingsFactory {
    public Settings LoadOrCreateDefaultSettings() {
        string settingsFullPath = PathHelper.ApplicationSettingsFullPath();

        try {
            return Settings.ReadSettingsFromFile(settingsFullPath);
        } catch(FileNotFoundException) {
            Settings defaultSettings = new Settings();
            string json = JsonSerializer.Serialize(defaultSettings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(settingsFullPath, json);

            return defaultSettings;
        }
    }
}