using ClassIsland.Plugin.TrialHomework.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClassIsland.Plugin.TrialHomework.Services;

/// <summary>
/// 试写作业名单核心服务：根据日期与设置解析当天的试写作业人员名单。
/// </summary>
public class TrialDutyService : ObservableRecipient
{
    private static readonly char[] NameSeparators =
        { '、', '，', ',', ' ', '/', '；', ';', '\n', '\r', '\t', '　' };

    public PluginSettings Settings { get; }

    public TrialDutyService(PluginSettings settings)
    {
        Settings = settings;
    }

    /// <summary>
    /// 获取给定日期应使用的名单组。
    /// </summary>
    public TrialGroup? GetEffectiveGroup(DateTime date)
    {
        var groups = Settings.Groups;
        if (groups.Count == 0)
        {
            return null;
        }

        var index = Settings.AutoRotateByMonth
            ? ((date.Month - 1) % groups.Count + groups.Count) % groups.Count
            : ((Settings.ActiveGroupIndex % groups.Count) + groups.Count) % groups.Count;
        return groups[index];
    }

    /// <summary>
    /// 获取给定日期的试写作业人员名单。
    /// </summary>
    public IReadOnlyList<string> GetNamesFor(DateTime date)
    {
        var group = GetEffectiveGroup(date);
        if (group == null)
        {
            return Array.Empty<string>();
        }

        var raw = group[date.DayOfWeek];
        return raw.Split(NameSeparators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    /// <summary>
    /// 获取给定日期的名单文本，多个名字以顿号连接。
    /// </summary>
    public string GetNamesTextFor(DateTime date)
    {
        return string.Join("、", GetNamesFor(date));
    }
}
