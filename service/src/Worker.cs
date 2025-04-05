using System.Text;
using System.Text.Json;

namespace service;

public class Worker : BackgroundService, IDisposable
{
    private readonly ILogger<Worker> _logger;
    private readonly HttpClient _httpClient;
    private FileSystemWatcher _configurationFileSystemWatcher = new FileSystemWatcher();
    private Settings _settings;

    private const int MAX_FETCH_WALLPAPER_ATTEMPT = 3;
    private const int TIME_BEFORE_RETRY_SEC = 10;

    public Worker(ILogger<Worker> logger, Settings settings)
    {
        _logger = logger;
        _settings = settings;
        _httpClient = new HttpClient();

        string settingsFullPath = PathHelper.ApplicationSettingsFullPath();
        WatchFile(settingsFullPath);
    }

    private void WatchFile(string path) {
        _configurationFileSystemWatcher = new FileSystemWatcher
        {
            Path = Path.GetDirectoryName(path),
            Filter = Path.GetFileName(path),
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
            EnableRaisingEvents = true
        };

        _configurationFileSystemWatcher.Created += OnSettingsFileCreated;
        _configurationFileSystemWatcher.Changed += OnSettingsFileChanged;
    }

    private void OnSettingsFileCreated(object sender, FileSystemEventArgs e)
    {
        try {
            _settings = Settings.ReadSettingsFromFile(e.FullPath);
        } catch (Exception ex) {
            _logger.LogError($"Failed to read settings after the file was created: {ex}");
        }
    }

    private void OnSettingsFileChanged(object sender, FileSystemEventArgs e)
    {
        try {
            _settings = Settings.ReadSettingsFromFile(e.FullPath);
        } catch (Exception ex) {
            _logger.LogError($"Failed to read settings after the file was modified: {ex}");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try {
                byte[] image = await FetchWallpaper(stoppingToken);

                string wallpaperFullPath = PathHelper.WallpaperFullPath();
                bool success = await WallpaperHelper.SetWallpaper(image, wallpaperFullPath);

                if(success) {
                    _logger.LogInformation($"Wallpaper applied.");
                } else {
                    _logger.LogInformation($"Unable to apply wallpaper.");
                }

            } catch(Exception e) {
                _logger.LogError($"Unexpected error: {e}");
            } finally {
                await Task.Delay(_settings.GENERATION_INTERVAL_MS, stoppingToken);
            }
        }
    }

    private async Task<byte[]> FetchWallpaper(CancellationToken stoppingToken) {
        StringContent jsonContent = new StringContent(JsonSerializer.Serialize(_settings.wallpaperConfiguration), Encoding.UTF8, "application/json");

        for(int attempt = 0; attempt < MAX_FETCH_WALLPAPER_ATTEMPT; attempt++) {
            try {
                HttpResponseMessage response = await _httpClient.PostAsync(_settings.API_URL, jsonContent, stoppingToken);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                } else {
                    _logger.LogError($"HTTP error, failed to fetch. {response.StatusCode}");
                }
            } catch(HttpRequestException) {
                _logger.LogError("Unable to reach the API.");
            } finally {
                _logger.LogInformation($"Retrying... ({attempt + 1}/{MAX_FETCH_WALLPAPER_ATTEMPT})");
                await Task.Delay(TimeSpan.FromSeconds(TIME_BEFORE_RETRY_SEC), stoppingToken);
            }
        }

        return [];
    }

    public override void Dispose()
    {
        _configurationFileSystemWatcher.Created -= OnSettingsFileCreated;
        _configurationFileSystemWatcher.Changed -= OnSettingsFileChanged;

        base.Dispose();
    }
}
