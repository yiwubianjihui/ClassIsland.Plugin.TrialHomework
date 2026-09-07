using System.IO;
using ClassIsland.Core.Abstractions;
using ClassIsland.Core.Attributes;
using ClassIsland.Core.Extensions.Registry;
using ClassIsland.Plugin.TrialHomework.Controls;
using ClassIsland.Plugin.TrialHomework.Models;
using ClassIsland.Plugin.TrialHomework.Services;
using ClassIsland.Plugin.TrialHomework.Views.SettingsPages;
using ClassIsland.Shared.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClassIsland.Plugin.TrialHomework;

[PluginEntrance]
public class Plugin : PluginBase
{
    public PluginSettings Settings { get; private set; } = new();

    public override void Initialize(HostBuilderContext context, IServiceCollection services)
    {
        var configPath = Path.Combine(PluginConfigFolder, "Settings.json");
        Settings = ConfigureFileHelper.LoadConfig<PluginSettings>(configPath);
        Settings.PropertyChanged += (_, _) => ConfigureFileHelper.SaveConfig(configPath, Settings);

        services.AddSingleton(Settings);
        services.AddSingleton<TrialDutyService>();
        services.AddSettingsPage<TrialDutySettingsPage>();
        services.AddComponent<TrialDutyComponent, TrialDutyComponentSettingsControl>();
        services.AddNotificationProvider<TrialDutyReminderProvider>();
    }
}
