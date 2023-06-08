# 什么是unity？
unity是一个游戏框架，它的功能非常丰富，相比其它框架的优势在于对于3d游戏的支持比较完善。  
什么是游戏框架？游戏框架就是把游戏常用到的代码抽象成通用的代码，避免任何游戏都从头开发。例如提供物理、碰撞检测、3d渲染等功能。  
游戏首先就有物体，有了物体，可以给物体挂载一堆脚本，从而控制物体的行为。物体是游戏中的核心东西。  
# 学习资料
* 官方文档：<https://docs.unity3d.com/cn/2020.3/Manual/GettingStartedAddingEditorComponents.html>
* Unity官方文档脚本参考：<https://docs.unity3d.com/ScriptReference/AsyncOperation.html>
* <https://www.w3cschool.cn/unity3d_jc/unity3d_jc-9dvl37yg.html>
* c语言编程网：<http://c.biancheng.net/unity3d/80/>
* 本地文档：file:///家目录/myUnity/2020.3.24f1/Documentation/en/Manual/index.html
* wangxuanyi的unity博客：https://gameinstitute.qq.com/community/detail/102158
* unity官方文档：<https://docs.unity3d.com/Manual/XR.html>
* <del>unity官方文档中文版：https://docs.unity3d.com/cn/2019.4/Manual/UNetPlayersCustom.html</del>
* unity cn中文版文档
https://docs.unity.cn/cn/current/Manual/UnityOverview.html
* unity本地文档：file:///Users/bytedance/Desktop/myUnity/2020.3.24f1/Documentation/en/Manual/index.html

# 脚本系统
unity支持 csharp，js，boo三种语言，然而csharp是支持最好的，js和boo逐渐被淘汰。boo其实就是python的变形。  
unity为什么要支持csharp而不是java、python？

# csharp
官方教程：https://docs.microsoft.com/zh-cn/dotnet/csharp/

# unity的IDE
- rider：强烈推荐，jetbrain出品，必属精品。
- visual studio：mac下不推荐，相比windows上的visual studio属于阉割版。
- visual studio code：不推荐

# unity入口
从一些根类、根包出发学习unity。

尝试use UnityEngine.，查看有哪些子包
* UnityEngine
* this
* GameObject

unity有许多场景，一个场景包含多个GameObject，它们之间组成了对象树。每个GameObject有若干个Component。

GameObject和Component是Unity的核心哲学。Camera、Transform等都可以看做是一种Component，其中Transform这个Component是必备的。
如果想用纯代码实现一个unity程序，只需要创建一个空的GameObject，然后给这个GameObject挂一个脚本即可。
一切皆资源。GameObject就像一个衣服架子，上面可以挂一些Component。Component代表功能、代表特效。

unity界面重要入口：
* File/BuildSettings：构建设置，设置目标平台
* Edit/ProjectSettings：整个项目的设置，重要入口PlayerSettings
* Window/Package Manager：整个项目的包管理
* Unity/Preference:整个Unity的设置

Unity脚本API四大入口  
- UnityEngine：引擎相关
  - Accessibility：可访问性
  - AI：AI算法
  - Analytics：分析
  - Android：安卓相关
  - Animations：动画相关
  - Apple：苹果手机相关
  - Assertions：断言相关
  - Audio：语音
  - CrashReportHandler：系统崩溃报告
  - Diagnostics：诊断
  - Events：时间
  - Experimental：实现模块
  - iOS
  -
- UnityEditor：Unity编辑器相关
- Unity：性能、渲染、Jobs，一些工具类
- Other

# unity架构的特点
- EntityComponentSystem：ECS架构，GameObject是实体，实体包含多个Component。
- C# Job System
- Burst Compiler
- shader解决方案：shaderLab+HLSL。
合称为DataOrientedTechStack（DOTS）。

# 2d和3d
图像在2d模式下是sprite，在3d模式下是纹理，texture。
3d的分类：
- 透视3d：摄像机是一个点，是一种第一人称风格。
- 正交3d：摄像机是一个面，有时候也成为鸟瞰图，2.5d，是一种第三人称风格。


# 基本概念
MeshFilter：定义了3d物体的形状，要想给一个GameObject定义形状，只需要给它添加一个MeshFilter。  
MeshRenderer：定义了MeshFilter确定的形状的外观。  
材质：Materials，材质把可视化表面组合起来，例如Textures、Color tints，Shaders等。使用材质可以定义如何渲染物体的表面。  
Prefabs：预制体，预制体相当于Class，可以创造出很多个对象。  
游戏由一个一个的场景组成，场景有若干个游戏物体组成，游戏物体有许多分量，每个分量都是代表了一类功能，分量就是Component，一个GameObject可以持有多个Component。脚本也是Component。


# unity安装历史版本
在unity hub中打开install页面，通常只会显示大版本的最新版，要想安装历史版本，需要去网页中点击使用unityhub下载。   
中文版通常小版本有6位，建议使用英文版。  
中文版：https://unity.cn/releases/full/2019
英文版：https://unity3d.com/cn/get-unity/download/archive



# Unity为什么采用在Update里面主动获取事件而不是像GUI一样通过事件触发回调？
Unity把输入看做一个存储，当开发者需要某个事件的时候需要手动去读取这个输入。  
GUI把输入看做一个触发源。

GUI是静态的，刷新频率很低，当用户没有操作的时候，界面就不需要刷新。  
游戏是动态的，刷新频率很高，当用户没有操作的时候，对于大多数游戏来说依旧需要不停地更新UI。所以游戏一定需要Update()方法。  
GUI的输入相对简单，并且大部分都是离散信号，例如鼠标点击了某个按钮，键盘敲击了某个字符等。  
游戏的输入是连续的，例如在XR游戏中需要不停地获取手柄的位置信息、角度信息，这些信息是始终存在输入的，如果使用事件回调的机制就会导致不停地回调。   

# Unity编译优化
1. 如果有多个平台，可以使用Assets软链接的方式，让多个项目共享一套代码，避免频繁切换平台导致的效率低下
2. 使用mono+32位能够显著提升打包效率
3. 拆分程序集。使用动态链接库，把脚本分成几个不同的assemblies，分别构建成dll
4. 使用更高配置的电脑，CPU好、内存大对于编译速度的提升非常明显，例如使用固态硬盘

# Unity的文档系统
* Manual：包括两部分，核心手册和模块手册，例如Input System，XR Interaction Toolkit等是两个重要的
* Scripting API

Unity的内置模块：
* Android JNI
* Animation
* Asset Bundle
* Audio
* Cloth
* Director
* Image Conversion
* IMGUI
* JSONSerialize
* Particle System
* Physics
* Physics 2D
* Screen Capture
* subsystems
* Terrain
* Terrain Physics
* TileMap
* UI
* UIElements
* Umbra
* Unity Anylytics
* Unity Web Request
* Unity Web Request Asset Bundle
* Unity Web Request Audio
* Unity Web Request Texture
* Unity Web Request WWW
* Vihicles
* Video
* VR
* Wind
* XR
# 课程资料
GAMES101-现代计算机图形学入门-闫令琪：
https://www.bilibili.com/video/BV1X7411F744?p=1&vd_source=3057378174726ae1406e5bbc9f83c3b2

GAMES202-高质量实时渲染
https://www.bilibili.com/video/BV1YK4y1T7yY/?spm_id_from=333.788.recommend_more_video.0&vd_source=3057378174726ae1406e5bbc9f83c3b2

GAMES203: 三维重建和理解
https://www.bilibili.com/video/BV1pw411d7aS/?spm_id_from=333.788.recommend_more_video.0&vd_source=3057378174726ae1406e5bbc9f83c3b2

计算机图形学基础
https://www.bilibili.com/video/BV1rL411x7KC/?spm_id_from=333.788.recommend_more_video.5&vd_source=3057378174726ae1406e5bbc9f83c3b2

一个知乎大佬：
https://www.zhihu.com/people/lxyhpp/posts