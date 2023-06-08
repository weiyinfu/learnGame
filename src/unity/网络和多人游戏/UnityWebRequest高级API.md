# Get请求
```
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
 
public class MyBehaviour : MonoBehaviour {
    void Start() {
        StartCoroutine(GetText());
    }
 
    IEnumerator GetText() {
        UnityWebRequest www = UnityWebRequest.Get("http://www.my-server.com");
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            // 将结果显示为文本
            Debug.Log(www.downloadHandler.text);
 
            // 或者以二进制数据格式检索结果
            byte[] results = www.downloadHandler.data;
        }
    }
}
```

# GetTexture
```
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
 
public class MyBehaviour : MonoBehaviour {
    void Start() {
        StartCoroutine(GetTexture());
    }
 
    IEnumerator GetTexture() {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://www.my-server.com/image.png");
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }
}
```
Texture的获取也可以通过DownloadHandler
```
    IEnumerator GetTexture() {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://www.my-server.com/image.png");
            yield return www.SendWebRequest();

            Texture myTexture = DownloadHandlerTexture.GetContent(www);
        }
```

# 从服务器下载AssetBundle
```
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
 
public class MyBehaviour : MonoBehaviour {
    void Start() {
        StartCoroutine(GetAssetBundle());
    }
 
    IEnumerator GetAssetBundle() {
        UnityWebRequest www = UnityWebRequest.GetAssetBundle("http://www.my-server.com/myData.unity3d");
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
        }
    }
}
```

# POST请求提交表单
表单鼓励使用` List<IMultipartFormSection> formData`，旧版的`UnityWebRequest.Post(string url, WWWForm formData);`已经不鼓励使用了。  
```
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class MyBehavior : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

        UnityWebRequest www = UnityWebRequest.Post("http://www.my-server.com/myform", formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}
```

# PUT请求
```
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
 
public class MyBehavior : MonoBehaviour {
    void Start() {
        StartCoroutine(Upload());
    }
 
    IEnumerator Upload() {
        byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
        UnityWebRequest www = UnityWebRequest.Put("http://www.my-server.com/upload", myData);
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Upload complete!");
        }
    }
}
```