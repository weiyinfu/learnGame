# 参考资料

https://docs.unity3d.com/cn/2022.2/Manual/class-InputManager.html

# 学习路线

1. 了解Unity在PC端如何获取鼠标键盘的输入事件，了解Input

2. 了解Unity如何获取XR的输入事件，了解InputDevices

3. 了解XR Interaction Toolkit，了解Unity的输入的高阶用法

# 名词解释

HMD：头戴，Head Mounted Display

# Unity的两套独立的输入系统

InputManager：旧版的输入系统

InputSystem：新版的输入系统

输入系统不影响API的使用。

Unity 支持来自多种输入设备的输入，包括：

- 键盘和鼠标

- 游戏杆

- 控制器

- 触摸屏

- 加速度计或陀螺仪等移动设备的运动感应功能

- VR 和 AR 控制器

```Java
float horizontalInput = Input.GetAxis ("Horizontal");
float moveSpeed = 10;
//定义对象移动的速度。

float horizontalInput = Input.GetAxis("Horizontal");
//获取水平输入轴的数值。

float verticalInput = Input.GetAxis("Vertical");
//获取垂直输入轴的数值。

transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * moveSpeed * Time.deltaTime);
//将对象移动到 XYZ 坐标，分别定义为 horizontalInput、0 以及 verticalInput。
```

移动端输入：触碰点

```Java
    void Update()
    {
        foreach(Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // 从当前触摸坐标构造一条射线
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray))
                {
                    // 如果命中，则创建一个粒子
                    Instantiate(particle, transform.position, transform.rotation);
                }
            }
        }
    }
```

加速度计

```Java
using UnityEngine;

public class Accelerometer : MonoBehaviour
{
    float speed = 10.0f;

    void Update()
    {
        Vector3 dir = Vector3.zero;
        // 我们假设设备与地面平行，
        // 主屏幕按钮位于右侧

        // 将设备的加速度轴重新映射到游戏坐标：
        // 1) 设备的 XY 平面映射到 XZ 平面
        // 2) 绕 Y 轴旋转 90 度

        dir.x = -Input.acceleration.y;
        dir.z = Input.acceleration.x;

        // 将加速度矢量限制为单位球体
        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        // 使其每秒移动 10 米而不是每帧 10 米...
        dir *= Time.deltaTime;

        // 移动对象
        transform.Translate(dir * speed);
    }
}
```

# XR输入系统

Unity的输入系统一般只通过Input这个类就能够实现。

但是对于XR，目前推荐使用InputDevices这个类作为入口类。

Unity的Input、XR.InputTracking这两个类也能够获取XR输入，但是不鼓励使用，尽量使用InputDevices，这样语义更明确、语法更简洁。Input的缺点在于没法以向量的形式获取数据。

以Input为例，要想获取primaryAxis，就需要Input.GetAxis(1),Input.GetAxis(2)，这样才能获取左手的摇杆位置；Input.GetAxis(4),Input.GetAxis(5)才能够获取右手的摇杆位置。

```Java
primary2DAxis [(1,2)/(4,5)]
```

# InputDevices

## InputDevices

### 静态

InputDevices的静态函数只有一个目标：获取InputDevice。它提供了几种检索InputDevice的方式

- 通过XRNode获取，这也是最常用的方式

- 通过Role获取，对应枚举InputDeviceRole

- 通过Characteristics获取，对应枚举InputDeviceCharacteristics

- GetDeviceAtXRNode：根据XRNode获取InputDevice

- GetDevicesAtRole：根据Role获取设备

- GetDevicesWithCharacteristics：根据设别的特性获取设备。

### 成员

- DeviceRole：设备的角色，InputDeviceRole

- TryGet+（InputFeatureType中的数据类型）：从设备读取数据

事件：

- 设备连接

- 设备断连

- 设备配置发生变化

### 获取全部的可用设备

```Java
var inputDevices = new List<UnityEngine.XR.InputDevice>();
UnityEngine.XR.InputDevices.GetDevices(inputDevices);

foreach (var device in inputDevices)
{
    Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
}
```

## XRNode

- 左眼

- 右眼

- 中心眼

- 头

- 左手

- 右手

- 游戏控制器

- 跟踪参照物

- 硬件跟踪器

XRNode跟InputDeviceRole基本上是同一类东西。

## InputDeviceCharacteristics

一个设备可能包含多种特性，使用一个int值表示一个设备的特性，例如HeldInHand|Left表示左右持有的设备。就像使用rwx表示一个文件的特性一样。

- HeadMounted

- Camera

- HeldInHand

- HandTracking

- EyeTracking

- TrackedDevice

- Controller

- TrackingReference

- Left

- Right

## InputDeviceRole

输入设备的角色，包括：

- 左手持有

- 右手持有

- 游戏控制器

- 跟踪参照物

- 硬件跟踪器

```Java
public enum InputDeviceRole : uint
{
  /// <summary>
  ///   <para>This device does not have a known role.</para>
  /// </summary>
  Unknown,
  /// <summary>
  ///   <para>This device is typically a HMD or Camera.</para>
  /// </summary>
  Generic,
  /// <summary>
  ///   <para>This device is a controller that represents the left hand.</para>
  /// </summary>
  LeftHanded,
  /// <summary>
  ///   <para>This device is a controller that represents the right hand.</para>
  /// </summary>
  RightHanded,
  /// <summary>
  ///   <para>This device is a game controller.</para>
  /// </summary>
  GameController,
  /// <summary>
  ///   <para>This device is a tracking reference used to track other devices in 3D.</para>
  /// </summary>
  TrackingReference,
  /// <summary>
  ///   <para>This device is a hardware tracker.</para>
  /// </summary>
  HardwareTracker,
  /// <summary>
  ///   <para>This device is a legacy controller.</para>
  /// </summary>
  LegacyController,
}
```

## 触觉

Haptic是触觉的意思，有些游戏手柄支持触觉。

```Java
List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>(); 

UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, devices);

foreach (var device in devices)
{
    UnityEngine.XR.HapticCapabilities capabilities;
    if (device.TryGetHapticCapabilities(out capabilities))
    {
            if (capabilities.supportsImpulse)
            {
                uint channel = 0;
                float amplitude = 0.5f;
                float duration = 1.0f;
                device.SendHapticImpulse(channel, amplitude, duration);
            }
    }
}
```

# XR Input Tracking：Unity旧版XR输入接口

## InputTracking

Unity.Xr.InputTracking是Unity旧版的输入入口类。

### 设备事件

输入设备的跟踪有四个事件

- trackingAcquired：获取设备跟踪

- trackingLost：设备丢失

- nodeAdded：设备添加

- nodeRemoved：设备移除

### 读取数据

- 获取XRNode的位置

- 获取XRNode的Rotation

- 获取XRNode的DeviceId

从InputTracking中读取数据已经不鼓励使用了，请使用InputDevices。例如获取设备地址。

```Java
[NativeConditional("ENABLE_VR", "Vector3f::zero")]
[Obsolete("This API is obsolete, and should no longer be used. Please use InputDevice.TryGetFeatureValue with the CommonUsages.devicePosition usage instead.")]
public static Vector3 GetLocalPosition(XRNode node)
{
  Vector3 ret;
  InputTracking.GetLocalPosition_Injected(node, out ret);
  return ret;
}
```

## XRNodeState

XNode：结点的类型

AwailableTrackingData：可用的数据列表

位置

角度

速度

角速度

加速度

角加速度

tracked：是否正在跟踪

## AwailableTrackingData

枚举，描述一个XRNodeState可用的数据列表可以组合这个枚举，例如位置|旋转，表示这个XRNodeState可用的数据是位置和旋转。

- 位置

- 旋转

- 速度

- 角速度

- 加速度

- 角加速度

# InputFeatureUsage

## InputFeatureUsage

Unity引擎里面的类，定义了一个输入类型，可以描述所有的输入。它包含的字段：

- name：输入的名称

- UsageType：输入的类型

## InputFeatureType

```Java
internal enum InputFeatureType : uint
{
  Custom = 0,//Custom类型，使用byte数组表示
  Binary = 1,//bool
  DiscreteStates = 2,//uint,枚举状态
  Axis1D = 3,//一维浮点
  Axis2D = 4,//二维浮点：例如鼠标
  Axis3D = 5,//三维浮点
  Rotation = 6,//角度，Quaternion，四元数
  Hand = 7,//Hand类型
  Bone = 8,//Bone类型
  Eyes = 9,//眼睛
  kUnityXRInputFeatureTypeInvalid = 4294967295, // 0xFFFFFFFF，未定义的操作类型
}
```

## InputFeature中涉及到的结构体

Hand、Bone、Eyes

## Bone

- deviceId：设备ID

- featureIndex：featureID

- Position

- Rotation

- ParentBone：父骨骼

- ChildBones：List<Child>

## Eyes

眼睛有左眼和右眼：

- LeftEyes

- RightEyes

以下传感器是左眼和右眼都有的。

- EyeRotation

- EyePosition

- FixationPoint：修正点的位置

- OpenAmount：眼睛的睁开量

## Hand类型

Hand是一个Bone的封装，一个Hand包含一个RootBone和多个FingerBones

- 设备Id

- FeatureIndex

- RootBone：Bone

- FingerBones：List<Bone>返回手指骨骼列表

Unity支持的输入的类型：InputFeatureType。

- bool

- uint

- float

## CommonUsages

UnityEngine.XR中定义了一些InputFeatureUsage，与Pico XR SDK的InputFeatureUsage类似。

CommonUsage中定义的是比较通用的InputFeatureUsage，而PXR_Usage中存放的是Pico特有的InputFeatureUsage，这些featureUsage大多是与眼睛有关系。

```Java
/// <summary>
///   <para>Informs to the developer whether the device is currently being tracked.</para>
/// </summary>
public static InputFeatureUsage<bool> isTracked = new InputFeatureUsage<bool>("IsTracked");
/// <summary>
///   <para>The primary face button being pressed on a device, or sole button if only one is available.</para>
/// </summary>
public static InputFeatureUsage<bool> primaryButton = new InputFeatureUsage<bool>("PrimaryButton");
/// <summary>
///   <para>The primary face button being touched on a device.</para>
/// </summary>
public static InputFeatureUsage<bool> primaryTouch = new InputFeatureUsage<bool>("PrimaryTouch");
/// <summary>
///   <para>The secondary face button being pressed on a device.</para>
/// </summary>
public static InputFeatureUsage<bool> secondaryButton = new InputFeatureUsage<bool>("SecondaryButton");
/// <summary>
///   <para>The secondary face button being touched on a device.</para>
/// </summary>
public static InputFeatureUsage<bool> secondaryTouch = new InputFeatureUsage<bool>("SecondaryTouch");
/// <summary>
///   <para>A binary measure of whether the device is being gripped.</para>
/// </summary>
public static InputFeatureUsage<bool> gripButton = new InputFeatureUsage<bool>("GripButton");
/// <summary>
///   <para>A binary measure of whether the index finger is activating the trigger.</para>
/// </summary>
public static InputFeatureUsage<bool> triggerButton = new InputFeatureUsage<bool>("TriggerButton");
/// <summary>
///   <para>Represents a menu button, used to pause, go back, or otherwise exit gameplay.</para>
/// </summary>
public static InputFeatureUsage<bool> menuButton = new InputFeatureUsage<bool>("MenuButton");
/// <summary>
///   <para>Represents the primary 2D axis being clicked or otherwise depressed.</para>
/// </summary>
public static InputFeatureUsage<bool> primary2DAxisClick = new InputFeatureUsage<bool>("Primary2DAxisClick");
/// <summary>
///   <para>Represents the primary 2D axis being touched.</para>
/// </summary>
public static InputFeatureUsage<bool> primary2DAxisTouch = new InputFeatureUsage<bool>("Primary2DAxisTouch");
/// <summary>
///   <para>Represents the secondary 2D axis being clicked or otherwise depressed.</para>
/// </summary>
public static InputFeatureUsage<bool> secondary2DAxisClick = new InputFeatureUsage<bool>("Secondary2DAxisClick");
/// <summary>
///   <para>Represents the secondary 2D axis being touched.</para>
/// </summary>
public static InputFeatureUsage<bool> secondary2DAxisTouch = new InputFeatureUsage<bool>("Secondary2DAxisTouch");
/// <summary>
///   <para>Indicates whether the user is present and interacting with the device.</para>
/// </summary>
public static InputFeatureUsage<bool> userPresence = new InputFeatureUsage<bool>("UserPresence");
/// <summary>
///   <para>Represents the values being tracked for this device.</para>
/// </summary>
public static InputFeatureUsage<InputTrackingState> trackingState = new InputFeatureUsage<InputTrackingState>("TrackingState");
/// <summary>
///   <para>Value representing the current battery life of this device.</para>
/// </summary>
public static InputFeatureUsage<float> batteryLevel = new InputFeatureUsage<float>("BatteryLevel");
/// <summary>
///   <para>A trigger-like control, pressed with the index finger.</para>
/// </summary>
public static InputFeatureUsage<float> trigger = new InputFeatureUsage<float>("Trigger");
/// <summary>
///   <para>Represents the users grip on the controller.</para>
/// </summary>
public static InputFeatureUsage<float> grip = new InputFeatureUsage<float>("Grip");
/// <summary>
///   <para>The primary touchpad or joystick on a device.</para>
/// </summary>
public static InputFeatureUsage<Vector2> primary2DAxis = new InputFeatureUsage<Vector2>("Primary2DAxis");
/// <summary>
///   <para>A secondary touchpad or joystick on a device.</para>
/// </summary>
public static InputFeatureUsage<Vector2> secondary2DAxis = new InputFeatureUsage<Vector2>("Secondary2DAxis");
/// <summary>
///   <para>The position of the device.</para>
/// </summary>
public static InputFeatureUsage<Vector3> devicePosition = new InputFeatureUsage<Vector3>("DevicePosition");
/// <summary>
///   <para>The position of the left eye on this device.</para>
/// </summary>
public static InputFeatureUsage<Vector3> leftEyePosition = new InputFeatureUsage<Vector3>("LeftEyePosition");
/// <summary>
///   <para>The position of the right eye on this device.</para>
/// </summary>
public static InputFeatureUsage<Vector3> rightEyePosition = new InputFeatureUsage<Vector3>("RightEyePosition");
/// <summary>
///   <para>The position of the center eye on this device.</para>
/// </summary>
public static InputFeatureUsage<Vector3> centerEyePosition = new InputFeatureUsage<Vector3>("CenterEyePosition");
/// <summary>
///   <para>The position of the color camera on this device.</para>
/// </summary>
public static InputFeatureUsage<Vector3> colorCameraPosition = new InputFeatureUsage<Vector3>("CameraPosition");
/// <summary>
///   <para>The velocity of the device.</para>
/// </summary>
public static InputFeatureUsage<Vector3> deviceVelocity = new InputFeatureUsage<Vector3>("DeviceVelocity");
/// <summary>
///   <para>The angular velocity of this device, formatted as euler angles.</para>
/// </summary>
public static InputFeatureUsage<Vector3> deviceAngularVelocity = new InputFeatureUsage<Vector3>("DeviceAngularVelocity");
/// <summary>
///   <para>The velocity of the left eye on this device.</para>
/// </summary>
public static InputFeatureUsage<Vector3> leftEyeVelocity = new InputFeatureUsage<Vector3>("LeftEyeVelocity");
/// <summary>
///   <para>The angular velocity of the left eye on this device, formatted as euler angles.</para>
/// </summary>
public static InputFeatureUsage<Vector3> leftEyeAngularVelocity = new InputFeatureUsage<Vector3>("LeftEyeAngularVelocity");
/// <summary>
///   <para>The velocity of the right eye on this device.</para>
/// </summary>
public static InputFeatureUsage<Vector3> rightEyeVelocity = new InputFeatureUsage<Vector3>("RightEyeVelocity");
/// <summary>
///   <para>The angular velocity of the right eye on this device, formatted as euler angles.</para>
/// </summary>
public static InputFeatureUsage<Vector3> rightEyeAngularVelocity = new InputFeatureUsage<Vector3>("RightEyeAngularVelocity");
/// <summary>
///   <para>The velocity of the center eye on this device.</para>
/// </summary>
public static InputFeatureUsage<Vector3> centerEyeVelocity = new InputFeatureUsage<Vector3>("CenterEyeVelocity");
/// <summary>
///   <para>The angular velocity of the center eye on this device, formatted as euler angles.</para>
/// </summary>
public static InputFeatureUsage<Vector3> centerEyeAngularVelocity = new InputFeatureUsage<Vector3>("CenterEyeAngularVelocity");
/// <summary>
///   <para>The velocity of the color camera on this device.</para>
/// </summary>
public static InputFeatureUsage<Vector3> colorCameraVelocity = new InputFeatureUsage<Vector3>("CameraVelocity");
/// <summary>
///   <para>The angular velocity of the color camera on this device, formatted as euler angles.</para>
/// </summary>
public static InputFeatureUsage<Vector3> colorCameraAngularVelocity = new InputFeatureUsage<Vector3>("CameraAngularVelocity");
/// <summary>
///   <para>The acceleration of the device.</para>
/// </summary>
public static InputFeatureUsage<Vector3> deviceAcceleration = new InputFeatureUsage<Vector3>("DeviceAcceleration");
/// <summary>
///   <para>The angular acceleration of this device, formatted as euler angles.</para>
/// </summary>
public static InputFeatureUsage<Vector3> deviceAngularAcceleration = new InputFeatureUsage<Vector3>("DeviceAngularAcceleration");
/// <summary>
///   <para>The acceleration of the left eye on this device.</para>
/// </summary>
public static InputFeatureUsage<Vector3> leftEyeAcceleration = new InputFeatureUsage<Vector3>("LeftEyeAcceleration");
/// <summary>
///   <para>The angular acceleration of the left eye on this device, formatted as euler angles.</para>
/// </summary>
public static InputFeatureUsage<Vector3> leftEyeAngularAcceleration = new InputFeatureUsage<Vector3>("LeftEyeAngularAcceleration");
/// <summary>
///   <para>The acceleration of the right eye on this device.</para>
/// </summary>
public static InputFeatureUsage<Vector3> rightEyeAcceleration = new InputFeatureUsage<Vector3>("RightEyeAcceleration");
/// <summary>
///   <para>The angular acceleration of the right eye on this device, formatted as euler angles.</para>
/// </summary>
public static InputFeatureUsage<Vector3> rightEyeAngularAcceleration = new InputFeatureUsage<Vector3>("RightEyeAngularAcceleration");
/// <summary>
///   <para>The acceleration of the center eye on this device.</para>
/// </summary>
public static InputFeatureUsage<Vector3> centerEyeAcceleration = new InputFeatureUsage<Vector3>("CenterEyeAcceleration");
/// <summary>
///   <para>The angular acceleration of the center eye on this device, formatted as euler angles.</para>
/// </summary>
public static InputFeatureUsage<Vector3> centerEyeAngularAcceleration = new InputFeatureUsage<Vector3>("CenterEyeAngularAcceleration");
/// <summary>
///   <para>The acceleration of the color camera on this device.</para>
/// </summary>
public static InputFeatureUsage<Vector3> colorCameraAcceleration = new InputFeatureUsage<Vector3>("CameraAcceleration");
/// <summary>
///   <para>The angular acceleration of the color camera on this device, formatted as euler angles.</para>
/// </summary>
public static InputFeatureUsage<Vector3> colorCameraAngularAcceleration = new InputFeatureUsage<Vector3>("CameraAngularAcceleration");
/// <summary>
///   <para>The rotation of this device.</para>
/// </summary>
public static InputFeatureUsage<Quaternion> deviceRotation = new InputFeatureUsage<Quaternion>("DeviceRotation");
/// <summary>
///   <para>The rotation of the left eye on this device.</para>
/// </summary>
public static InputFeatureUsage<Quaternion> leftEyeRotation = new InputFeatureUsage<Quaternion>("LeftEyeRotation");
/// <summary>
///   <para>The rotation of the right eye on this device.</para>
/// </summary>
public static InputFeatureUsage<Quaternion> rightEyeRotation = new InputFeatureUsage<Quaternion>("RightEyeRotation");
/// <summary>
///   <para>The rotation of the center eye on this device.</para>
/// </summary>
public static InputFeatureUsage<Quaternion> centerEyeRotation = new InputFeatureUsage<Quaternion>("CenterEyeRotation");
/// <summary>
///   <para>The rotation of the color camera on this device.</para>
/// </summary>
public static InputFeatureUsage<Quaternion> colorCameraRotation = new InputFeatureUsage<Quaternion>("CameraRotation");
/// <summary>
///   <para>Value representing the hand data for this device.</para>
/// </summary>
public static InputFeatureUsage<Hand> handData = new InputFeatureUsage<Hand>("HandData");
/// <summary>
///   <para>An Eyes struct containing eye tracking data collected from the device.</para>
/// </summary>
public static InputFeatureUsage<Eyes> eyesData = new InputFeatureUsage<Eyes>("EyesData");
/// <summary>
///   <para>A non-handed 2D axis.</para>
/// </summary>
[Obsolete("CommonUsages.dPad is not used by any XR platform and will be removed.")]
public static InputFeatureUsage<Vector2> dPad = new InputFeatureUsage<Vector2>("DPad");
/// <summary>
///   <para>Represents the grip pressure or angle of the index finger.</para>
/// </summary>
[Obsolete("CommonUsages.indexFinger is not used by any XR platform and will be removed.")]
public static InputFeatureUsage<float> indexFinger = new InputFeatureUsage<float>("IndexFinger");
/// <summary>
///   <para>Represents the grip pressure or angle of the middle finger.</para>
/// </summary>
[Obsolete("CommonUsages.MiddleFinger is not used by any XR platform and will be removed.")]
public static InputFeatureUsage<float> middleFinger = new InputFeatureUsage<float>("MiddleFinger");
/// <summary>
///   <para>Represents the grip pressure or angle of the ring finger.</para>
/// </summary>
[Obsolete("CommonUsages.RingFinger is not used by any XR platform and will be removed.")]
public static InputFeatureUsage<float> ringFinger = new InputFeatureUsage<float>("RingFinger");
/// <summary>
///   <para>Represents the grip pressure or angle of the pinky finger.</para>
/// </summary>
[Obsolete("CommonUsages.PinkyFinger is not used by any XR platform and will be removed.")]
public static InputFeatureUsage<float> pinkyFinger = new InputFeatureUsage<float>("PinkyFinger");
/// <summary>
///   <para>Represents a thumbrest or light thumb touch.</para>
/// </summary>
[Obsolete("CommonUsages.thumbrest is Oculus only, and is being moved to their package. Please use OculusUsages.thumbrest. These will still function until removed.")]
public static InputFeatureUsage<bool> thumbrest = new InputFeatureUsage<bool>("Thumbrest");
/// <summary>
///   <para>Represents a touch of the trigger or index finger.</para>
/// </summary>
[Obsolete("CommonUsages.indexTouch is Oculus only, and is being moved to their package.  Please use OculusUsages.indexTouch. These will still function until removed.")]
public static InputFeatureUsage<float> indexTouch = new InputFeatureUsage<float>("IndexTouch");
/// <summary>
///   <para>Represents the thumb pressing any input or feature.</para>
/// </summary>
[Obsolete("CommonUsages.thumbTouch is Oculus only, and is being moved to their package.  Please use OculusUsages.thumbTouch. These will still function until removed.")]
public static InputFeatureUsage<float> thumbTouch = new InputFeatureUsage<float>("ThumbTouch");
```
