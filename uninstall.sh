#!/bin/bash
# STranslate Avalonia 卸载脚本

set -e

if [ "$EUID" -ne 0 ]; then
    echo "❌ 请使用 sudo 运行此脚本"
    exit 1
fi

echo "=========================================="
echo " STranslate Avalonia - Uninstall"
echo "=========================================="

INSTALL_DIR="/opt/STranslate.Avalonia"

# 💡 删除安装目录
if [ -d "$INSTALL_DIR" ]; then
    echo "🗑️  删除程序文件: $INSTALL_DIR"
    rm -rf "$INSTALL_DIR"
else
    echo "ℹ️  未找到安装目录"
fi

# 💡 删除 desktop 文件
if [ -f "/usr/share/applications/stranslate.desktop" ]; then
    echo "🗑️  删除桌面快捷方式"
    rm -f /usr/share/applications/stranslate.desktop
    
    if command -v update-desktop-database &> /dev/null; then
        update-desktop-database /usr/share/applications
    fi
fi

# 💡 删除符号链接
if [ -L "/usr/local/bin/stranslate" ]; then
    echo "🗑️  删除符号链接"
    rm -f /usr/local/bin/stranslate
fi

echo ""
echo "✅ 卸载完成！"
