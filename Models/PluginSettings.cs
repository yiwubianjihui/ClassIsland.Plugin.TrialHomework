using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClassIsland.Plugin.TrialHomework.Models;

/// <summary>
/// 插件全局设置。
/// </summary>
public class PluginSettings : ObservableRecipient
{
    private ObservableCollection<TrialGroup> _groups = new()
    {
        new TrialGroup
        {
            Name = "第 1 组",
            Monday = "示例同学A、示例同学B",
            Tuesday = "示例同学C、示例同学D",
            Wednesday = "示例同学E、示例同学F",
            Thursday = "示例同学G、示例同学H",
            Friday = "示例同学I、示例同学J"
        },
        new TrialGroup
        {
            Name = "第 2 组"
        }
    };

    private int _activeGroupIndex;
    private bool _autoRotateByMonth = true;
    private bool _enableReminder = true;
    private string _reminderTime = "14:20";
    private bool _reminderOnSchoolDaysOnly = true;
    private string _reminderMessage = "今天试写作业的同学：{names}\n请带好今天的作业，提前前往试写位置。";

    public ObservableCollection<TrialGroup> Groups
    {
        get => _groups;
        set
        {
            if (Equals(value, _groups)) return;
            _groups = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 手动选中的名单组下标。仅在关闭“按月轮换”时使用。
    /// </summary>
    public int ActiveGroupIndex
    {
        get => _activeGroupIndex;
        set
        {
            if (value == _activeGroupIndex) return;
            _activeGroupIndex = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 是否按月份自动轮换名单组：1 月使用第 1 组，2 月使用第 2 组……循环。
    /// </summary>
    public bool AutoRotateByMonth
    {
        get => _autoRotateByMonth;
        set
        {
            if (value == _autoRotateByMonth) return;
            _autoRotateByMonth = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 是否启用下午试写提醒。
    /// </summary>
    public bool EnableReminder
    {
        get => _enableReminder;
        set
        {
            if (value == _enableReminder) return;
            _enableReminder = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 提醒时间，格式 HH:mm，例如 14:20。
    /// </summary>
    public string ReminderTime
    {
        get => _reminderTime;
        set
        {
            if (value == _reminderTime) return;
            _reminderTime = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 是否仅在周一至周五提醒。
    /// </summary>
    public bool ReminderOnSchoolDaysOnly
    {
        get => _reminderOnSchoolDaysOnly;
        set
        {
            if (value == _reminderOnSchoolDaysOnly) return;
            _reminderOnSchoolDaysOnly = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 提醒文本模板，{names} 会替换为当天名单。
    /// </summary>
    public string ReminderMessage
    {
        get => _reminderMessage;
        set
        {
            if (value == _reminderMessage) return;
            _reminderMessage = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 当前手动选中的名单组。
    /// </summary>
    [JsonIgnore]
    public TrialGroup? ActiveGroup => Groups.Count == 0
        ? null
        : Groups[((ActiveGroupIndex % Groups.Count) + Groups.Count) % Groups.Count];
}
