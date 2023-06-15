Unity的脚本分为两类：运行时和Editor中。写脚本的时候一定要区分好脚本的位置，在运行时调用Editor脚本肯定会出错，在Editor中调用运行时脚本也会出错。

Unity的Editor相关的脚本都位于UnityEditor命名空间，与构建相关的脚本位于UnityEditor.Build命名空间。

Unity Editor的构建过程中的回调基本上都位于UnityEditor.Build这个命名空间中，每一个回调是一个接口，每一个接口只包含一个回调函数。编辑器会扫描所有实现了这些接口的类，在执行的过程中调用这些回调。由此引入一个问题：如果某个回调接口有多个实现，它们的调用顺序需要怎么控制呢？

实际上，所有的回调接口都继承自IOrderedCallback，IOrderedCallback有一个callbackOrder属性，这个属性决定了执行顺序。

https://docs.unity3d.com/ScriptReference/Build.IOrderedCallback.html

|                            |                                                                                                                                |
| -------------------------- | ------------------------------------------------------------------------------------------------------------------------------ |
| IProcessBuildWithReport    | 在开始构建之前执行的回调。IProcessBuildWithReport继承自接口IOrderedCallback。<br><br>IProcessBuildWithReport有一个OnPreprocessBuild函数，在开始构建之前执行这个回调。 |
| IActiveBuildTargetChanged  | 构建目标发生改变的时候执行此回调。                                                                                                              |
| IFilterBuildAssemblies     | 忽略不需要打包assemblies                                                                                                              |
| Ill2CppProcessor           | 已经删除，在构建IL2cpp之前执行这个函数                                                                                                         |
| IPostBuildPlayerScriptDLLs | 在编译脚本结束之后执行此回调                                                                                                                 |
| IPostprocessBuild          | 已废弃，推荐使用IPostProcessBuildWithReport，这个回调没有report参数，信息太少，因此被废弃。                                                                 |
| IPreprocessBuild           | 已废弃，推荐使用IPreprocessBuildWithReport                                                                                             |
| IProcessSceneWithReport    | 在构建每一个场景的时候都会执行此回调。                                                                                                            |
| IProcessScene              | 已废弃，推荐使用IProcessSceneWithReport。                                                                                               |

从上述接口中，可以看出很多接口已经废弃了。原因是它们参数太少，改用一个WithReport函数替代。

# IPostGenerateGradleAndroidProject：生成gralde项目之后的回调

虽然与构建相关的回调都位于UnityEditor.Build目录下，但是与平台相关的、又与构建相关的脚本位于平台命名空间下。

UnityEditor.Android包下唯一的接口，在Editor目录下所有实现这个接口的类都会执行OnPostGenerateGradleAndroidProject函数。

趁此机会，了解一下UnityEditor.Android包下的内容。这个包是一个非常简单的包，代码量非常小。

两个类：

- AndroidExternalToolSettings：对应PlayerSettings里面的一些配置。
  
  - gradlePath：gradle可执行程序的路径。
  
  - jdkRootPath：jdk的路径
  
  - keystoreDedicatedLocation
  
  - maxJvmHeapSize：用于构建Android应用的Jvm堆大小
  
  - ndkRootPath、sdkRootPath：Android SDK、NDK的路径

- AndroidPlatformIconKind：是一个类，包含一些静态属性。UnityEditor.PlatformIconKind是一个接口，它的实现包括AndroidPlatformIconKind，iOSPlatformIconKind。
  
  - Adaptive：自适应
  
  - Legacy：遗留的
  
  - Round：圆角的

# XRBuildHelper：专为XR实现的生命周期回调

UnityEditor.XR.Management是一个专门的包，用于管理XR开发相关的任务。

XRBuildHelper位于UnityEditor.XR.Management命名空间下，用于管理编译中的回调。

XRBuildHelper实现了IPreprocessBuildWithReport和IPostprocessBuildWithReport两个接口。

```Plaintext
public abstract class XRBuildHelper<T> : IPreprocessBuildWithReport, IPostprocessBuildWithReport where T : Object
```
