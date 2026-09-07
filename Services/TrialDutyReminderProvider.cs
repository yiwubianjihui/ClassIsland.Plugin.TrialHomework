using System.Globalization;
using Avalonia.Threading;
using ClassIsland.Core.Abstractions.Services.NotificationProviders;
using ClassIsland.Core.Attributes;
using ClassIsland.Core.Models.Notification;
using ClassIsland.Plugin.TrialHomework.Models;
using Microsoft.Extensions.Logging;

namespace ClassIsland.Plugin.TrialHomework.Services;

/// <summary>
/// 试写作业提醒提供方：在设定时间到达后，以课堂提醒的形式弹出当天试写名单。
/// </summary>
[NotificationProviderInfo("D94E6C5A-3F2B-4E81-A7C9-6B0D8E2F4A17", "试写作业提醒", "\uE823",
    "在设定的时间提醒当天试写作业的同学，适合放在下午试写课开始前。")]
public class TrialDutyReminderProvider : NotificationProviderBase
{
    /// <summary>
    /// 错过提醒时间的补发窗口。例如应用在提醒时间之后才启动时，在此窗口内仍然会补发提醒。
    /// </summary>
    private static readonly TimeSpan MakeupWindow = TimeSpan.FromMinutes(30);

    private ILogger<TrialDutyReminderProvider> Logger { get; }
    private PluginSettings Settings { get; }
    private TrialDutyService DutyService { get; }

    private DispatcherTimer? _timer;
    private DateTime _lastFiredDate = DateTime.MinValue;

    public TrialDutyReminderProvider(PluginSettings settings, TrialDutyService dutyService,
        ILogger<TrialDutyReminderProvider> logger)
    {
        Settings = settings;
        DutyService = dutyService;
        Logger = logger;
    }

    public new Task StartAsync(CancellationToken cancellationToken)
    {
        Dispatcher.UIThread.Post(StartTimer);
        return Task.CompletedTask;
    }

    public new Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Stop();
        return Task.CompletedTask;
    }

    private void StartTimer()
    {
        if (_timer != null)
        {
            return;
        }

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += (_, _) => OnTick();
        _timer.Start();
        Logger.LogInformation("试写作业提醒计时器已启动。");
    }

    private void OnTick()
    {
        try
        {
            var now = DateTime.Now;
            if (!Settings.EnableReminder || _lastFiredDate.Date == now.Date)
            {
                return;
            }

            if (Settings.ReminderOnSchoolDaysOnly && now.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            {
                return;
            }

            var raw = Settings.ReminderTime?.Trim().Replace('：', ':') ?? "";
            if (!TimeSpan.TryParse(raw, CultureInfo.InvariantCulture, out var timeOfDay))
            {
                return;
            }

            var elapsed = now - (now.Date + timeOfDay);
            if (elapsed < TimeSpan.Zero || elapsed > MakeupWindow)
            {
                return;
            }

            _lastFiredDate = now.Date;
            ShowReminder(false);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "触发试写作业提醒时出现异常。");
        }
    }

    /// <summary>
    /// 立即显示一次试写作业提醒。
    /// </summary>
    /// <param name="isTest">是否为测试提醒。</param>
    public void ShowReminder(bool isTest)
    {
        var names = DutyService.GetNamesTextFor(DateTime.Now);
        if (string.IsNullOrWhiteSpace(names))
        {
            Logger.LogInformation("今天没有试写作业名单，跳过{}提醒。", isTest ? "测试" : "");
            return;
        }

        var text = (Settings.ReminderMessage ?? "").Replace("{names}", names);
        ShowNotification(new NotificationRequest
        {
            MaskContent = NotificationContent.CreateTwoIconsMask(isTest ? "试写作业（测试）" : "试写作业",
                rightIcon: "\uE70F"),
            OverlayContent = NotificationContent.CreateSimpleTextContent(text,
                x => x.Duration = TimeSpan.FromSeconds(isTest ? 15 : 30))
        });
    }
}
