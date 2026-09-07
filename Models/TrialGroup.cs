using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClassIsland.Plugin.TrialHomework.Models;

/// <summary>
/// 一个名单组：包含一周七天的试写作业人员名单。
/// 名单以文本形式保存，多个名字之间使用空格、顿号、逗号等分隔。
/// </summary>
public class TrialGroup : ObservableRecipient
{
    private string _name = "新组";
    private string _monday = "";
    private string _tuesday = "";
    private string _wednesday = "";
    private string _thursday = "";
    private string _friday = "";
    private string _saturday = "";
    private string _sunday = "";

    public string Name
    {
        get => _name;
        set
        {
            if (value == _name) return;
            _name = value;
            OnPropertyChanged();
        }
    }

    public string Monday
    {
        get => _monday;
        set
        {
            if (value == _monday) return;
            _monday = value;
            OnPropertyChanged();
        }
    }

    public string Tuesday
    {
        get => _tuesday;
        set
        {
            if (value == _tuesday) return;
            _tuesday = value;
            OnPropertyChanged();
        }
    }

    public string Wednesday
    {
        get => _wednesday;
        set
        {
            if (value == _wednesday) return;
            _wednesday = value;
            OnPropertyChanged();
        }
    }

    public string Thursday
    {
        get => _thursday;
        set
        {
            if (value == _thursday) return;
            _thursday = value;
            OnPropertyChanged();
        }
    }

    public string Friday
    {
        get => _friday;
        set
        {
            if (value == _friday) return;
            _friday = value;
            OnPropertyChanged();
        }
    }

    public string Saturday
    {
        get => _saturday;
        set
        {
            if (value == _saturday) return;
            _saturday = value;
            OnPropertyChanged();
        }
    }

    public string Sunday
    {
        get => _sunday;
        set
        {
            if (value == _sunday) return;
            _sunday = value;
            OnPropertyChanged();
        }
    }

    public string this[DayOfWeek day] => day switch
    {
        DayOfWeek.Monday => Monday,
        DayOfWeek.Tuesday => Tuesday,
        DayOfWeek.Wednesday => Wednesday,
        DayOfWeek.Thursday => Thursday,
        DayOfWeek.Friday => Friday,
        DayOfWeek.Saturday => Saturday,
        _ => Sunday
    };
}
