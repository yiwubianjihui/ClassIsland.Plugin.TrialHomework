using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassIsland.Core.Abstractions.Controls;
using ClassIsland.Core.Attributes;
using ClassIsland.Plugin.TrialHomework.Models;
using ClassIsland.Plugin.TrialHomework.Services;
using ClassIsland.Shared;
using Microsoft.Extensions.Hosting;

namespace ClassIsland.Plugin.TrialHomework.Views.SettingsPages;

/// <summary>
/// 试写作业助手设置页面。
/// </summary>
[SettingsPageInfo("yibianhui.trialhomework", "试写作业助手")]
public partial class TrialDutySettingsPage : SettingsPageBase, INotifyPropertyChanged
{
    private PluginSettings Settings { get; }
    private TrialDutyService DutyService { get; }

    private TrialGroup? _effectiveGroup;
    private string _todayNamesText = "";

    public new event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<TrialGroup> Groups => Settings.Groups;

    public TrialGroup? EffectiveGroup
    {
        get => _effectiveGroup;
        set
        {
            if (Equals(value, _effectiveGroup)) return;
            _effectiveGroup = value;
            OnPropertyChanged();
        }
    }

    public string TodayNamesText
    {
        get => _todayNamesText;
        set
        {
            if (value == _todayNamesText) return;
            _todayNamesText = value;
            OnPropertyChanged();
        }
    }

    public TrialDutySettingsPage(TrialDutyService dutyService)
    {
        DutyService = dutyService;
        Settings = dutyService.Settings;
        InitializeComponent();
        DataContext = this;
        Refresh();
    }

    private void Refresh()
    {
        EffectiveGroup = DutyService.GetEffectiveGroup(DateTime.Now);
        var names = DutyService.GetNamesTextFor(DateTime.Now);
        TodayNamesText = string.IsNullOrWhiteSpace(names) ? "（今天还没有名单）" : names;
    }

    private void ComboBoxGroup_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ComboBoxGroup.SelectedItem is TrialGroup group && Settings.Groups.Contains(group))
        {
            Settings.ActiveGroupIndex = Settings.Groups.IndexOf(group);
        }

        Refresh();
    }

    private void ButtonAddGroup_OnClick(object? sender, RoutedEventArgs e)
    {
        Settings.Groups.Add(new TrialGroup
        {
            Name = $"第 {Settings.Groups.Count + 1} 组"
        });
        Refresh();
    }

    private void ButtonRemoveGroup_OnClick(object? sender, RoutedEventArgs e)
    {
        if (Settings.Groups.Count <= 1 || ComboBoxGroup.SelectedItem is not TrialGroup group)
        {
            return;
        }

        Settings.Groups.Remove(group);
        Refresh();
    }

    private void ButtonTestReminder_OnClick(object? sender, RoutedEventArgs e)
    {
        // 注意：AddNotificationProvider<T> 只把提醒提供方注册为 IHostedService，
        // 并未注册为 DI 服务 T 本身；直接 IAppHost.GetService<TrialDutyReminderProvider>()
        // 会抛 ArgumentException（并导致插件被判定异常而自动禁用）。
        var provider = IAppHost.TryGetService<IEnumerable<IHostedService>>()
            ?.OfType<TrialDutyReminderProvider>()
            .FirstOrDefault();
        provider?.ShowReminder(true);
    }

    private void ButtonRefresh_OnClick(object? sender, RoutedEventArgs e)
    {
        Refresh();
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
