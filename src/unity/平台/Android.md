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
