# 官方文档

https://docs.unity3d.com/Packages/com.unity.inputsystem@1.4/manual/UISupport.html

# 作用

两套输入系统

- 旧版：InputManager

- 新版：InputSystem

在2022 editor中已经将新版的InputSystem作为默认配置。如果想在2019的editor中使用，则需要在PackageManager中手动添加InputSystem这个依赖。

目前来看，InputSystem是未来，尽量使用InputSystem。

在代码中使用的时候，

替换InputManager，对应UnityEngine.Input类。

# 三种用法

## 通过输入设备直接读取输入

读取gamepad。其它的可以使用：Keyboard.current,Mouse.current.

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class MyPlayerScript : MonoBehaviour
{
    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return; // No gamepad connected.if (gamepad.rightTrigger.wasPressedThisFrame)
        {
            // 'Use' code here
        }

        Vector2 move = gamepad.leftStick.ReadValue();
        // 'Move' code here
    }
}
```

## 通过InputAction读取输入

1. 添加一个PlayerInput组件。每个PlayerInput代表游戏中的一个玩家。

2. 为PlayerInput创建Actions，创建名为.inputactions的文件，将其关联到PlayerInput组件。

3. 编辑Action的Response
   
   1. SendMessages
   
   2. Broadcast Messages
   
   3. Invoke Unity Events
   
   4. Invoke C Sharp Events

# 常用技巧

当前帧是否按下了空格键

```Python
Keyboard.current.space.wasPressedThisFrame
```

找到所有的游戏手柄

```csharp
    var allGamepads = Gamepad.all;

    // Or more specific versions.var allPS4Gamepads = DualShockGamepadPS4.all;
    
    //方法二
    // Go through all devices and select gamepads.
    InputSystem.devices.Select(x => x is Gamepad);

    // Query everything that is using the gamepad layout or based on that layout.// NOTE: Don't forget to Dispose() the result.
    InputSystem.FindControls("<gamepad>");
```

找到玩家当前使用的手柄

```csharp
var gamepad = Gamepad.current;

    // This works for other types of devices, too.
    var keyboard = Keyboard.current;
    var mouse = Mouse.current;
```

监控设备变更

```csharp
InputSystem.onDeviceChange +=
        (device, change) =>
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    // New Device.break;
                case InputDeviceChange.Disconnected:
                    // Device got unplugged.break;
                case InputDeviceChange.Connected:
                    // Plugged back in.break;
                case InputDeviceChange.Removed:
                    // Remove from Input System entirely; by default, Devices stay in the system once discovered.break;
                default:
                    // See InputDeviceChange reference for other event types.break;
            }
        }
```

获取触摸屏的输入

```csharp
    using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

    public void Update(){
        foreach (var touch in Touch.activeTouches)
            Debug.Log($"{touch.touchId}: {touch.screenPosition},{touch.phase}");
    }
```

# InputAction

`var action=new InputAction("设备名称/设备按钮");`

一个action有许多状态：

- performed

- triggered

# UI系统与输入系统的交互器

EventSystem是一个GameObject，它包含UI与输入的事件转发。

旧版的InputManager的事件处理是StandaloneInputModule。

新版的InputSystem的事件处理是InputSystemUIInputModule。

如果使用XR，需要添加XR UI InputModule。



# UI Toolkit+XR+InputSystem=暂不支持

XR与UI Toolkit的结合使用上不支持，这意味着无法通过VR控制器设别操作使用UI Toolkit创建的界面。
