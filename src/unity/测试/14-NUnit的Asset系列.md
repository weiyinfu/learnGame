NUnit有两种模式的Assert，一种模式是That模式，这种模式的特点是非常灵活；另一种模式是函数式，有很多个小函数，这些小函数的实现其实都是基于That模式。小函数模式使用起来更加直观简洁。  

有一些断言只能使用That实现。
```csharp
int[] array = { 1, 2, 3 };
Assert.That(array, Has.Exactly(1).EqualTo(3));
Assert.That(array, Has.Exactly(2).GreaterThan(1));
Assert.That(array, Has.Exactly(3).LessThan(100));
```

有一些断言使用小函数和That都能实现，That的功能是小函数功能的超集。 
```cshrap
Assert.AreEqual(4, 2 + 2);
Assert.That(2 + 2, Is.EqualTo(4));
```

# 常用的断言类
* Assert：最基础的断言
* StringAssert：与字符串这种类型相关的断言
* CollectionAssert：集合断言
* FileAssert：文件断言
* DirectoryAssert：目录断言

如果自定义的类想要实现统一的断言，类的命名上可以使用XXXAssert的形式。  
