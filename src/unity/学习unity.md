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
# unity三体运动

# unity俄罗斯方块

# mono behavior的回调
Awake：当一个脚本实例被载入时Awake被调用。我们大多在这个类中完成成员变量的初始化
Start：仅在Update函数第一次被调用前调用。因为它是在Awake之后被调用的，我们可以把一些需要依赖Awake的变量放在Start里面初始化。 同时我们还大多在这个类中执行StartCoroutine进行一些协程的触发。要注意在用C#写脚本时，必须使用StartCoroutine开始一个协程，但是如果使用的是JavaScript，则不需要这么做。
Update：当MonoBehaviour启用时，其Update在每一帧被调用。
FixedUpdate：当MonoBehaviour启用时，其 FixedUpdate 在每一固定帧被调用。
OnEnable：当对象变为可用或激活状态时此函数被调用。
OnDisable：当对象变为不可用或非激活状态时此函数被调用。
OnDestroy：当MonoBehaviour将被销毁时，这个函数被调用。