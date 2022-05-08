# unitypackage
unitypackage可以用于打包一些资源、脚本等。unitypackage导入之后就会导入到assets里面，如果assets有依赖的话，也会相应的导入依赖。在导出unitypackage的时候，可以取消勾选"include dependencies"

# unity导出资源的正确方法
unity的文件分为两类：Assets和Packages
Pacakges大多数情况下只是维护一个包的引用，但是也可以直接把包拷贝到Pacakges目录下面，这样分享给别人的时候就不会出现找不到包的问题了。
如果对Packages进行修改，则会永远进行修改，可能会影响到其它的Unity项目，因为Package只是引用。
导出资源的时候使用ExportAssets进行导出。

# assets目录
Resources：
需要动态加载的文件放入，打包时，在这个文件夹里的文件不管有没有被使用，都会被打包出来。
Plugins：
插件目录，该目录编译时会优先编译，以便项目中调用。
这里我们在使用Android项目的时候会将.jar .aar文件放入Plugins\Android\libs中，而将.so 64位文件Plugins\Android\libs\arm64-v8a，.so 32位放入Plugins\Android\libs\armeabi-v7a中
Editor:
该目录下的代码可调用Unity Editor 的API，存放扩展编辑器的代码。编译时不会被打包到游戏中。
Standard Assets
该文件夹下的文件会优先被编译，以便项目调用，它与Plugins一样，打包时会被编译到同一个.sln文件里。
SteamingsAssets
该目录下的文件会在打包时打包到项目中去，与Resources一样不管有没有用到的文件，在打包时都会被打包出来。
Resources与SteamingsAssets的区别
Resources下 文件在打时会进行压缩与加密，但是StremingsAssets下的文件是直接被打包出来。所以SteamingsAssets中主要存放2进制文件
Resources中的材质球、预制体等资源，会在打包时自动寻找引用资源，打包到Resurce中。

# package原理
unity的package是在电脑上共用的，项目A和项目B依赖同一个package，则这个package在磁盘上位于同一个位置，在项目A里面更改package的内容会影响到项目B。  
把包直接放在Packages目录下面，则能够解决package位置问题。  
从外部引入的packages是soft link，而不是复制一份。 