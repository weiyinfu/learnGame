全球版：https://learn.unity.com/， VR开发专题：https://learn.unity.com/pathway/vr-development?signup=true

国内版：https://learn.u3d.cn/

可选包：https://docs.unity3d.com/Manual/pack-safe.html

w3school：https://www.w3cschool.cn/unity3d_jc/unity3d_jc-6jet38c2.html

unity国内开发者社区：https://developer.unity.cn/

Unity资源商店：https://assetstore.unity.com/

# Unity教程的结构

Unity教程分为手册和脚本API两部分。

手册是功能说明。

脚本API 是API Reference。

Unity教程都是讲的Unity的内置模块。

Unity Registry是Unity官方提供的可选包，这些可选包的文档在另一个地方：https://docs.unity3d.com/Manual/pack-safe.html

# Unity教程的主题

其中比较重要的是图形、物理，了解了这两块基本上做游戏就没问题了。我最需要恶补的就是图形。

输入

2D

图形

世界构建：地形、树

物理

脚本

多人游戏

音视频

动画

UI

导航和寻路

Unity服务

XR

# Unity的包

打开Unity的PackageManager，浏览一下Unity的各种包。

Unity的包分为四类：

- 内置包：Unity 编辑器自带的包

- Unity Registry：Unity官方包

- InProject：项目中已经导入的包

- MyAssets：我收藏的包

# Unity的内置包

## UI

- IMGUI：Unity的立即UI，只能用在Editor中

- UI：Unity的拖拽式UI，每一个元素都是一个GameObject，只能用在运行时

- UI Elements：Unity新版UI

- UI Elements Native

## 地形

- Terrain

- TerrainPhysics

- Tilemap

- Wind：风区，可以影响地形和粒子效果

## 物理

- Cloth

- Vehicles:汽车的物理模拟，使用轮子碰撞器组件

- ParticleSystem

- Physics、Physics2D

## 多媒体

- ImageConversion

- ScreenCapture

- Audio

- Video

## 网络

- Unity Web Request
  
  - Unity Web Request Asset Bundle
  
  - Unity Web Request Audio
  
  - Unity Web Request Texture

## 其它

- AI：实现了寻路算法

- AndroidJNI

- AssetBundle

- Director

- JsonSerialize：Unity最早实现的JSON序列化库，基本上全是bug。

- Subsystems

- Umbra：遮挡剔除系统

- Analytics：埋点相关

- XR、VR：

- Wind

# Unity官方包

## 2D相关

- 2D Animation

- 2D IK：preview阶段，根据部分动作预测整体动作。

- 2D Pixel Perfect

- 2D PSD Importer

- 2D Sprite

- 2D SpriteShape

- 2D Tilemap Editor

- 2D Tilemap Extras（Preview阶段）

## 自适应性能

- Adaptive Performance

- AdaptivePerformance 三星

## Unity服务

Advertisement：广告；iOS14 Advertising Support：iOS上的广告支持

Authentication：授权服务，可以用Oculus等一些著名的ID进行登录。

Economy：经济

UDP：Unity Distribution Portal，让开发者可以访问其它三方Android商店。

GameFoundation：

### 基础服务

Cloud Code：云服务，server-less

CloudSave：云存储

RemoteConfig：类似字节的TCC，存储远程配置。

CCD：Cloud Content Dilivery，云内容，例如直播流、视频流等

Analytics

Analytics Library：Unity的埋点、客户端分析服务

WebGL Publisher：将webgl导出的东西一键部署到远端，类似一个静态站点。

### IAP

因为Unity对IAP投入力度非常大，涉及模块较多，因此单独讨论。

小米SDK：小米商店的支付SDK

In App Purchasing

### 多人游戏

Lobby：多人游戏

MultiPlayerHLAPI：多人游戏高级API

Netcode For Entities：用于DOTS的多人游戏

Relay：中继

## XR

XR部分有很多公司的XR 开发插件。

- AR foundation

- ARCore XR Plugin：苹果的XR工具包

- ARKit Face Tracking

- ARKit XR Plugin

- MagicLeap XRPlugin

- Oculus XR Plugin

- OpenXR Plugin

- Windows XR Plugin：提供了Unity XR SDK的实现，允许使用windows 混合现实设备。

- XR Interaction Toolkit

- XR Plugin Management

- MockHMD XR Plugin

## 文件格式

- FBX Exporter：允许将场景导出为fbx文件，然后再AutoDesk、maya、3dmax等软件中编辑，然后导回到unity。

- Alembic：支持.abc文件，这是一种动画传输文件。

## 移动端

Android Logcat：用于在UnityEditor中展示来自Android设备的日志。可以在Window/Analysize/Android Logcat中查看。

DeviceSimulator:它是过去Game窗口的替代者，Game窗口的缺点就是尺寸无法模拟真实设备的尺寸。

## 编辑器工具

QuickSearch：在Assets、场景、菜单、包、API、配置中搜索，相当于Unity中的SearchEverything

EditorCoroutines：在Unity Editor中构建类似MonoBehavior的对象。

Unity Recorder：在编辑器的Play模式下允许录制动画、音频、视频等。

## 测试

ProfileAnalyzer：性能分析工具

MemoryProfiler：内存分析工具

TestFramework：包括一个UI和一个测试框架。

Code Coverage：代码覆盖率统计

## 脚本编辑器

JetBrains Rider Editor

Visual Studio Editor

Visual Studio Code Editor

## 编辑器

Unity是一系列子编辑器的集合，所谓游戏引擎就是游戏开发工具集合，而游戏开发工具就是地形编辑、场景编辑、UI编辑、时间线编辑、shader编辑、动画编辑。

PolyBrush：MeshPaiting、Sculpting，集合绘制工具

Timeline：时间线工具。

UI Builder：拖拽形式构建UI

Terrain Tools：地形工具

ShaderGraph：可以无需编程编辑shader。

ProBuilder：构建、编辑特定几何形状。

Scriptable BuildPipeline：将AssetBundle移动到C#中进行。

Cinemachine：专业的摄像机

- VisualEffectGraph：视觉效果图，可以用于编辑视觉效果。

- PostProcessing：有一系列特效和图片过滤器，可以直接用在摄像机上，来提升游戏视觉效果。

- Animation Rigging：Animation Rigging工具箱。

## 渲染管线RP

- Core RP

- HighDefinition RP

- UniversalRP

## DOTS

HavokPhysics for Unity，用于DOTS系统的物理引擎。

Jobs

Burst：将IL字节码转成高度优化的二进制码，基于LLVM。

## 其它

InputSystem

Kinematica：下一代动画库

Mathematics：提供类似shader数学函数的语法。

AI navigation

ML Agents：基于机器学习的角色行为

Addressables：资源管理

MobileNotification：在Android、IOS上发送通知。

TextMeshPro：文字库。

VersionControl：版本管理，使用Plastic SCM。

Unity UI：像GameObject一样的UI。

Newtonsoft.JSON因为使用过于广泛，从Unity2020开始，已经成为Unity的标准JSON库了。
