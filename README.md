# ClassIsland.Plugin.TrialHomework · 试写作业助手

一款 [ClassIsland](https://github.com/ClassIsland/ClassIsland) 插件，用于展示与提醒**每天试写作业的同学**。

> 试写作业是学校为控制作业量开展的活动：每天挑选 3~4 位同学，在下午的一节课上试写当天布置的全部作业。名单每周确认一整周、每月轮换一组。

## ✨ 功能

| 功能 | 说明 |
| --- | --- |
| 📋 名单组件 | 在主界面添加「试写作业名单」组件，**课间显示**当天试写人员，**上课自动隐藏**，不遮挡课表 |
| ⏰ 下午提醒 | 到达设定时间（默认 14:20）后，以课堂提醒方式弹出当天名单，适合在试写课开始前提醒 |
| 🔄 按月轮换 | 支持多个名单组：1 月第 1 组、2 月第 2 组……自动循环，也可手动固定使用某一组 |
| 📝 简单录入 | 每周七天各一个输入框，名字之间用空格 / 顿号 / 逗号分隔即可 |
| 🔔 测试提醒 | 设置页一键「测试提醒」，无需等到第二天 |

## 📦 安装

1. 前往 [Releases](https://github.com/yiwubianjihui/ClassIsland.Plugin.TrialHomework/releases) 下载最新的 `.cipx` 插件包；
2. 打开 ClassIsland → 设置 → 插件 → 安装插件，选择下载的 `.cipx` 文件；
3. 安装后重启 ClassIsland。

> 也可以在 [Actions](https://github.com/yiwubianjihui/ClassIsland.Plugin.TrialHomework/actions) 页面下载最新构建产物。

## 🚀 使用

1. **填写名单**：打开 设置 → 试写作业助手，在「编辑名单」里按星期填写每天的人员（多个名字用空格、顿号或逗号分隔）；
2. **添加组件**：进入主界面编辑模式 → 添加组件 → 「试写作业名单」，课间即会显示当天名单，上课时自动隐藏（可在组件设置中调整）；
3. **下午提醒**：在设置页开启「启用下午试写提醒」并设定时间（默认 14:20），每天到达时间后会弹出提醒；提醒文本中的 `{names}` 会替换为当天名单；
4. **月度轮换**：默认开启「按月轮换」，1 月用第 1 组、2 月用第 2 组……无需手动切换。

### 注意事项

- 下午提醒需要 ClassIsland 处于运行状态；如果应用在提醒时间之后才启动，**30 分钟内仍会补发**一次提醒；
- 提醒时间格式为 `HH:mm`（如 `14:20`）。

## 🛠️ 从源码构建

```bash
git clone https://github.com/yiwubianjihui/ClassIsland.Plugin.TrialHomework.git
cd ClassIsland.Plugin.TrialHomework
dotnet build -c Release
```

构建产物为项目目录下的 `cipx/ClassIsland.Plugin.TrialHomework.cipx`，可直接在 ClassIsland 中安装。

- 目标框架：.NET 8（跟随官方 ExamplePlugin）
- 插件 SDK：`ClassIsland.PluginSdk 2.0.0.*`
- 面向版本：ClassIsland 2.0 及以上（`apiVersion: 2.0.0.0`）

## 📁 数据与配置

插件配置保存在 ClassIsland 配置目录下的 `Plugins/Config/yibianhui.trialhomework/Settings.json`，卸载插件前备份该文件即可保留名单。

## 📄 许可

[MIT License](LICENSE)
