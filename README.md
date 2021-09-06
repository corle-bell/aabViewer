# aabViewer

 - 软件功能都是通过调用命令行工具处理的。
 - 使用之前需要有java环境。
 - 如果需要使用签名base64功能需要安装 openssl 并且配置环境变量(v2.5后版本已经内置openssl)。
 - v2.3之后的包中移除了谷歌的库，可以在这里[下载](https://github.com/google/bundletool/releases)
 - 如果想了解build-tools具体的使用可以点击[这里](https://developer.android.com/studio/command-line/bundletool)
 - 配置文件中的xml节点的配置是xpath语法，如果想了解更多可以点击[这里 ](https://www.w3school.com.cn/xpath/xpath_syntax.asp)



### v1.0
可以查看内容有
版本号
包名
是否存在调试标识
最低支持版本
编译版本
facebookid

可以生成签名base64hash值
sha1指纹

### v2.0
需要根据配置文件读取manifest下的节点信息
规则如下：

{显示名称}#{节点路径}#{过滤器}

节点路径是采用x-path的方式查找

过滤器为空为不启用

在存在多个相同路径节点的情况下使用过滤器，过滤器根据属性的name判断，返回value的值

例如多个meta-data节点统计并存那么可以这样查找
FacebookId#/manifest/application/meta-data#com.facebook.sdk.ApplicationId


### v2.1
增加了拖拽功能和安装失败的提示
可以把拖拽签名文件和aab直接拖入程序内

### v2.2
修复了路径存在空格导致无法解析和安装的问题

### v2.3
在没有输入签名文件的时候使用配置文件中的默认签名安装
增加了输出日志 在生成以及安装存在错误是会输出到根目录的log.txt
移除了Relase中的 bundletool-all-1.8.0.jar，可以从谷歌的[github](https://github.com/google/bundletool/releases)上下载,或者下载v2.3之前的版本里面有。

### v2.4
在读取manifest节点下的值是string.xml的key的话 现在会直接转化为目标值
增加了读取icon的功能

### v2.5
集成了Openssl工具在程序内，增加了运行环境的检测