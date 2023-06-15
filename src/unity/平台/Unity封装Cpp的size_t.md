需要封装的函数：

ppfUserHandle ppf_UserArray_GetElement(const ppfUserArrayHandle obj , size_t index);

# 分析

size_t是C++中的数据类型，在32位平台上是uint32，在64位平台上是uint64。



size_t映射到C#的备选项有：

- size_t=>ulong

- size_t=>uint

- size_t=>UIntPtr：跟随系统，与size_t类型最相匹配。



如果使用方案一：在unity32位打包的时候，就会把C#的ulong传递给C++的size_t，这就是64位转32位，会出错。

如果使用方案二：C#侧会受到一些限制，C#的可用数值范围变小了。但是这种方法在数值较小的情况下，不会有什么问题。

如果使用方案三：一切完美解决。



关键在于C#的封装：

Oculus把所有的size_t转成了UIntPtr，所以就自动避免了这个问题。

```

[DllImport(*DLL_NAME*, CallingConvention = CallingConvention.*Cdecl*)]
public static extern IntPtr *ovr_UserArray_GetElement*(IntPtr obj, UIntPtr index);

```

# 解决方案

1. 因为在64位下面总是正常的，所以只需要考虑兼容32位。把所有出现size_t的地方都使用UIntPtr替代。





https://stackoverflow.com/questions/32906774/what-is-equal-to-the-c-size-t-in-c-sharp
