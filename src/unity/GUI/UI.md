# Unity的UI系统

Unity官方支持三套UI系统。

* UI Toolkit：也叫UIElements，是Unity最新的UI系统，Unity有意将其作为默认的UI系统，但是目前不够完善。和web、android、javafx、WPF等技术一样，它使用XML定义UI树，使用CSS定义样式。
* uGUI：是一个较旧的，使用GameObject的UI系统。它最大的优势就是支持拖拽式创建UI界面、支持3D，像处理普通的GameObject那样处理UI元素。
* IMGUI：Instant Mode GUI，是一个代码驱动的UI工具包。IMGUI就是著名的`data=GUI(data)`
  这种模式的API，在Unity中用于编写Inspector和EditorWindow。

两种UI模式：

* 运行时UI，用于游戏内
* GUI扩展UI，用于编写Editor扩展

uGUI只能用于游戏内，IMGUI只能用于编辑器扩展，而UI Toolkit统一了Unity的UI，既能用于游戏内又能用于GUI扩展。  
Unity中经常存在一种东西，多种实现方案。例如UI系统包括UI toolkit、uGUI、IMGUI等。输入系统包括InputSystem和InputManager。

# 在使用UnityEditor做工具的时候选择哪种UI？

首先，可选项只有IMGUI和UI Toolkit这两种方案，uGUI只能在运行时使用。   
IMGUI的优势在于只需要考虑第一次渲染的时候的界面设计，之后每次更改都会触发重新渲染。这种反复重绘的方式优点就是响应灵活，缺点是性能比较差。然而，性能差这个缺点实在算不上什么，因为在Unity Editor里面UI通常很小，这种性能损耗算不上什么。许多新兴的UI库都是这种模式，例如react、flutter等，都是基于重绘实现的。基于组件实现的缺点就是不灵活。      
IMGUI的响应可以做到更及时。例如一个配置，可以在两个窗口内都能进行修改。如果同时打开两个窗口，期望是在一个窗口中的改动可以很快在另一个窗口中看到变化。如果使用UI
Toolkit，就需要处理OnFocus之类的事件，在这些事件里面对控件进行手动修改。  
再比如，一个窗口里面有一个中英文切换按钮，点击中文之后所有控件的文本变成中文，点击英文之后，所有空间的文本变成英文。如果使用IMGUI，轻而易举就能实现；如果使用UI
Toolkit，则需要手动处理每个空间的显示。界面上有多少个控件，点击按钮的时候就需要操作多少个控件，代码写起来复杂度较高。  
另外，IMGUI存在了很长时间，相对成熟一些，UI Toolkit的PropertyField对于列表类型的数据展示较为原始，没有排序、没有添加和删除按钮，只能更改列表长度。

那么UI Toolkit有什么优点呢？style控制相对灵活，可以为每个元素精准设置样式，对于熟悉CSS的人非常友好。  

IMGUI是函数式编程，UI Toolkit是面向过程编程。函数式编程操作值，面向过程编程操作对象。  

一言以蔽之，IMGUI写起来爽，用起来慢；UI Toolkit看上去清晰，写起来麻烦，用起来快。  
如果做的UI需要多次渲染，那么使用IMGUI；如果做的UI只需要静态展示，使用UI Toolkit。

# UGUI和NGUI的区别

UGUI是unity官方推出的UI库，NGUI是社区实现的改进版的UI，经过一段时间发展，UGUI基本上超越了NGUI。以后不需要再学NGUI了。

1.UGUI界面展示是在画布下(Canvas)，而NGUI是在UIRoot下

2.UGUI继承RectTransform，RectTransform继承Transform，而Ngui直接继承Transform

3.UGUI没有图集Atlas，是直接使用图片，而Ngui需要使用图集，对图集进行管理和维护

4.UGUI有锚点，可以自动适配屏幕，NGUI没有暂未发现此功能

5.UGUI中Btn需要有sprite，button，而NGUI只需要一个UIButton方法，和一个BoxCollider。

6.NGUI基于C#编写的，会产出比较多的GC，UGUI是基于C++，性能比较好。基于canvas渲染比较好。

# unity富文本TMP

在window/textMeshPro中通过import可以导入富文本的资源和实例。
TMP：text mesh pro，是高级版的富文本框。可以灵活地控制字体。
它在ProjectSettings中都有专门的TextMeshPro部分进行设置，在Window菜单中也有专门的入口，因此TMP是比较重要的功能。

![img_5.png](../res/img_5.png)