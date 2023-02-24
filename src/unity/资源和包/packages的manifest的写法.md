参考资料：https://docs.unity3d.com/cn/current/Manual/upm-scoped.html

# 注册表服务器
开发者可以自己搭建包注册表服务器，这是一个中心化的包存储服务器，可以使用npm管理。  


如下例所示，在scopedRegistries里面，定义了General和Tools两个服务器，PackageManager去指定的URL处下载包，将包复制到Library目录下面。
每个服务器有一个scopes，它是一个数组，存储一些包名的前缀。PackageManager会选择匹配度最高的服务器去下载包。  
```json
{
    "scopedRegistries": [
        {
            "name": "General",
            "url": "https://example.com/registry",
            "scopes": [
                "com.example", "com.example.tools.physics"
            ]
        },
        {
            "name": "Tools",
            "url": "https://mycompany.example.com/tools-registry",
            "scopes": [
                "com.example.mycompany.tools"
            ]
        }
    ],
    "dependencies": {
        "com.unity.animation": "1.0.0",
        "com.example.mycompany.tools.animation": "1.0.0",
        "com.example.tools.physics": "1.0.0",
        "com.example.animation": "1.0.0"
    }
}
```

manifest.json一般是不需要手动编辑的，可以在UI里面进行设置。 打开ProjectSettings，选择PackageManager即可。  
# 包的三种特殊形式
https://docs.unity3d.com/cn/current/Manual/upm-git.html
除了可以通过制定包名+版本号的方式指定依赖，还可以通过以下三种方式指定依赖：
1. git仓库：`"com.mycompany.mypackage": "git@mycompany.github.com:gitproject/com.mycompany.mypackage.git"`
2. 本地文件夹：`"my_package_a": "file:../github/my_package_folder"`
3. 嵌入式依赖项：直接把包文件夹放在packages目录下面。  

# 包的脚本API
https://docs.unity3d.com/cn/current/Manual/upm-api.html

常见用途：
1. 获取已经安装的包的列表
2. 将包添加到项目
3. PackageManager事件