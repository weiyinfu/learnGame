# 什么是unity？
unity是一个游戏框架，它的功能非常丰富，相比其它框架的优势在于对于3d游戏的支持比较完善。
什么是游戏框架？游戏框架就是把游戏常用到的代码抽象成通用的代码，避免任何游戏都从头开发。例如提供物理、碰撞检测、3d渲染等功能。
游戏首先就有物体，有了物体，可以给物体挂载一堆脚本。物体可以挂载属性，物体是游戏中的核心东西。
# 学习资料
* 官方文档：https://docs.unity3d.com/cn/2020.3/Manual/GettingStartedAddingEditorComponents.html
* https://www.w3cschool.cn/unity3d_jc/unity3d_jc-9dvl37yg.html
* c语言编程网：http://c.biancheng.net/unity3d/80/
* 本地文档：file:///Users/bytedance/Desktop/myUnity/2020.3.24f1/Documentation/en/Manual/index.html
* wangxuanyi的unity博客：https://gameinstitute.qq.com/community/detail/102158
* unity官方文档：
  https://docs.unity3d.com/Manual/XR.html
* unity官方文档中文版：
https://docs.unity3d.com/cn/2019.4/Manual/UNetPlayersCustom.html
* unity本地文档：
  file:///Users/bytedance/Desktop/myUnity/2020.3.24f1/Documentation/en/Manual/index.html

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
一切皆资源。GameObject就像一个衣服架子，上面可以挂一些Component。Component就是代表功能、代表特效。

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

# unity的调试
在web环境中，chrome的console非常好用，unity是一个exe，exe可以实现自己的console，exe可以完全控制所有的内容。  

# unity架构的特点
- EntityComponentSystem：ECS架构
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

# position和localPosition的区别
position是根据世界原点为中心   
localPosition是根据父节点为中心,如果没有父节点,localpositon和position是没有区别的  
选中一个物体左上角Global和Local切换看物体世界坐标轴和本地坐标轴  
transform组件下的Position属性是一个相对于父对象的局部坐标，当然了，如果其没有父对象，其属性框里显示的自然而然也就是世界坐标。如果有父对象，那么在unity编辑器显示的值是这个transform以上一级父对象为基准的坐标。   


# Unity提供了几种光源，分别是什么
答：
四种。
平行光：Directional Light
点光源：Point Light
聚光灯：Spot Light
区域光源：Area Light