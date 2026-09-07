using CommunityToolkit.Mvvm.ComponentModel;

namespace ClassIsland.Plugin.TrialHomework.Models;

/// <summary>
/// 试写作业名单组件的设置。
/// </summary>
public class TrialDutyComponentSettings : ObservableRecipient
{
    private string _title = "今日试写作业";
    private bool _showOnlyOnBreak = true;
    private bool _showOnAfterSchool;
    private bool _hideWhenEmpty = true;

    /// <summary>
    /// 组件标题。
    /// </summary>
    public string Title
    {
        get => _title;
        set
        {
            if (value == _title) return;
            _title = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 仅课间显示：上课时自动隐藏，避免遮挡。
    /// </summary>
    public bool ShowOnlyOnBreak
    {
        get => _showOnlyOnBreak;
        set
        {
            if (value == _showOnlyOnBreak) return;
            _showOnlyOnBreak = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 放学后是否继续显示。
    /// </summary>
    public bool ShowOnAfterSchool
    {
        get => _showOnAfterSchool;
        set
        {
            if (value == _showOnAfterSchool) return;
            _showOnAfterSchool = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 当天没有名单时隐藏组件。
    /// </summary>
    public bool HideWhenEmpty
    {
        get => _hideWhenEmpty;
        set
        {
            if (value == _hideWhenEmpty) return;
            _hideWhenEmpty = value;
            OnPropertyChanged();
        }
    }
}
