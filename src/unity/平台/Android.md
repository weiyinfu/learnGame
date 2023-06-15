# 在Native中获取当前Activity

在unity中有时需要传给android插件Context，这样可调用一些如getApplicationxxx的android api。  
可通过下面的代码获取currentActivity：

```
private AndroidJavaObject getUnityContext()
{
    AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    AndroidJavaObject unityActivity = unityClass.GetStatic<androidjavaobject>("currentActivity");
    return unityActivity;
}
```

在C++里面获取当前Activity

```
//get unity activity first
SYMBOL_HIDDEN jobject getUnityActivity(JNIEnv *jni) {

    jclass clazz = jni->FindClass("com/unity3d/player/UnityPlayer");
    if (clazz == NULL) {
        LOGE("[getUnityActivity]cannot find class:com/unity3d/player/UnityPlayer");
        return NULL;
    }
    jfieldID activityFI = jni->GetStaticFieldID(clazz, "currentActivity", "Landroid/app/Activity;");
    jobject activityObj = jni->GetStaticObjectField(clazz, activityFI);
    return activityObj;
}
```

# Unity常用路径

## editor

* dataPath    D:/Documents/Xuporter/Assets
* persistentDataPath    C:/Users/Administrator/AppData/LocalLow/mj/path
* streamingAssetsPath    D:/Documents/Xuporter/Assets/StreamingAssets
* temporaryCachePath    C:/Users/ADMINI~1/AppData/Local/Temp/mj/path

## android

* dataPath    /data/app/com.mi.path-1.apk    无权限
* persistentDataPath    /data/data/com.mi.path/files    读写，强推荐
* streamingAssetsPath    jar:file:///data/app/com.mi.path-1.apk!/assets    只读
* temporaryCachePath    /data/data/com.mi.path/cache    读写

## iphone

* dataPath    /var/mobile/Containers/Bundle/Application/AFE239B4-2FE5-48B5-8A31-FC23FEDA0189/ad.app/Data     无权限
* persistentDataPath    /var/mobile/Containers/Data/Application/FFEEF1E0-E15E-4BC0-9E8F-78084A2085A0/Documents 读写，强推荐
* streamingAssetsPath    /var/mobile/Containers/Bundle/Application/AFE239B4-2FE5-48B5-8A31-FC23FEDA0189/ad.app/Data/Raw    只读
* temporaryCachePath    /var/mobile/Containers/Data/Application/FFEEF1E0-E15E-4BC0-9E8F-78084A2085A0/Library/Caches    读写

# Android项目的编译

如果是包含了Android工程就需要进行gradle的修改，在Unity的安装目录下面Editor\Data\PlaybackEngines\AndroidPlayer\Tools\GradleTemplates找到这个目录，这里面包含了gradle的配置，如果需要对gradle进行修改，把相应的拷贝出来放在plugins/Android目录，在编译的时候就会替换默认的gradle文件。
在编译安卓工程的时候，会将主工程编译成一个库，然后launcher项目编译应用程序去包含这个主工程，所以在gradle主要修改就baseProjectTemplate和mainTemplate，安卓工程引用了kotlin，这边需要在baseProjectTemplate中

```plain
dependencies {
                classpath   'com.android.tools.build:gradle:3.6.0'
                classpath   "org.jetbrains.kotlin:kotlin-  gradle  -plugin:1.3.71"
              **BUILD_SCRIPT_DEPS**
          }
kolin gradle，然后还需要在maintemplate中添加
apply plugin: 'kotlin-android'
apply plugin: 'kotlin-android-extensions'
**APPLY_PLUGINS**
dependencies {
    implementation fileTree(dir: 'libs', include: ['*.jar'])
        implementation "org.jetbrains.kotlin:kotlin-stdlib-jdk7:1.3.71"
**DEPS**}
```

其实就是把安卓工程中的gradle配置写入到maintemplate.gradle文件中，gradle的修改需要根据项目需求进行修改。
第一种编译方式 以Unity为主，把安卓工程编译成一个jar包，放入到Plugins/Android中，然后通过Unity引擎编译成Apk，File-buildsetting-build
第二种编译方式，以Android为主，勾选BuildSetting 中的ExportProject，然后export，把Unity工程导出，这个操作会把所有的dll 库编译成cpp的代码，把资源编译成android的assets然后再放到android工程进行编译

# UI Orientation

一共有五种取值。  

* Portrait：纵向，包括Portrait和Portrait Upside down
* Landscape：横向，分为Landscape Left和Landscape Right
* Audo Rotation：自动旋转

# Unity编译报错：缺少appcompat类，找不到context compat

https://stackoverflow.com/questions/69021225/resource-linking-fails-on-lstar/69045181#69045181

mainTemplate.gradle

```
dependencies {
    implementation fileTree(dir: 'libs', include: ['*.jar'])
    implementation 'androidx.appcompat:appcompat:1.4.1'
    implementation 'com.bytedance.speechengine:speechengine_asr_tob:1.0.6'
```

只加这个依赖，可能会报错

```
Execution failed for task ':app:processDevelopmentDebugResources'.

> A failure occurred while executing com.android.build.gradle.internal.tasks.Workers$ActionFacade
   > Android resource linking failed
.../app/build/intermediates/incremental/mergeDevelopmentDebugResources/merged.dir/values/values.xml:2682: AAPT: error: resource android:attr/lStar not found
```

launchTemplate.gradle

```
configurations.all {
  resolutionStrategy {
    force 'androidx.core:core:1.6.0'
    force 'androidx.core:core-ktx:1.6.0'
  }
}
```

# Unity的JNI反射获取函数签名是基于反射

例如load64(android.content.Context,android.content.Context,int ,int ,int ,int ,int)这个签名不能接受application类型的参数。

```
var getApiFunction = driverClass.CallStatic<long>("load64", application.Call<AndroidJavaObject>("getContext"), packageContext.Call<AndroidJavaObject>("getContext"), 1, 1, 50, 0, 0);
```

# Android移动端脚本

应用程序可以通过 <u><a href="https://docs.unity3d.com/cn/2020.3/ScriptReference/Input.html">Input</a></u> 和 <u><a href="https://docs.unity3d.com/cn/2020.3/ScriptReference/Handheld.html">Handheld</a></u> 类来访问 Android 设备的大多数功能。有关更多信息，请参阅：

- <u><a href="https://docs.unity3d.com/cn/2020.3/Manual/MobileInput.html">移动设备输入</a></u>

- <u><a href="https://docs.unity3d.com/cn/2020.3/Manual/MobileKeyboard.html">移动键盘</a></u>

## **振动支持**

可通过调用 <u><a href="https://docs.unity3d.com/cn/2020.3/ScriptReference/Handheld.Vibrate.html">Handheld.Vibrate</a></u> 来触发振动。不含振动硬件的设备将忽略此调用。

## 屏幕方向

可在 iOS 和 Android 设备上控制应用程序的屏幕方向。检测方向变化或强制使用特定方向对于创建一些取决于用户如何握持设备的游戏行为很有用。

通过访问 <u><a href="https://docs.unity3d.com/cn/2020.3/ScriptReference/Screen-orientation.html">Screen.orientation</a></u> 属性来获取设备方向。允许的方向如下：

* Portrait：设备处于纵向模式，直立握持设备，主屏幕按钮位于底部。

* Portrait Upside down：设备处于纵向模式，但是上下颠倒，直立握持设备，主屏幕按钮位于顶部。

* LandscapeLeft：设备处于横向模式，直立握持设备，主屏幕按钮位于右侧

* LandscapeRight：设备处于横向模式，直立握持设备，主屏幕按钮位于左侧



将 <u><a href="https://docs.unity3d.com/cn/2020.3/ScriptReference/Screen-orientation.html">Screen.orientation</a></u> 设置为上述方向之一，或使用 <u><a href="https://docs.unity3d.com/cn/2020.3/ScriptReference/ScreenOrientation.AutoRotation.html">ScreenOrientation.AutoRotation</a></u> 来控制屏幕方向。启用自动旋转后，仍可根据具体情况禁用某个方向。

使用以下脚本来控制自动旋转：

- <u><a href="https://docs.unity3d.com/cn/2020.3/ScriptReference/Screen-autorotateToPortrait.html">Screen.autorotateToPortrait</a></u>

- <u><a href="https://docs.unity3d.com/cn/2020.3/ScriptReference/Screen-autorotateToPortraitUpsideDown.html">Screen.autorotateToPortraitUpsideDown</a></u>

- <u><a href="https://docs.unity3d.com/cn/2020.3/ScriptReference/Screen-autorotateToLandscapeLeft.html">Screen.autorotateToLandscapeLeft</a></u>

- <u><a href="https://docs.unity3d.com/cn/2020.3/ScriptReference/Screen-autorotateToLandscapeRight.html">Screen.autorotateToLandscapeRight</a></u>





# 创建Android项目编译失败

可能是keystore有损坏，删掉旧版的keystore重新编译即可。





# Unity的RuntimeInitializeOnLoadMethodAttribute

Unity提供了一些回调钩子，这些回调的场景如下：

RuntimeInitializeLoadType

- AfterSceneLoad：场景加载之后

- BeforeSceneLoad：场景加载之前

- AfterAssembliesLoaded：

- BeforeSplashScreen：Splash显示之前

- SubsystemRegistration：子系统注册的时候的回调

这套回调机制只有两个东西，一个类和一个枚举类型

```Bash
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
static void Initializing()
{
    ShootsStub.GetInstance();
}
```

# Unity demo停一段时间之后报错

  报错Mesh包含的顶点数过多。一般是因为渲染的东西太多导致的。

  在我的程序中，有一个Text显示Log，这个Text越来越长，导致文本渲染不过来。

```

```