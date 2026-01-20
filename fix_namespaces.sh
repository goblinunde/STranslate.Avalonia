#!/bin/bash
# 批量修复命名空间：STranslate → STranslate.Avalonia

TARGET_DIR="/home/yyt/Downloads/STranslate.Avalonia"

echo "开始修复命名空间..."

# 修复 .cs 文件中的命名空间
find "$TARGET_DIR" -name "*.cs" -type f -exec sed -i \
  -e 's/namespace STranslate\b/namespace STranslate.Avalonia/g' \
  -e 's/using STranslate\b/using STranslate.Avalonia/g' \
  -e 's/using STranslate\./using STranslate.Avalonia./g' \
  {} +

echo "命名空间修复完成！"

# 统计修改的文件数量
echo "修改的 C# 文件数量:"
find "$TARGET_DIR" -name "*.cs" -type f | wc -l
