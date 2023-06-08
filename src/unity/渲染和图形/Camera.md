# 透视相机和正交相机 
Unity支持两种摄像机 透视相机和正交相机  
透视相机：符合一般视觉规律，即近大远小、近清晰远模糊。相机像一个点一样观察世界。    
正交相机：显示的对象不随距离变远而缩小的摄像机称为正交摄像机  

# 获取camera
* 使用Camera.main;
* 使用名称获取Camera
    ```
    Camera mainCamera;//使用public字段，在Unity Editor里面绑定
    GameObject gameObject=GameObject.Find("Main Camera");
    mainCamera=gameObject.GetComponent<Camera>();
    ```
* 使用ObjectType获取Camera（推荐）
    ```
    Camera mainCamera = FindObjectOfType<Camera>();
    ```

# 相机的关键参数
camera的位置和尺寸
* camera.transform.posision//获取位置
* camera.rect//视区的尺寸

相机的大小有两种设置方式，这两种方式其实是等价的。一种方式是设置相机到画布中心的距离，另一种方式是设置视区的高度。  
* 视区：viewport，表示相机能够看到的最远的那个面，一个四面椎体内的东西都是可见的。如果是2d应用，则视区是一个平面矩形。
* aspect：宽高比，宽度除以高度。宽高比是系统参数，程序无法改变这个参数。
* orthographicSize：视区的高度。程序可以改变这个参数。  

屏幕的大小：
* Screen.width
* Screen.height

设置完orthographicSize之后，视区是一个坐标系。水平方向为x轴，竖直方向为y轴。x的范围是`[-width,width]`，其中`width=orthographicSize*aspect`。  

# 画布字太小
更改画布的Canvas Scaler：设置为Scale With Screen Size。随着屏幕大小缩放画布。


# Unity提供了几种光源，分别是什么
答：
四种。  
平行光：Directional Light  
点光源：Point Light  
聚光灯：Spot Light  
区域光源：Area Light  


# 使用RayCast找到游戏对象
```
Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
if (hit.collider !=null) {
    Debug.Log (hit.collider.gameObject.name);
```

# 坐标转换
Camera下面有8个与点和坐标转换相关的函数：
* Vector3 WorldToScreenPoint(Vector3 position)
* Vector3 WorldToViewportPoint(Vector3 position) 
* Vector3 ViewportToWorldPoint(Vector3 position) 
* Vector3 ViewportToScreenPoint(Vector3 position)
* Vector3 ScreenToWorldPoint(Vector3 position)
* Vector3 ScreenToViewportPoint(Vector3 position)
* Ray ViewportPointToRay(Vector3 pos)
* Ray ScreenPointToRay(Vector3 pos, Camera.MonoOrStereoscopicEye eye)

[](./res/unity坐标转换.jpg)  

其中涉及四个概念：
* ScreenPoint：是一个二维点，表示屏幕上的坐标
* ViewportPoint：是一个二维点，与摄像机的ViewportRect有关，是一个相对于摄像机的坐标系
* WorldPoint：一个二维点，与摄像机有关。  
* Ray：一束光线

ViewportPoint是一个以左下角为原点的一个矩形，ViewportPoint的x和y的坐标取值都是0到1之间。  
WorldPoint是一个以中心为原点的坐标系，在2d中，z的取值为相机的z坐标，xy为相机的视区坐标。  

# ClearFlags：重绘标志
什么叫重绘标志？相机所看到的是画布，画布每帧重绘的时候需要执行Clear()清空上一帧的东西。  
ClearFlags表示重绘之前应该如何处理画布。
* Skybox：天空盒，每帧重绘的时候，首先把天空盒画上去。
* SolidColor：纯色，每帧重绘的时候，使用纯色绘制。  
* DepthOnly：仅深度
* Don't Clear：不清除，表示每帧绘制的时候不执行Canvas.Clear()操作。如果有一个旋转的物体，则会看到这个物体的重影。  

# Culling Mask：
Culling：宰杀，部分捕杀  
Culling Mask：选择性让相机渲染哪些Layer的物体

## Layer：层级
Unity的场景是由GameObject组成的，每个GameObject可以指定Layer。其中前七个Layer是Unity内置的。
* nothing：什么都没有，这个层空空如也
* everything：什么都有，这个层包括全部的GameObject
* default
* transparent FX
* ignore raycast
* water
* UI

一个相机的CullingMask是一个int数字，所以Unity最多支持32个层。
```plain
1.用于只渲染某一层
_camera.cullingMask = 1<<8; //cube 只渲染第八层
_camera.cullingMask = 1<<9; //sphere 只渲染第九层
_camera.cullingMask = 1<<10; //capsule 只渲染第十层
只渲染第8、9、10层
_camera.cullingMask = (1 << 10) + (1<<9) +(1<<8);

2.渲染所有层
_camera.cullingMask = -1; //对应 everything

3.任何层都不渲染
_camera.cullingMask = 0; //对应 nothing

4.在原来基础添加某一层
_camera.cullingMask |= (1 << 10); //在原来的基础上增加第10层

5.在原来基础减去某一层
_camera.cullingMask &= ~(1 << 10); //在原来的基础上减掉第10层
6.渲染除了某一层外的所有层
_camera.cullingMask = ~(1 << 10); //渲染除第10层之外的其他所有层
**
```
# 场景设置天空盒
在Window/Render/Lightning打开窗口，设置Scene/Environment，选择SkyboxMaterial。  
创建Material，设置Shader为Skybox下面的6面体模式，使用6张图片作为背景图。   

