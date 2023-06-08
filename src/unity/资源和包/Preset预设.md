预设和Prefab是Unity中简化操作的两大利器，它们本质上就是一套模板。  

Prefab=GameObject的模板。 
预设=MonoBehavior的模板。  

Unity的GameObject包含很多个组件，每个组件都有一些public的属性，可以公开设置。  
当一个组件的属性非常多的时候，设置起来就很麻烦，这时候可以使用Prefab。  
除了Prefab，也可以使用Preset（预设）。预设就是组件的一组配置方式。  
如下图所示，点击XR Controller组件的问号后面的那个预设按钮，可以选择预设。也可以将当前配置保存为预设。  
![img.png](res/preset.png)


预设相比Prefab更为轻量，prefab一定会引入新的GameObject，而预设则只改一个组件的成员值而不改变GameObject。  

每个预设相当于一个工厂方法
```plain
MonoBehavior preset1(){
    ...
}
MonoBehavior preset2(){
    ...
}
```

预设相当于Vue中的Mixin，可以将预设文件设置为某个MonoBehavior的默认设置。
一个预设只能更改一个MonoBehavior。  

# 如何保存Preset
在一个MonoBehavior上面点击右上角的Preset按钮，可以为该组件导入预设。在该浮窗底部有一个Save Current Preset，点击可以将当前MonoBehavior保存在一个预设。  
