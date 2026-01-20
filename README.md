# STranslate Avalonia - Linux 版本

> 将 WPF 版 STranslate 迁移到 Avalonia UI，实现 Linux 原生支持

## 项目状态

- ✅ **项目结构**: 已完成
- ✅ **核心逻辑**: 已迁移 (Core/Services/Helpers)
- ✅ **基础 UI**: 已完成
- 🚧 **Linux 特性**: 进行中 (全局快捷键、截图、托盘)
- ⏳ **构建测试**: 等待 .NET SDK 安装

## 前置要求

### 1. 安装 .NET SDK 8.0

```bash
# Fedora 43
sudo dnf install -y dotnet-sdk-8.0
```

### 2. 安装系统依赖

```bash
# 截图工具
sudo dnf install -y gnome-screenshot scrot

# 全局快捷键依赖
sudo dnf install -y libevdev-devel

# 音频支持 (TTS)
sudo dnf install -y portaudio-devel
```

## 构建说明

### 开发模式运行

```bash
cd /home/yyt/Downloads/STranslate.Avalonia
dotnet restore
dotnet build
dotnet run
```

### 发布 Linux 版本

```bash
# 发布为自包含应用 (包含 .NET Runtime)
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true

# 输出目录
# bin/Release/net8.0/linux-x64/publish/STranslate.Avalonia
```

### 安装到系统

```bash
# 复制到 /opt
sudo mkdir -p /opt/STranslate.Avalonia
sudo cp -r bin/Release/net8.0/linux-x64/publish/* /opt/STranslate.Avalonia/

# 安装 desktop 文件
sudo cp stranslate.desktop /usr/share/applications/

# 授予执行权限
sudo chmod +x /opt/STranslate.Avalonia/STranslate.Avalonia
```

## 功能实现

### ✅ 已实现

- [x] 基础翻译界面
- [x] MVVM 架构
- [x] ReactiveUI 集成
- [x] 核心业务逻辑 (完全复用)
- [x] Linux 全局快捷键 (SharpHook)
- [x] Linux 截图服务 (gnome-screenshot/scrot)
- [x] 剪贴板服务

### 🚧 进行中

- [ ] 托盘图标
- [ ] 完整 UI 界面 (Controls/Pages)
- [ ] OCR 集成
- [ ] TTS 语音朗读
- [ ] 主题系统

### ⏳ 计划中

- [ ] 配置文件迁移
- [ ] 数据库迁移
- [ ] 插件系统
- [ ] 云同步

---

## 📋 完整迁移TODO清单

基于原WPF版STranslate的完整迁移计划，详细追踪每个组件的迁移进度。

### 阶段一：分析与规划 ✅

- [x] 分析原始WPF项目结构
- [x] 识别所有主要界面（Views）
- [x] 识别所有自定义控件（Controls）
- [x] 梳理核心功能模块
- [x] 制定迁移实施计划

### 阶段二：核心界面迁移 🚧

#### 主窗口

- [x] MainWindow.xaml → MainWindow.axaml (基础布局)
- [x] 翻译输入/输出界面
- [ ] 历史记录界面
- [ ] 服务面板完整集成

#### 附加窗口

- [ ] SettingsWindow - 设置窗口
- [ ] ImageTranslateWindow - 图片翻译窗口
- [ ] OcrWindow - OCR窗口
- [ ] PromptEditWindow - 提示词编辑窗口
- [ ] VocabularyWindow - 单词本窗口
- [ ] AboutWindow - 关于窗口

### 阶段三：自定义控件迁移 🚧

- [x] **IconButton** - 图标按钮
- [x] **HeaderControl** - 头部工具栏
- [x] **InputControl** - 输入控件
- [x] **OutputControl** - 输出控件 (简化版)
- [ ] **HistoryControl** - 历史记录控件
- [ ] **ServicePanel** - 服务面板
- [ ] **HotkeyControl** - 快捷键控件
- [ ] **EditableTextBlock** - 可编辑文本块
- [ ] **ImageZoom** - 图片缩放控件
- [ ] **WordExchange** - 词汇交换控件
- [ ] **SnackbarContainer** - 提示条容器
- [ ] **Badge/STranslateBadge** - 徽章控件
- [ ] **CardGroup** - 卡片组

### 阶段四：核心服务实现 🚧

- [x] **翻译服务** (SimplifiedTranslateService - Google API)
  - [x] 基础翻译功能
  - [ ] 完整Plugin架构
  - [ ] 多翻译引擎支持
- [ ] **OCR服务**
  - [ ] Tesseract集成
  - [ ] 截图区域识别
- [ ] **TTS语音服务**
  - [ ] 多语言语音支持
  - [ ] 语音播放控制
- [ ] **托盘图标服务**
  - [ ] 托盘图标显示
  - [ ] 右键菜单
  - [ ] 常驻后台
- [ ] **主题系统**
  - [ ] 亮色/暗色主题
  - [ ] 主题切换动画
  - [ ] 自定义主题
- [ ] **配置管理**
  - [ ] 配置文件读写
  - [ ] 设置界面
  - [ ] 导入/导出配置
- [ ] **数据库服务**
  - [ ] SQLite集成
  - [ ] 历史记录存储
  - [ ] 单词本管理
- [ ] **WebDAV云同步**
  - [ ] 配置同步
  - [ ] 单词本同步
  - [ ] 冲突解决

### 阶段五：Linux特定功能 🚧

- [x] **全局快捷键** (SharpHook)
  - [x] 基础快捷键监听
  - [ ] 快捷键冲突检测
  - [ ] 自定义快捷键设置
- [x] **截图服务** (gnome-screenshot)
  - [x] 全屏截图
  - [x] 区域截图
  - [ ] 截图编辑
- [x] **剪贴板服务**
  - [x] 剪贴板监听
  - [ ] 剪贴板历史
- [ ] **托盘图标优化**
  - [ ] 适配不同Linux桌面环境
  - [ ] StatusNotifierItem支持
- [ ] **系统集成**
  - [ ] Desktop Entry文件
  - [ ] MIME类型关联
  - [ ] DBus集成

### 阶段六：Plugin系统迁移 ⏳

- [ ] **Plugin接口定义**
  - [ ] ITranslator接口
  - [ ] ISTT接口 (语音转文本)
  - [ ] ITTS接口 (文本转语音)
  - [ ] IOCR接口 (光学字符识别)
- [ ] **翻译引擎Plugin**
  - [ ] Google Translate
  - [ ] DeepL
  - [ ] 百度翻译
  - [ ] 有道翻译
  - [ ] ChatGPT
  - [ ] Gemini
  - [ ] OpenAI
- [ ] **Plugin加载机制**
  - [ ] 插件发现
  - [ ] 动态加载
  - [ ] 插件管理界面

### 阶段七：数据迁移 ⏳

- [ ] **配置迁移**
  - [ ] WPF配置文件解析
  - [ ] 转换为Avalonia格式
  - [ ] 迁移向导
- [ ] **数据库迁移**
  - [ ] 历史记录迁移
  - [ ] 单词本迁移
  - [ ] 自定义提示词迁移

### 阶段八：测试与验证 ⏳

- [ ] **功能测试**
  - [x] 基础翻译功能
  - [ ] 所有UI控件交互
  - [ ] 快捷键功能
  - [ ] Plugin系统
- [ ] **UI/UX测试**
  - [ ] 响应式布局
  - [ ] 主题切换
  - [ ] 动画效果
- [ ] **性能测试**
  - [ ] 翻译速度
  - [ ] 内存占用
  - [ ] 启动速度
- [ ] **打包测试**
  - [ ] AppImage打包
  - [ ] Flatpak打包
  - [ ] 系统依赖验证
- [ ] **部署验证**
  - [ ] Fedora 43测试
  - [ ] Ubuntu测试
  - [ ] Arch Linux测试

### 进度统计

- **总体进度**: ~25%
- **UI界面**: 30% (4/12 控件完成)
- **核心服务**: 15% (1/8 服务完成)
- **Linux功能**: 40% (快捷键、截图、剪贴板基本完成)
- **Plugin系统**: 0% (未开始)

---

## 核心UI控件实现总结 (2026-01-20)

### ✅ 已完成控件 (4/12)

#### 1. **IconButton** - 图标按钮

- 自定义图标大小控制
- 悬停效果
- 命令绑定支持
- 用途：工具栏按钮（复制、播放、设置等）

#### 2. **HeaderControl** - 顶部工具栏

- **10个功能按钮**：
  - 窗口置顶 (IsTopmost)
  - 设置面板 (SettingCommand)
  - 历史记录导航 (Previous/Next)
  - 主题切换 (ColorScheme)
  - 截图翻译 (Screenshot)
  - OCR识别
  - 自动翻译开关
  - 隐藏输入框
  - 鼠标划词钩子
- 完整命令绑定架构

#### 3. **InputControl** - 翻译输入控件

- **核心功能**：
  - ✅ Ctrl+滚轮调节字体大小
  - ✅ Enter快捷键执行翻译
  - ✅ 语言识别状态显示
  - ✅ 工具栏按钮（8个）：
    - 保存到单词本
    - 播放语音
    - 复制文本
    - 删除换行符
    - 删除空格
  - ✅ 实时字符计数
  - ✅ 翻译粘贴支持

#### 4. **OutputControl** - 翻译结果显示

- **简化版实现**（后续完善）：
  - ✅ 翻译结果文本框
  - ✅ 简洁的卡片布局
  - ✅ 复制和语音播放按钮
  - ⏳ 词典模式（音标、词性、例句）
  - ⏳ 多服务结果展示
  - ⏳ PascalCase/camelCase/snake_case转换

### 🎨 UI集成状态

- ✅ 所有控件已注册样式到 `App.axaml`
- ✅ MainWindow 已集成所有核心控件
- ✅ 完整的翻译界面布局
- ✅ 翻译按钮和结果显示绑定

### 🔨 技术实现亮点

- **StyledProperty替代DependencyProperty**：完全适配Avalonia属性系统
- **ReactiveUI延迟初始化**：避免UI线程问题
- **命令模式**：18个命令全部就绪（TranslateCommand、CopyCommand等）
- **模块化设计**：每个控件独立、可复用

### 📋 下一步工作

1. **完善OutputControl**：添加词典模式和多服务结果展示
2. **语言选择器**：绑定实际的语言切换逻辑
3. **历史记录**：HistoryControl实现
4. **设置面板**：ServicePanel和SettingsWindow
5. **主题系统**：完整的亮色/暗色主题支持

### 🧪 测试建议

```bash
# 编译并运行测试
cd /home/yyt/Downloads/STranslate.Avalonia
dotnet build && dotnet run

# 测试功能：
# 1. 输入文本（支持Ctrl+滚轮调节字体）
# 2. 点击"翻译"按钮
# 3. 查看翻译结果显示
# 4. 测试工具栏按钮（复制、语音等）
```

---

## 快捷键 (默认)

| 功能 | 快捷键 |
| --- | --- |
| 打开主窗口 | `Alt + F8` |
| 截图翻译 | `Alt + A` |
| 划词翻译 | `Alt + D` |
| 退出程序 | `Ctrl + Shift + Q` |
| 隐藏窗口 | `Esc` |

## 目录结构

```plaintext
STranslate.Avalonia/
├── App.axaml              # 应用程序入口
├── App.axaml.cs
├── Program.cs             # Main 函数
├── Assets/                # 资源文件
│   └── app.ico
├── ViewModels/            # MVVM ViewModels
│   ├── ViewModelBase.cs
│   └── MainWindowViewModel.cs
├── Views/                 # 界面文件
│   ├── MainWindow.axaml
│   └── MainWindow.axaml.cs
├── Core/                  # 核心业务逻辑 (从 WPF 版迁移)
├── Services/              # 服务层
│   └── Linux/             # Linux 特定实现
│       ├── HotkeyService.cs
│       ├── ScreenshotService.cs
│       └── ClipboardService.cs
├── Helpers/               # 工具类
├── Converters/            # 数据转换器
└── STranslate.Avalonia.csproj
```

## 技术栈

| 组件 | 技术 |
| --- | --- |
| UI 框架 | Avalonia UI 11.2.2 |
| MVVM  | ReactiveUI |
| 全局快捷键 | SharpHook 5.3.7 |
| 数据库 | SQLite (Microsoft.Data.Sqlite) |
| HTTP | HttpClient (System.Net.Http) |
| 日志 | Serilog |
| DI 容器 | Microsoft.Extensions.DependencyInjection |

## 从 WPF 迁移的兼容性

| WPF 特性 | Avalonia 替代方案 | 状态 |
| --- | --- | --- |
| `Window.InputBindings` | `Window.KeyBindings` | ✅ 已适配 |
| `TaskbarIcon` | `TrayIcon` | 🚧 进行中 |
| `NHotkey.Wpf` | `SharpHook` | ✅ 已实现 |
| `ScreenGrab` | `gnome-screenshot` | ✅ 已实现 |
| `iNKORE.UI.WPF.Modern` | `Avalonia.Themes.Fluent` | ⏳ 计划中 |

## 故障排除

### 全局快捷键不工作

```bash
# 检查 SharpHook 权限
sudo setcap cap_net_admin+eip /opt/STranslate.Avalonia/STranslate.Avalonia
```

### 截图失败

```bash
# 确认截图工具已安装
which gnome-screenshot || sudo dnf install gnome-screenshot
```

### Wayland 支持

项目已通过 SharpHook 支持 Wayland，但部分功能可能需要 XWayland 兼容层。

## 贡献

欢迎提交 Issue 和 Pull Request！

## 许可证

MIT License - 继承自原 STranslate 项目

## 致谢

- 原项目: [STranslate](https://github.com/STranslate/STranslate) by [@zggsong](https://github.com/zggsong)
- Avalonia UI Team
- SharpHook Contributors
