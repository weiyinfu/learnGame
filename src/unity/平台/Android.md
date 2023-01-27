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
* dataPath	D:/Documents/Xuporter/Assets
* persistentDataPath	C:/Users/Administrator/AppData/LocalLow/mj/path
* streamingAssetsPath	D:/Documents/Xuporter/Assets/StreamingAssets
* temporaryCachePath	C:/Users/ADMINI~1/AppData/Local/Temp/mj/path

## android
* dataPath	/data/app/com.mi.path-1.apk	无权限
* persistentDataPath	/data/data/com.mi.path/files	读写，强推荐
* streamingAssetsPath	jar:file:///data/app/com.mi.path-1.apk!/assets	只读
* temporaryCachePath	/data/data/com.mi.path/cache	读写

## iphone
* dataPath	/var/mobile/Containers/Bundle/Application/AFE239B4-2FE5-48B5-8A31-FC23FEDA0189/ad.app/Data 	无权限
* persistentDataPath	/var/mobile/Containers/Data/Application/FFEEF1E0-E15E-4BC0-9E8F-78084A2085A0/Documents 读写，强推荐
* streamingAssetsPath	/var/mobile/Containers/Bundle/Application/AFE239B4-2FE5-48B5-8A31-FC23FEDA0189/ad.app/Data/Raw	只读
* temporaryCachePath	/var/mobile/Containers/Data/Application/FFEEF1E0-E15E-4BC0-9E8F-78084A2085A0/Library/Caches	读写

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
