Unity通过两个独立的系统提供输入支持：
* 输入管理器InputManager是Unity核心平台的一部分，默认情况下就能使用。  
* 输入系统InputSystem，必须使用PackageManager安装才能使用，是Unity新版的主推的输入系统，它需要.net4 运行时。

在 Unity 设计之初，并没有预见到现在如此丰富的平台和设备支持规模，所以一开始设计的输入系统 UnityEngine.Input 使用起来并不舒适，在多设备和多平台输入处理时显得力不从心且不够优雅，甚至连游戏中热插拔手柄这种操作都显得十分臃肿和复杂。Unity Tec 从 2016 年起开始逐步开发新一代输入系统 UnityEngine.InputSystem ，到今天（Unity 2019.3.0f6），已经迭代到了 1.0.0-preview.4 版本。官方表示大概在 Unity 2020 版本推出新输入系统的正式版，但是旧输入系统的下架时间并没有被明确。

新输入系统基于事件，输入设备和动作逻辑互相分离，通过配置映射来处理输入信息。具有易用，多设备多平台协调一致的特点。现在已经可以在 Package Manager 中安装使用了。

Unity的这两种输入系统是互斥的，要想更改只能重启。Unity的XR Interaction Toolkit也有最新版，新版的特点就是鼓励使用基于Action的输入，不鼓励直接访问设备的输入，这种理念与InputSystem是相似的。我认为InputManager和Devices-Based XR输入系统更具灵活性并且容易上手。    

<https://docs.unity3d.com/Packages/com.unity.inputsystem@1.4/manual/Migration.html>

# 如何避免InputSystem频繁弹窗？
新版的输入系统InputSystem需要通过PackageManager安装，只有在安装了InputSystem的情况下才会弹窗，要想避免弹窗，直接在Pacakge/manifest里面把InputSystem删掉即可。   

# 如何切换到InputSystem？
1. 打开PacakgeManager添加InputSystem包，一旦导入这个包，之后每次打开Unity如果没有启用新输入系统都会提示是否启用新的输入系统。
2. 在PlayerSettings里面设置输入为新的InputSystem
3. 把各个场景中的EventySystem按照提示启用新输入系统的MonoBehavior。  
4. 对于类似InGameDebugConsole等一些库，需要手动添加引用，解决编译报错。

# 输入涉及到的API
* Input的静态方法系列
* 移动设备使用Input.touches

    ```plain
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
    ```
* VR、XR的InputDevices系列