using service;

string applicationDirectoryFullPath = PathHelper.ApplicationDirectoryFullPath();
if (!Directory.Exists(applicationDirectoryFullPath)) {
    Directory.CreateDirectory(applicationDirectoryFullPath);
}

SettingsFactory settingsFactory = new SettingsFactory();

IHostBuilder builder = Host.CreateDefaultBuilder(args);

builder.ConfigureLogging((hostingContext, config) => { 
    config.AddProvider(new FileLoggerProvider(PathHelper.LogFullPath()));
});

builder.ConfigureServices((hostContext, services) => {

    Settings settings = settingsFactory.LoadOrCreateDefaultSettings();

    services.AddSingleton(settings);

    services.AddHostedService<Worker>();
});

if(OperatingSystem.IsWindows()) {
    builder.UseWindowsService();
} else if(OperatingSystem.IsLinux()) {
    builder.UseSystemd();
}


IHost host = builder.Build();
host.Run();
