using System.Text.Json;

namespace service;

[Serializable]
public class Settings {
    public string API_URL { get; set; } = "http://localhost:8080/images";
    public int GENERATION_INTERVAL_MS { get; set; } = 60 * 60 * 1000;
    public WallpaperSettings wallpaperConfiguration  { get; set; } = new WallpaperSettings();

    public static Settings ReadSettingsFromFile(string path) {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<Settings>(json);
    }
}