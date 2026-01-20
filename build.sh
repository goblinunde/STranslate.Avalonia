#!/bin/bash
# STranslate Avalonia 构建脚本

set -e

echo "=========================================="
echo " STranslate Avalonia - Linux Build Script"
echo "=========================================="

# 💡 检查 .NET SDK
if ! command -v dotnet &> /dev/null; then
    echo "❌ 错误: 未检测到 .NET SDK"
    echo "请先安装: sudo dnf install -y dotnet-sdk-8.0"
    exit 1
fi

echo "✅ .NET SDK 版本:"
dotnet --version

# 💡 恢复依赖
echo ""
echo "📦 恢复 NuGet 包..."
dotnet restore

# 💡 构建项目
echo ""
echo "🔨 构建项目..."
dotnet build -c Release

# 💡 发布为独立应用
echo ""
echo "📦 发布 Linux 版本 (自包含)..."
dotnet publish -c Release -r linux-x64 --self-contained true \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true \
    -p:EnableCompressionInSingleFile=true

echo ""
echo "=========================================="
echo "✅ 构建完成！"
echo "=========================================="
echo ""
echo "可执行文件位置:"
echo "  bin/Release/net8.0/linux-x64/publish/STranslate.Avalonia"
echo ""
echo "运行测试:"
echo "  ./bin/Release/net8.0/linux-x64/publish/STranslate.Avalonia"
echo ""
echo "安装到系统:"
echo "  sudo ./install.sh"
echo "=========================================="
