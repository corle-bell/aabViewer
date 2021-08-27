# aabViewer
软件功能都是通过调用命令行工具处理的。
使用之前需要有java环境。
如果需要使用签名base64功能需要安装 openssl 并且配置环境变量。

**v1.0**
可以查看内容有
版本号
包名
是否存在调试标识
最低支持版本
编译版本
facebookid

可以生成签名base64hash值
sha1指纹

**v2.0**
需要根据配置文件读取manifest下的节点信息
规则如下：

{显示名称}#{节点路径}#{过滤器}

节点路径是采用x-path的方式查找

过滤器为空为不启用

在存在多个相同路径节点的情况下使用过滤器，过滤器根据属性的name判断，返回value的值

例如多个meta-data节点统计并存那么可以这样查找
FacebookId#/manifest/application/meta-data#com.facebook.sdk.ApplicationId


**v2.1**
增加了拖拽功能和安装失败的提示
可以把拖拽签名文件和aab直接拖入程序内

**v2.2**
修复了路径存在空格导致无法解析和安装的问题
