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