# aabViewer

- 软件功能都是通过调用命令行工具处理的。
- 使用之前需要有java环境。
- v2.3之后的包中移除了谷歌的库，可以在这里[下载](https://github.com/google/bundletool/releases)
- 如果想了解build-tools具体的使用可以点击[这里](https://developer.android.com/studio/command-line/bundletool)
- 配置文件中的xml节点的配置是xpath语法，如果想了解更多可以点击[这里 ](https://www.w3school.com.cn/xpath/xpath_syntax.asp)
- 在快捷方式的选项卡里的目标里加入参数 Logcat可以直接启动Logcat页面

下拉选择框 UCheckComboBox 来自

[winform 下拉框多选 List ItemsComboBox _winform combox做多选-CSDN博客](https://blog.csdn.net/qq_38567182/article/details/145593621?spm=1001.2101.3001.6650.1&depth_1-utm_source=distribute.pc_relevant.none-task-blog-2%7Edefault%7EYuanLiJiHua%7EPaidSort-1-145593621-blog-46007311.235%5Ev43%5Epc_blog_bottom_relevance_base2&utm_relevant_index=2)





https://github.com/corle-bell/aabViewer/blob/main/ScreenShot/main.png



https://github.com/user-attachments/assets/23495070-91c3-4943-9229-cb0d4946c232

## v1.0

可以查看内容有 
版本号 
包名 
是否存在调试标识 
最低支持版本 
编译版本 
facebookid 

可以生成签名base64hash值 
sha1指纹 

## v2.0

需要根据配置文件读取manifest下的节点信息
规则如下：

{显示名称}#{节点路径}#{过滤器}

节点路径是采用x-path的方式查找

过滤器为空为不启用

在存在多个相同路径节点的情况下使用过滤器，过滤器根据属性的name判断，返回value的值

例如多个meta-data节点统计并存那么可以这样查找
FacebookId#/manifest/application/meta-data#com.facebook.sdk.ApplicationId

## v2.1

增加了拖拽功能和安装失败的提示  
可以把拖拽签名文件和aab直接拖入程序内

## v2.2

修复了路径存在空格导致无法解析和安装的问题

## v2.3

在没有输入签名文件的时候使用配置文件中的默认签名安装 
增加了输出日志 在生成以及安装存在错误是会输出到根目录的log.txt 
移除了Relase中的 bundletool-all-1.8.0.jar，可以从谷歌的[github](https://github.com/google/bundletool/releases)上下载,或者下载v2.3之前的版本里面有。  

## v2.4

在读取manifest节点下的值是string.xml的key的话 现在会直接转化为目标值  
增加了读取icon的功能

## v2.5

集成了Openssl工具在程序内，增加了运行环境的检测

## v2.6

增加了连接设备信息的界面，调整UI布局,内置Openssl行不通我又给删掉了。。  

## v2.7

增加了universal模式安装。某些无法安装的设备可以使用这个来安装  

## v2.8

解析,安装 aab和 获取手机信息改为了异步操作。  
增加了一个loading界面  
增加了获取签名文件MD5值的功能   

## v2.9

增加了签名管理功能  
修改了缓存更新的机制，如果不重启应用 不更换aab就不会更新apks的缓存  

## v3.0

增加了签名创建功能  
修改了环境检测的流程  

## v3.1.1

增加了签名创建的自定义模板  
增加了移除签名功能  
优化UI布局  

> 配置文件路径：Config/sign.ini     
> 别名#密码#期限#名字#组织机构#组织机构名称#市#省#国家代码

## v3.1.1

修复了创建签名的有效期不正确的错误  

## v3.1.2

增加查看log的按钮  
增加快速打开缓存目录的按钮  
增加了查看AndroidManifest的功能  

## v4.0.0

增加了查看Apk的功能  
增加了直接运行应用的功能  
修改了界面布局  
修改了图标  
重构了代码结构

## v4.0.1

修复了在分区根目录报错的问题

## v4.0.2

添加XAPK的解析器

## v4.0.3

移除了DotNetZip,更改为.Net自带的压缩库  

需要Framework4.5及以上。

## v4.0.4

修复了某些机型无法覆盖安装的问题。

## v4.0.5

adb改为直接调用  
优化安装并运行的流程  
增加清除进程功能,清理卡住的adb进程

## v5.0.0

增加Logcat功能  
增加Firebase的调试开启  

## v5.0.1

优化Logcat功能  
通过参数Logcat调用可以直接 调起Logcat页面

## v5.0.2

优化log展示的列表，使用了ListView的虚拟化。解决了筛选后卡顿的问题。  

## v5.0.3

修复了直接关闭程序可能导致程序报错的  
获取当前运行的进程改为了异步操作，避免打开页面时候的卡顿。

## v5.0.4

AAB安装后会自动记录当前使用的签名信息，下次进入可直接匹配。  
增加了检查更新的功能。

## v5.0.5

修复了自动更新切换无效的问题。  
打开时会检查更新,每天只检测一次。

## v5.0.6

修复了AAB获取启动Activity不正确的问题。  
修复了更新时下载地址不正确的问题。

## v5.0.7

增强了TAG筛选功能，增加了TAG剔除功能。可以保存TAG配置。  
增加暂停继续按钮  
优化了Log的解析