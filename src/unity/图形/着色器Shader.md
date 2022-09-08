# 什么是着色器？
着色器是用来渲染图形的一种技术，通过shader可以控制每一个像素的渲染方式，从而实现自定义显卡渲染画面的算法。

drawCall是CPU调用GPU执行一次绘制的命令。  
shader就是GPU流水线上一些高度可编程的阶段，我们可以通过Shader控制流水线中的渲染细节。

# shader怎么用
shader及其配置打包形成Material材质。材质赋予到三维模型上就可以输出了。  

三维模型等于形状+材质。材质的实现方式之一就是shader。贴图可以作为shader的输入，shader能够把贴图等一些原始资源整合起来。    
# 着色器分类
着色器有两种：顶点Shader和像素Shader。顶点着色器确定了多面体的顶点，像素着色器决定了多面体每个面的颜色。
* 顶点着色器：VertexShader。3D图形是由很多个三角面片组成的，顶点Shader就是计算每个三角面片上的顶点，为像素渲染做准备。
* 像素着色器（也叫片元shader、片段shader）：PixelShader、FragmentShader。以像素为单位，计算光照、颜色。

# 常用的着色语言
* openCV：GLSL，GL shading language
* directX：微软出品，HLSL，Hight Level Shading Language，高级着色语言
* nvidia：CG，C for Graphics，用于图形的C语言，兼容directX和openCV，它对GLSL和HLSL做了进一步封装，是微软和英伟达互协作在标准硬件光照语言的语法和语义上达成的一种一致性协议。unity就采用这种语言。


# ShaderLab
为了自定义渲染效果，我们往往需要和大量的文件和设置打交道，而Unity Shader为我们提供了一层抽象来封装这些打交道的细节，即Unity Shader。而我们和这层抽象打交道的语言就是ShaderLab。它使用一系列的语义块(比如Property, SubShader)来描述一个Unity Shader文件，Unity在背后会根据使用的平台来把这些结构编译成真正的代码和Shader文件，而开发者只需要和Unity打交道即可。 作者：2025AT https://www.bilibili.com/read/cv14408919 出处：bilibili

表面着色器：以牺牲性能的前提下方便了渲染编写，是对顶点/片元着色器的一种封装。

表面着色器的代码直接定义在了SubShader模块下。

顶点/片元着色器：顶点/片元着色器的代码需要定义在Pass语义块内，灵活性高，性能好，但代价是编写麻烦。

Unity Shader是用ShaderLab语言编写，但是在SubShader内部，我们需要嵌套CG/HLSL语言去定义渲染内容的细节。 

# Unity的shader
Unity中支持三种Shader，分别是：
* Standard Surface Shader：标准表面着色器
* Unlit Shader：无光照着色器
* Image Effect Shader：图片效果着色器

unity的着色器语言叫做shaderlab，其实就是CG（C for Graphics）。
* surface shader：是对vertexFragment Shader的封装，它实际上是对vertex/fragment shader的封装，只要学会了vertex/fragment shader就能够掌握serface shader。
* vertex 和fragment shader
* fixed function shader：是一种比较比较底层的shader，已经被淘汰了，完全没有学习的必要。  

综上，unity的shader虽然多，但是真正需要了解得只有vertex/fragment shader。

# Unity3D Shader分哪几种，有什么区别？
答：表面着色器的抽象层次比较高，它可以轻松地以简洁方式实现复杂着色。表面着色器可同时在前向渲染及延迟渲染模式下正常工作。
顶点片段着色器可以非常灵活地实现需要的效果，但是需要编写更多的代码，并且很难与Unity的渲染管线完美集成。
固定功能管线着色器可以作为前两种着色器的备用选择，当硬件无法运行那些酷炫Shader的时，还可以通过固定功能管线着色器来绘制出一些基本的内容。

# 写法
## Shader的名称
可以通过在Shader名字中加("/")来控制Unity Shader出现的位置：
比如："Custom/MyShader"在材质面板的位置就是Shader-Custom-MyShader

## Properties
Properties语义块中声明的属性是在Inspector面板可以直接调节的属性
声明格式：Name("Display name", PropertyType) = DefaultValue

## SubShader

一个Unity Shader文件中可以包含多个SubShader文件，但至少要有一个。Unity会选择第一个能够在目标平台上运行的SubShader，如果都不支持，会调用Fallback语义指定的SubShader。
SubShader中包含RenderSetup,Tags和Pass通道。

- RenderSetup用于设置逐片元操作中的流程，比如关闭深度写入，开启混合模式等。

- Tags则用于设置渲染方式： 

subshader是啥呢，算法，就是写给GPU渲染的shader片段了，这里记住，一个shader当中至少有一个subshader。每一次显卡进行处理的时候呢，只能选择其中一个subshader去执行。那为什么会有多个subshader呢？这和硬件有关。

在读取shader的时候，会先从第一个subshader读取，如果第一个能适配当前硬件，就不会往下读了；如果硬件太老跟不上，第一个读取不了，就会读取第二个看能不能与我适配。也就是说，subshader的所有方案会向下简化。如果这些列举的subshader都用不了怎么办？那就是第三个Fallback了。