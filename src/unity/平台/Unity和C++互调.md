# C++调用JNI常见套路

在OnLoad的时候把env保存下来，然后getEnv的时候使用AttachCurrentThread。需要注意的是：每个进程都有自己的JNIEnv对象，不能把JNIEnv作为全局变量保存下来，可以把JavaVM这个对象保存下来。

当开发者通过JNI_OnLoad的方式加载so的时候，JNI_OnLoad才会被调用。如果开发者直接把env传了进来，则也能够通过env把JavaVM保存下来。

```Bash
jint JNI_OnLoad(JavaVM* vm, void* reserved) {
  //将vm对象保存下来
  return JNI_VERSION_1_6;
}
JNIEnv*getEnv(){
    JNIEnv* jni_env = 0;
    vm->AttachCurrentThread(&jni_env, 0);
}
```

通过env获取JavaVM

```C#
SYMBOL_HIDDEN void initGlobalData(JNIEnv *env) {
    auto g = getGlobalData();
    if (g->Vm == nullptr) {
        env->GetJavaVM(&(g->Vm));
    }
}
```

# 传递activity给C++层

```C++
    AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");    var requestId = CLIB.ppf_InitializeAndroidAsynchronous(appId, activity.GetRawObject(), IntPtr.Zero);
```

# 动态库

## 如果一个so使用了JNI_ON_LOAD

那么加载它的时候，必须使用java的system.loadlibrary加载这个动态链接库，而不能直接使用C++去加载它。JVM虚拟机加载了一个动态链接库，跟C++加载了一个动态链接库，不一样。

在Unity里面使用JVM显式加载一个库。

//AndroidJavaObject system = new AndroidJavaClass("java.lang.System");

// system.CallStatic("loadLibrary", "volcenginertc");

//system.CallStatic("loadLibrary", "byteaudio");

## Unity报错：找不到dll

1. 首先检查dll的属性，是否勾选了android armv7或者armv8等。

2. 检查apk里面是否有dll，如果没有，说明动态链接库压根没有打包进去

3. Unity在加载A.dll的过程中，如果发生错误，则直接报A.dll不存在；因此，找不到dll的原因可能是加载dll的时候报错了。
   
   1. 在JNI_OnLoad等处判断是否有抛出异常
   
   2. A.dll是否依赖了另一个dll，如果另一个dll缺失，则A.dll加载失败。

# Unity调C++时无法传递JNIEnv

https://stackoverflow.com/questions/38862197/getting-valid-jnienv-pointer

只能在C++侧使用JNI_Onload来实现获取JVM。

# unity导入package含有dylib

设置/安全与隐私/通用，设置允许xxx。

# C++调用C#的方式
