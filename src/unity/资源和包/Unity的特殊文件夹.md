Unity检测Assets目录下面的一些特殊文件夹名称，这些文件夹都有各自用途。文件夹不一定直接放在Assets目录下面，可以嵌套多层。
# Editor文件夹
1. Editor测试脚本
2. Editor菜单、窗口定制脚本


# Plugins
C/C++的插件目录。

# Resources
资源文件。可以使用Resources.Load()直接加载。  
这个文件夹下的内容会加密打包到应用里面。  
# StreamingAssets
同样是资源文件。但是这些资源不会被加密，解压apk之后能够找到这些资源，这些资源文件以独立文件的形式存在。  

# 隐藏的资源
在导入package、unitypackage的过程中，Unity自动忽略一些特殊文件和文件夹，这些包括：
* 隐藏文件夹，即以`.`开头，包括.git
* 以~开头的文件夹，Unity把它们当做Sample
* 名为cvs的文件和文件夹，这是一个CVS这种版本管理工具
* 临时文件，即以.tmp结尾的文件