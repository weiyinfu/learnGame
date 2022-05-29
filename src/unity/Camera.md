# 获取camera
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
* WordPoint：一个二维点，与摄像机有关。  
* Ray：一束光线

ViewportPoint是一个以左下角为原点的一个矩形，ViewportPoint的x和y的坐标取值都是0到1之间。  
WorldPoint，在2d中，z位相机的z坐标，xy为相机的视区坐标，以中心为原点。  