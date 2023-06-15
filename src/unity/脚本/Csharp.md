# awemesome.net

https://dotnet.libhunt.com/categories



# C#代码

C#字节数组

```
MemoryStream stream = new MemoryStream();
using (BinaryWriter writer = new BinaryWriter(stream))
{
    writer.Write(myByte);
    writer.Write(myInt32);
    writer.Write("Hello");
}
byte[] bytes = stream.ToArray();
```

C#创建随机数组

```
int Min = 0;
int Max = 20;
Random randNum = new Random();
int[] test2 = Enumerable
    .Repeat(0, 5)
    .Select(i => randNum.Next(Min, Max))
    .ToArray();
```



# 读写文件

```
            if (! Directory.Exists(C:\Users\Administrator\123))     // 返回bool类型，存在返回true，不存在返回false
            {
                Directory.CreateDirectory(C:\Users\Administrator\123);      //不存在则创建路径
            }

           if (! File.Exists(C:\Users\Administrator\123\1.txt))        // 返回bool类型，存在返回true，不存在返回false                                     
            {
                 File.Create(C:\Users\Administrator\123\1.txt);         //不存在则创建文件
            }
```

IntPtr运算符

```
/// For passing to native C
public static explicit operator IntPtr(RtcGetTokenOptions options)
{
    return options != null ? options.Handle : IntPtr.Zero;
}
```

# 静态类的优点

静态类只能包含静态成员。您不能为静态类创建对象。

1. 如果将任何成员声明为非静态成员，则会收到错误消息。

2. 当您尝试为静态类创建实例时，它会再次生成编译时错误，因为可以使用其类名直接访问静态成员。

3. 在类定义中，在 class 关键字之前使用 static 关键字来声明静态类。

4. 静态类成员通过类名后跟成员名来访问。

# unity多线程插件loom

一个队列，用于普通线程和UI线程之间的通信。

# unity使用toml

https://github.com/dezhidki/Tommy/blob/master/Tommy/Tommy.cs

https://lab.uwa4d.com/lab/62839f03a8103dabd055548d

https://github.com/toml-lang/toml

# parent

dont use xx.parent=xxx,use setParent

userObj.transform.SetParent(friendContent.transform, true);
}

# UI操作只能在协程进行

11-17 10:54:24.090 28748 28909 E Unity : Trying to add RawImageHeadImage (UnityEngine.UI.RawImage) for graphic rebuild while we are already inside a graphic rebuild loop. This is not supported.

使用HTTP请求ContinueWith会有另一个协程，这个协程与主线程还是有区别的。

创建一个MonoBehavior，它有一个队列，把需要执行的函数放到这个队列里面。



# LineRenderer

# Matrix4x4

# unity竟然不支持cylinder collider

圆柱体在三维空间中就会不停地滚动，根本停不下来
