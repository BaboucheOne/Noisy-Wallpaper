using service;

string applicationDirectoryFullPath = PathHelper.ApplicationDirectoryFullPath();
if(!Directory.Exists(applicationDirectoryFullPath)) {
    Directory.CreateDirectory(applicationDirectoryFullPath);
}

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddSingleton(provider => {
    SettingsFactory settingsFactory = new SettingsFactory();
    return settingsFactory.LoadOrCreateDefaultSettings();
});

builder.Services.AddHostedService<Worker>();

IHost host = builder.Build();
host.Run();
