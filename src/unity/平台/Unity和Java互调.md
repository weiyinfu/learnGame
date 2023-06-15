# Unity支持的Android插件

Unity的Android插件可以通过以下方式提供。

- AAR：一个压缩包

- Android Library项目，使用Android Studio创建的一个代码目录。Unity会自动扫描Assets下面的目录，如果一个目录下面有mylibrary.androidlib文件，则Unity将其视为一个AndroidLibrary项目。

- jar插件：jar插件就是一个jar包，放在Assets目录下任意文件夹即可。

- 直接把Java源文件放在Plugins里面

# C#调用Java

Unity调用JNI的原理：实际上Unity是无法直接调用Java的，它只能直接调用C++。但是在Android平台上调用Java是一个非常强烈的需求。而JNI接口又几乎是不怎么变化的，因此Unity团队把JNI（C++）的头文件用C#封装了一遍，从而可以在C#里面调用JNI。JNI不够好用，又对它使用AndroidJavaObject系列包装了一层。

Unity C#调用Java插件的有三层封装，对应三种级别的API。

第一层：AndroidJNI是调用原始JNI接口的包装器。这个类底层实现是C语言，这个类的所有方法都是静态的，并且与Java原生接口一一对应。可以认为，它就是JNI的原始封装，它所有的方法都是静态方法。

第二层：AndroidJNIHelper是AndroidJNI的一些便利封装。

第三层：AndroidJavaObject和AndroidJavaClass是对AndroidJNI和AndroidJNIHelper的封装。在使用JNI调用时自动执行许多任务，并且使用缓存加快Java的调用速度。AndroidJavaObject对应java.lang.Object,AndroidJavaClass对应java.lang.Class，它们的封装也是一一映射的。它们基本上提供与Java端的三种交互：

- 调用一个方法

- 获取字段的值

- 设置字段的值

这三种交互每种都包含两种调用：操作实例和操作类的静态成员。

总结一下，C#调用Java的三种方式

- 裸用AndroidJNI，比较底层

- AndroidJNIHelper+AndroidJNI

- AndroidJavaObject和AndroidJavaClass，高级API。

实例一：实例化对象

```java
AndroidJavaObject jo = new AndroidJavaObject("java.lang.String", "some_string"); 
  // jni.FindClass("java.lang.String");
  // jni.GetMethodID(classID, "<init>", "(Ljava/lang/String;)V");
  // jni.NewStringUTF("some_string");
  // jni.NewObject(classID, methodID, javaString);
  int hash = jo.Call<int>("hashCode"); 
  // jni.GetMethodID(classID, "hashCode", "()I");
  // jni.CallIntMethod(objectID, methodID);
```

实例化内部类的时候，使用$

内部类必须使用 $ 分隔符。请使用 `android.view.ViewGroup$LayoutParams`

实例二：获取应用程序的缓存目录

```java
AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
 // jni.FindClass("com.unity3d.player.UnityPlayer"); 
 AndroidJavaObject jo = jc.GetStatic <AndroidJavaObject>("currentActivity"); 
 // jni.GetStaticFieldID(classID, "Ljava/lang/Object;");  
 // jni.GetStaticObjectField(classID, fieldID); 
 // jni.FindClass("java.lang.Object"); 

 Debug.Log(jo.Call <AndroidJavaObject>("getCacheDir").Call<string>("getCanonicalPath")); 
 // jni.GetMethodID(classID, "getCacheDir", "()Ljava/io/File;"); // 或其任何基类！
 // jni.CallObjectMethod(objectID, methodID); 
 // jni.FindClass("java.io.File"); 
 // jni.GetMethodID(classID, "getCanonicalPath", "()Ljava/lang/String;"); 
 // jni.CallObjectMethod(objectID, methodID); 
 // jni.GetStringUTFChars(javaString);
```

实例三：Java将数据传递到Unity

UnityActivity有一个UnitySendMessage方法，用于将消息发送给Unity。

UnitySendMessage(gameObjectName,gameObjectCallbackFunctionName,realMessage)



```java
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour { 

    void Start () { 
        AndroidJNIHelper.debug = true; 
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) { 
        jc.CallStatic("UnitySendMessage", "Main Camera", "JavaMessage", "NewMessage");
        } 
    } 

    void JavaMessage(string message) { 
        Debug.Log("message from java: " + message); 
    }
}
```

实例四：尽量使用using来管理AndroidJavaClass和AndroidJavaObject的生命周期。

如果不使用using，则回收AndroidJavaClass、AndroidJavaObject的时候自动释放。

如果将 `AndroidJNIHelper.debug` 设置为 true，您将在调试输出中看到垃圾回收器的活动记录。

```java
//安全地获取系统语言
void Start () { 
    using (AndroidJavaClass cls = new AndroidJavaClass("java.util.Locale")) { 
        using(AndroidJavaObject locale = cls.CallStatic<AndroidJavaObject>("getDefault")) { 
            Debug.Log("current lang = " + locale.Call<string>("getDisplayLanguage")); 

        } 
    } 
}
```

实例五：liangdong的例子，isLogin(activity)

Java侧

```java
    
    public void GetUserInfo()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activityObject = jc.GetStatic<AndroidJavaObject>("currentActivity");
        Debug.Log("Login  update activityObject " + activityObject);

        AndroidJavaClass jc2 = new AndroidJavaClass("com.bytedance.picovr.sdk.usercenter.utils.UserCenterUtils");
        Boolean isLogin = jc2.CallStatic<Boolean>("isLogin", activityObject);
        Debug.Log("Login  update isLogin " + isLogin);
    }
```

C#侧

```java

    public class LoginCallBack : MonoBehaviour
    {

        public void LoginSuccess(string loginInfo)
        {
            Debug.Log("LoginCallBack LoginSuccess :" + loginInfo);
            Text PhoneNumtext = GameObject.Find("PhoneNumText").GetComponent<Text>();
            Text GenderText = GameObject.Find("GenderText").GetComponent<Text>();
            Text UniqidText = GameObject.Find("UniqidText").GetComponent<Text>();

            if (loginInfo != null)
            {
                JsonData jsrr = JsonMapper.ToObject(loginInfo);
                jsrr["phone"].ToString();
                PhoneNumtext.text = "用户手机号码：" + jsrr["phone"].ToString();
                GenderText.text = "用户性别:" + jsrr["gender"].ToString();
                UniqidText.text = "用户ID:" + jsrr["uniqid"].ToString();
            } else {

                PhoneNumtext.text = "用户未登录";
            }
        }

        public void UnLogin(string message) {
            Text PhoneNumtext = GameObject.Find("PhoneNumText").GetComponent<Text>();
            PhoneNumtext.text = "用户未登录";
        }

    }
```

实例六：XR旧代码中的支付和登录

登录支付部分的代码，它会通过调用Java的方式来完成一些动作。

```Plaintext
// Copyright © 2015-2021 Pico Technology Co., Ltd. All Rights Reserved.

#if !UNITY_EDITOR
#if UNITY_ANDROID
#define ANDROID_DEVICE
#elif UNITY_IPHONE
#define IOS_DEVICE
#elif UNITY_STANDALONE_WIN
#define WIN_DEVICE
#endif
#endif

using UnityEngine;

namespace Unity.XR.PXR
{
    public class PicoPaymentSDK
    {
        private static AndroidJavaObject _jo = new AndroidJavaObject("com.pico.loginpaysdk.UnityInterface");

        public static AndroidJavaObject jo
        {
            get { return _jo; }
            set { _jo = value; }
        }

        public static void Login()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject mJo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("init", mJo);
            jo.Call("authSSO");


        }

        public static void Pay(string payOrderJson)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject mJo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("init", mJo);
            jo.Call("pay", payOrderJson);

        }

        public static void QueryOrder(string orderId)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject mJo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("init", mJo);
            jo.Call("queryOrder", orderId);

        }

        public static void GetUserAPI()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject mJo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("init", mJo);
            jo.Call("getUserAPI");
        }
    }
}
```

```java
AndroidJavaObject jo = new AndroidJavaObject("com.pico.loginpaysdk.UnityInterface");
public static void Login(){
    ...
    jo.Call("init", mJo);
    jo.Call("authSSO");
    ...
}


public static void Pay(string payOrderJson){
    ...
    jo.Call("init", mJo);
    jo.Call("pay", payOrderJson);
    ...
}
```

实例七：

```java
string packageName = "com.unity3d.player.UnityPlayer";
              if (unityAcvity == null)
              {
                  unityAcvity = new AndroidJavaClass(packageName).GetStatic<AndroidJavaObject>("currentActivity");
              }
  //直接获取主Activity的类名
  AndroidJavaObject a = new AndroidJavaObject("packagename.classname");
  //获取指定包名下面的类
  classname.Call<returntype>(string methodname,params object[] args);
  // 通过AndroidJavaObject 中封装的方法获取调用类中的方法，同时传递参数，并获得返回类型
  这边只是其中的一种调用，在Api中还包含其他很多种可自行查阅
```

# Java调用C#

Java要想调用C#，就需要引入Unity的clasess.jar，这样Java调用C#的时候才能找到一些关键类。

在项目的libs目录中添加Unity 安装目录中的classes.jar(该jar文件中的UnityPlayer类中的UnitySendMessage方法可以实现Java方法调用Unity GameObject上绑定的C#脚本中的方法)。同时在build.gradle文件中的dependencies添加compileOnly files('libs/classes.jar') 。

此处compileOnly表示只用于编译，不要把classes.jar打包进aar里面去。

Java调用C#有两种方式：

1. UnitySendMessage：直接根据GameObject的名称找到GameObject，调用GameObject的某个函数。

2. 定义一个C#类，定义一个Java类，直接调用Unity

## UnitySendMessage

UnityPlayer.UnitySendMessage(String GameObjectName, String MethodName, String param)

这个函数只能向C#侧的某个GameObject发送消息，并且函数参数只能是string类型，C#侧可能需要解析string类型的消息。

```java
// 需要先引入Unity的Jar包
import com.unity3d.player.UnityPlayer;
// 调用 object上 C# 脚本中的方法
UnityPlayer.UnitySendMessage("objectName", "methodName", "data");
```

### 示例一：loginpay中的实现

Android端，调用Java 方法进行数据处理 , 在回调中通知Unity端

```Java
public void init(Activity activity) {
    // 初始化
    ...
}

// 授权登录    
public void authSSO(){
   // 调用Java登录入口
   Login mLogin = new Login();
   mLogin.login(new Callback(){
       public void loginCallback(boolean isSuccess,String mesage){
           ...
           // 向 Unity 发送结果
           UnityPlayer.UnitySendMessage("PicoPayment", "LoginCallback", message);
           ...
       }
   });
}
// 支付
public void pay(String payOrderJson) {
    // 调用Java支付入口
    PayOrder order = PayOrder.parse(payOrderJson);
    PicoPay.getInstance(mUnityPlayerActivity).pay(order, new PaySDKCallBack(){
        @Override
        public void callback(String code, String msg) {
            ...
            // 向 Unity 发送结果
            UnityPlayer.UnitySendMessage("PicoPayment", "QueryOrPayCallback", msg);
            ...
        }
        ...
        ...
        
    });
}

```

### 示例二：Android改变画布颜色

Unity添加一个MonoBehavior

```java
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityAndroidTest: MonoBehaviour
{   
    // 0. 该成员就是 Canvas 内部的 Text 控件.
    public Text text1;

    void Start()
    {
        // 1. 页面初始化时，初始化 Android 通信对象
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

        // 2.定义一个int变量
        int res = 0;

        try
        {
            // 3.调用Android平台的 increment 自增方法，该方法会返回一个 int 值
            res = jo.Call<int>("increment", 2);
            // 4.将返回值更新到 Text 上
            text1.text = res.ToString();
        }
        catch (Exception e)
        {
            text1.text = "error";
        }
    }

    void Update()
    {
        
    }

    // 5.这里我们向Android平台暴露一个方法，调用后可以改变Text的颜色
    public void ChangeColor()
    {
        text1.color = Color.red;
    }
}
```

Android调用Unity

```java
public class UnityPlayerActivity extends Activity implements IUnityPlayerLifecycleEvents {
  // ...  
  // 省略其它代码
  // ...
  
  private int count = 1;
  
  // 1.Unity项目中的 UnityAndroidTest.cs 脚本，会主动唤起该方法
  public int increment(int value) {
      count += value;
      // 2.在这个方法中，我们通过发送消息，让 Canvas 执行 ChangeColor 方法
      //   将文字从黑色变成红色
      UnityPlayer.UnitySendMessage("Canvas", "ChangeColor", "");
      
      // 3.最终，将 int 类型的 count = 3 作为桥接方法的返回值返回
      return count;
  }
}
```

### 示例三：UnitySendMessageExtension

UnityPlayer.UnitySendMessage只能用来Java向C#发送一个通知，无法得到C#侧的Response。

基于UnitySendMessage可以实现带返回值的调用，原理就是C#侧主动把返回值推送给Java侧。

UnitySendMessage是一个同步函数，会等待C#侧函数执行结束。

Java侧的封装

```java
public final class MyPlugin
{
    //Make class static variable so that the callback function is sent to one instance of this class.public static MyPlugin testInstance;

     public static MyPlugin instance()
     {
         if(testInstance == null)
         {
             testInstance = new MyPlugin();
         }
         return testInstance;
     }

    string result = "";


    public string UnitySendMessageExtension(string gameObject, string functionName, string funcParam)
    {
        UnityPlayer.UnitySendMessage(gameObject, functionName, funcParam);
        string tempResult = result;
        return tempResult;
    }

    //Receives result from C# and saves it to result  variablevoid receiveResult(string value)
    {
        result = "";//Clear old data
        result = value; //Get new one
    }
}
```

C#侧的配合

```java
class TestScript: MonoBehaviour
{
    //Sends the data from PlayerPrefs to the receiveResult function in Javavoid sendResultToJava(float value)
    {
        using(AndroidJavaClass javaPlugin = new AndroidJavaClass("com.company.product.MyPlugin"))
        {
             AndroidJavaObject pluginInstance = javaPlugin.CallStatic("instance");
             pluginInstance.Call("receiveResult",value.ToString());
        }
    }

    //Called from Java to get the saved PlayerPrefsvoid testFunction(string key)
    {
        float value = PlayerPrefs.GetFloat(key) //Get the saved value from keysendResultToJava(value); //Send the value to Java
    }
}
```

在Java侧使用带返回值的UnitySendMessage

```C++
String str = UnitySendMessageExtension("ProfileLoad", "testFunction","highScore");
```

## 使用类

```java
Unity
public class A ：AndroidJavaProxy
{
    private const string interfaceName = "packageName.IB";
    public A() : base(interfaceName)
    {
    }
    func（）
    {
    }
}

Android
public interface IB
{
    func();
}
public void setA(AscanMatcher) {
    func();
}
利用接口的方式，做回调函数。


```

# Unity中C#与Java高频互调产生ANR

## 现象

ANR：Application Not Respond，应用无响应。与stackOverlow、segmentFault、nullPointerReference等著名错误类似，是一种错误的程序状态。

Unity中，C#与Java层的互相调用一般情况下没问题，但是在高频、互调的情况下会产生死锁，进而导致ANR问题。

首先，如果程序只涉及到C#调用Java，因为这个调用发生在主线程中，所以不会发生死锁问题。

其次，如果程序只是低频的出现Java调用C#，那么发生死锁的概率极低。

最后，如果程序高频出现C#调用Java和Java调用C#，那么出现死锁的概率就挺大。

C#调用Java一般都是在主线程中进行，Java调用C#则有可能是其它的线程。

当Java调用C#的时候，有一个加锁语句。如果C#和Java互调的时候，线程出现竞争，导致了死锁，那么就会出现程序卡住的情况。

## 解决方案

ANR产生的根本原因在于线程死锁。解决思路就是避免从Java侧主动调用C#。

实现方式是：保证Java调用C#的时候，统一收敛到UnityMain线程去执行。

在Java侧维护一个队列，Java想调用C#的时候，往Java的任务队列里面塞任务即可。

在C#侧创建一个MonoBehavior，每一帧调用Java，Java受到调用的时候，从队列里面弹出一个Runnable并执行之。

本文中第二部分介绍了很多Java调用C#的内容，其实这一部分内容是很危险的，只能在主线程中使用，不然就容易出现死锁。
