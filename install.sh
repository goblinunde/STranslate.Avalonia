#!/bin/bash
# STranslate Avalonia 系统安装脚本

set -e

if [ "$EUID" -ne 0 ]; then
    echo "❌ 请使用 sudo 运行此脚本"
    exit 1
fi

echo "=========================================="
echo " STranslate Avalonia - System Install"
echo "=========================================="

INSTALL_DIR="/opt/STranslate.Avalonia"
BUILD_DIR="bin/Release/net8.0/linux-x64/publish"

# 💡 检查构建产物
if [ ! -f "$BUILD_DIR/STranslate.Avalonia" ]; then
    echo "❌ 错误: 未找到构建产物"
    echo "请先运行: ./build.sh"
    exit 1
fi

# 💡 创建安装目录
echo "📁 创建安装目录: $INSTALL_DIR"
mkdir -p "$INSTALL_DIR"

# 💡 复制文件
echo "📦 复制程序文件..."
cp -r "$BUILD_DIR"/* "$INSTALL_DIR/"

# 💡 设置执行权限
echo "🔐 设置执行权限..."
chmod +x "$INSTALL_DIR/STranslate.Avalonia"

# 💡 安装 desktop 文件
echo "🖥️  安装桌面快捷方式..."
cp stranslate.desktop /usr/share/applications/

# 💡 更新 desktop 文件路径
sed -i "s|/opt/STranslate.Avalonia|$INSTALL_DIR|g" /usr/share/applications/stranslate.desktop

# 💡 更新 desktop 数据库
if command -v update-desktop-database &> /dev/null; then
    update-desktop-database /usr/share/applications
fi

# 💡 创建符号链接 (可选)
echo "🔗 创建符号链接..."
ln -sf "$INSTALL_DIR/STranslate.Avalonia" /usr/local/bin/stranslate

echo ""
echo "=========================================="
echo "✅ 安装完成！"
echo "=========================================="
echo ""
echo "使用方法:"
echo "  1. 从应用程序菜单启动 STranslate"
echo "  2. 命令行运行: stranslate"
echo "  3. 直接运行: $INSTALL_DIR/STranslate.Avalonia"
echo ""
echo "默认快捷键:"
echo "  Alt+F8    打开主窗口"
echo "  Alt+A     截图翻译"
echo "  Alt+D     划词翻译"
echo "=========================================="
