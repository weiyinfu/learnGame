# InputSystem
[InputSystem API文档](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/index.html)  

学习资料，在PackageManager中添加InputSystem的Samples。  
# 如果启用了新版输入系统，还使用旧版输入系统就会报错
```plain
InvalidOperationException: You are trying to read Input using the UnityEngine.Input class, but you have switched active Input handling to Input System package in Player Settings.
02-28 16:20:22.460 14806 31977 E Unity   :   at (wrapper managed-to-native) UnityEngine.Input.GetKeyDownInt(UnityEngine.KeyCode)
```

# 关键类
* InputAction
* InputBindings
* InputActionPhase表示InputAction的五种阶段。分别为：Canceled, Disabled, Performed, Started, Waiting
* InputActionAssets：是一种存储动作绑定关系的资源文件，扩展名为.InputActions，数据格式为 JSON。可以通过 Project 页面中 Create -> Input Actions 来创建一个新的 Input Action Assets。
* PlayerInput

# 使用示例
## 直接获取当前输入设备的状态
```plain
public void Update()
{
    var gp = Gamepad.current;
    if (gp == null) return;
    
    Vector2 leftStick = gp.leftStick.ReadValue(),
            rightStick = gp.rightStick.ReadValue();
    
    if (gp.buttonSouth.wasPressedThisFrame) Debug.Log("Pressed");
    if (gp.buttonSouth.wasReleasedThisFrame) Debug.Log("Released");
    
    //...
}
```
## 使用PlayerInput组件
```plain
public class Player : MonoBehaviour
{
    // Unity Event的情况，需要在Inspector中进行本函数的订阅操作
    public void OnAttack(InputAction.CallbackContext callback)
    {
        switch (callback.phase)
        {
            case InputActionPhase.Performed:
                Debug.Log("Attacking!");
        }
        move = callback.ReadValue<Vector2>();
    }
    
    // C# 事件的情况，与上方情况不共存
    private PlayerInput input;
    private Vector2 move;
    private void Awake()
    {
        // 添加订阅者函数, 当然也可以写成独立的函数
        input = GetComponent<PlayerInput>().onActionTriggered += 
            callback =>
        {
            if (callback.action.name == "Move")
            {
                move = callback.ReadValue<Vector2>();
            }
        };
        
        // ...
    }
}
```

## 直接使用InputAction，不让PlayerInput赚差价
```plain
public class Player : MonoBehaviour
{
    public InputAction moveAction;
    
    public void OnEnable()
    {
        moveAction.Enable();
    }
    
    public void OnDisable()
    {
        moveAction.Disable();
    }
    
     // 此时我们自定义的InputAction即可正常使用
    public void Update()
    {
        var move = moveAction.ReadValue<Vector2>();
        
        //...
    }
}

```

# 代码示例
```plain
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                keydown = true;
                Up();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                keydown = true;
                Down();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                keydown = true;
                Left();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                keydown = true;
                Right();
            }
```

# 使用宏处理新旧输入系统
```plain
 #if ENABLE_INPUT_SYSTEM
     // New input system backends are enabled.
 #endif
 
 #if ENABLE_LEGACY_INPUT_MANAGER
     // Old input backends are enabled.
 #endif
```