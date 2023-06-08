Unity检测Assets目录下面的一些特殊文件夹名称，这些文件夹都有各自用途。文件夹不一定直接放在Assets目录下面，可以嵌套多层。
# Editor文件夹
1. Editor测试脚本
2. Editor菜单、窗口定制脚本


# Plugins
C/C++的插件目录。

# Resources
资源文件。可以使用Resources.Load()直接加载。  
这个文件夹下的内容会加密打包到应用里面。  

* Resources文件夹下的资源无论使用与否都会被打包
* 资源会被压缩，转化成二进制
* 打包后文件夹下的资源只读
* 无法动态更改，无法做热更新
* 使用Resources.Load加载

# StreamingAssets
同样是资源文件。但是这些资源不会被加密，解压apk之后能够找到这些资源，这些资源文件以独立文件的形式存在。  


* 流数据的缓存目录
* 文件夹下的资源无论使用与否都会被打包
* 资源不会被压缩和加密
* 打包后文件夹下的资源只读，主要存放二进制文件
* 无法做热更新
* WWW类加载（一般用CreateFromFile ，若资源是AssetBundle，依据其打包方式看是否是压缩的来决定）
* 相对路径，具体路径依赖于实际平台，Android：Application.streamingAssetsPath；IOS: Application.dataPath + “/Raw” 或Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/xxx.app/Data/Raw

# Application.dataPath
* 游戏的数据文件夹的路径（例如在Editor中的Assets）
* 很少用到
* 无法做热更新
* IOS路径: Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/xxx.app/Data

# Application.persistentDataPath
* 持久化数据存储目录的路径（ 沙盒目录，打包之前不存在 ）
* 文件夹下的资源无论使用与否都会被打包
* 运行时有效，可读写
* 无内容限制，从StreamingAsset中读取二进制文件或从AssetBundle读取文件来写入PersistentDataPath中
* 适合热更新
* IOS路径: Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Documents

# 隐藏的资源
在导入package、unitypackage的过程中，Unity自动忽略一些特殊文件和文件夹，这些包括：
* 隐藏文件夹，即以`.`开头，包括.git
* 以~开头的文件夹，Unity把它们当做Sample
* 名为cvs的文件和文件夹，这是一个CVS这种版本管理工具
* 临时文件，即以.tmp结尾的文件




