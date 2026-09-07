using Avalonia.Controls;
using Avalonia.Threading;
using ClassIsland.Core.Abstractions.Controls;
using ClassIsland.Core.Abstractions.Services;
using ClassIsland.Core.Attributes;
using ClassIsland.Plugin.TrialHomework.Models;
using ClassIsland.Plugin.TrialHomework.Services;
using ClassIsland.Shared.Enums;

namespace ClassIsland.Plugin.TrialHomework.Controls;

/// <summary>
/// 试写作业名单组件：课间显示当天试写作业人员，上课时自动隐藏，避免遮挡。
/// </summary>
[ComponentInfo("F3A7C2D1-8B4E-4F6A-9C2D-15E7B8A9C0D1", "试写作业名单", "\uE70F",
    "课间显示当天试写作业的同学，上课时自动隐藏。")]
public partial class TrialDutyComponent : ComponentBase<TrialDutyComponentSettings>
{
    private TrialDutyService DutyService { get; }
    private ILessonsService LessonsService { get; }

    private bool _settingsHooked;

    public TrialDutyComponent(TrialDutyService dutyService, ILessonsService lessonsService)
    {
        DutyService = dutyService;
        LessonsService = lessonsService;
        InitializeComponent();

        LessonsService.OnClass += OnLessonsStateChanged;
        LessonsService.OnBreakingTime += OnLessonsStateChanged;
        LessonsService.OnAfterSchool += OnLessonsStateChanged;
        LessonsService.CurrentTimeStateChanged += OnLessonsStateChanged;
        // 主计时器每秒刷新一次，同时覆盖日期变更与设置变更后的界面更新。
        LessonsService.PostMainTimerTicked += OnPostMainTimerTicked;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        HookSettings();
        Refresh();
    }

    private void OnLessonsStateChanged(object? sender, EventArgs e)
    {
        RefreshOnUiThread();
    }

    private void OnPostMainTimerTicked(object? sender, EventArgs e)
    {
        RefreshOnUiThread();
    }

    private void HookSettings()
    {
        if (_settingsHooked || Settings == null)
        {
            return;
        }

        Settings.PropertyChanged += (_, _) => RefreshOnUiThread();
        _settingsHooked = true;
    }

    private void RefreshOnUiThread()
    {
        Dispatcher.UIThread.Post(Refresh);
    }

    private void Refresh()
    {
        HookSettings();
        if (Settings == null)
        {
            return;
        }

        var now = DateTime.Now;
        var names = DutyService.GetNamesTextFor(now);
        TitleText.Text = Settings.Title;
        NamesText.Text = names;

        var visible = !(Settings.HideWhenEmpty && string.IsNullOrWhiteSpace(names));
        var state = LessonsService.CurrentState;
        if (Settings.ShowOnlyOnBreak && state == TimeState.OnClass)
        {
            visible = false;
        }

        if (state == TimeState.AfterSchool && !Settings.ShowOnAfterSchool)
        {
            visible = false;
        }

        RootCard.IsVisible = visible;
    }
}
