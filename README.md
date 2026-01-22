# UBBRender
#### 起点
这里是一个用于解析UBB代码为XAML元素的可视化调试工具。
内部包含：
语法分割器；解析器；渲染器；AST呈现器。
#### 从源开始构建
1打开Visual Studio 2022.

2安装WinUI3所需的工作负载，包括Win10.0.19041 SDK.

3将“https://pkgs.dev.azure.com/dotnet/CommunityToolkit/_packaging/CommunityToolkit-Labs/nuget/v3/index.json”添加到Nuget源。

4克隆本仓库，并打开项目，运行生成，等待自动补全所有Nuget包。

5现在，你已经完成所有调试准备。

## UBB语法定义(待补全)

### Token:
LEFT_BRACKET:'[',
RIGHT_BRACKET:']',
SLASH:'/'
EQUAL:'=',
COMMA:',',
UBB_TAG_NAME=
粗体 [b]
斜体 [i]
下划线 [u]
删除线 [del]
字体大小 [size]
字体 [font]
颜色 [color]
链接 [url]
图片 [img]
音频 [audio]
视频 [video]
代码块 [code]
引用 [quote]
对齐 [align]
左对齐 [left]
居中 [center]
右对齐 [right]
列表 [list]
列表项 [*]
段落（自动生成）
换行 [br]
分隔线 [hr]
表情 [em]
行内公式 $Latex$
行间公式 $$Latex$$

### 语法:
UBB_TEXT = Any string not containing : "[]"
UBB_ATTRIBUTE_VALUE = Any string not containing : "[]$,="
UBB_TAG_ATTRIBUTE = COMMA UBB_ATTRIBUTE_VALUE
UBB_TAG_ATTRIBUTES = EQUAL UBB_ATTRIBUTE_VALUE UBB_TAG_ATTRIBUTE*
UBB_OPEN_TAG= LEFT_BRACKET UBB_TAG_NAME UBB_TAG_ATTRIBUTES? RIGHT_BRACKET
UBB_CLOSE_TAG = LEFT_BRACKET SLASH UBB_TAG_NAME RIGHT_BRACKET
UBB_SELF_CLOSING_TAG = LEFT_BRACKET UBB_TAG_NAME UBB_TAG_ATTRIBUTES? RIGHT_BRACKET
UBB_NODE := UBB_OPEN_TAG UBB_TEXT UBB_CLOSE_TAG
          = UBB_SELF_CLOSING_TAG

### Example

[begin][/begin]
[hr]
[ac01]
[img=https://test.cc98.org/v2/upload.jpg][/ig]
