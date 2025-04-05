namespace service;

[Serializable]
public class WallpaperSettings {
    public int Width { get; set; } = 1920;
    public int Height { get; set; } = 1080;
    public int Scale { get; set; } = 300;
    public int Octaves { get; set; } = 4;
    public int NumberOfPoints { get; set; } = 50;
    public float BlendFactor { get; set; } = 1.25f;
    public float Frequency { get; set; } = 1.0f;
    public float Amplitude { get; set; } = 2f;
    public string[] Colors { get; set; } = ["#348888", "#22BABB", "#9EF8EE", "#FA7F08", "#F24405"];

    public WallpaperSettings()
    {
    }
}