# 创建Get请求
```
UnityWebRequest wr = new UnityWebRequest(); // 完全为空
UnityWebRequest wr2 = new UnityWebRequest("http://www.mysite.com"); // 设置目标 URL

// 必须提供以下两项才能让 Web 请求正常工作
wr.url = "http://www.mysite.com";
wr.method = UnityWebRequest.kHttpVerbGET;   // 可设置为任何自定义方法，提供了公共常量

wr.useHttpContinue = false;
wr.chunkedTransfer = false;
wr.redirectLimit = 0;  // 禁用重定向
wr.timeout = 60;       // 此设置不要太小，Web 请求需要一些时间
```

# 创建UploadHandler
```
byte[] payload = new byte[1024];
// ...使用数据填充有效负载 ...

UnityWebRequest wr = new UnityWebRequest("http://www.mysite.com/data-upload");
UploadHandler uploader = new UploadHandlerRaw(payload);

// 发送标头："Content-Type: custom/content-type";
uploader.contentType = "custom/content-type";

wr.uploadHandler = uploader;
```

# 创建DownloadHandler
DownloadHandlers 有多种类型：

* DownloadHandlerBuffer 用于简单的数据存储，下载结果为二进制。
* DownloadHandlerFile 用于下载文件并将文件保存到磁盘（内存占用少）。
* DownloadHandlerTexture 用于下载图像。
* DownloadHandlerAssetBundle 用于提取 AssetBundle。
* DownloadHandlerAudioClip 用于下载音频文件。
* DownloadHandlerMovieTexture 用于下载视频文件。由于 MovieTexture 已被弃用，因此建议您使用 VideoPlayer 进行视频下载和电影播放。
* DownloadHandlerScript 是一个特殊类。就其本身而言，不会执行任何操作。但是，此类可由用户定义的类继承。此类接收来自 UnityWebRequest 系统的回调，然后可以使用这些回调在数据从网络到达时执行完全自定义的数据处理。

## Buffer
```
using UnityEngine;
using UnityEngine.Networking; 
using System.Collections;

 
public class MyBehaviour : MonoBehaviour {
    void Start() {
        StartCoroutine(GetText());
    }
 
    IEnumerator GetText() {
        UnityWebRequest www = new UnityWebRequest("http://www.my-server.com");
        www.downloadHandler = new DownloadHandlerBuffer();
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

## 文件
```
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileDownloader : MonoBehaviour {

    void Start () {
        StartCoroutine(DownloadFile());
    }
    
    IEnumerator DownloadFile() {
        var uwr = new UnityWebRequest("https://unity3d.com/", UnityWebRequest.kHttpVerbGET);
        string path = Path.Combine(Application.persistentDataPath, "unity3d.html");
        uwr.downloadHandler = new DownloadHandlerFile(path);
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError || uwr.isHttpError)
            Debug.LogError(uwr.error);
        else
            Debug.Log("File successfully downloaded and saved to " + path);
    }
}
```

# 纹理
DownloadHandlerTexture下载一个纹理， 等价于：使用DownloadHandlerBuffer+Texture.LoadImage（从原始字节创建纹理）。  

```
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking; 
using System.Collections;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class ImageDownloader : MonoBehaviour {
    UnityEngine.UI.Image _img;
 
    void Start () {
        _img = GetComponent<UnityEngine.UI.Image>();
        Download("http://www.mysite.com/myimage.png");
    }
 
    public void Download(string url) {
        StartCoroutine(LoadFromWeb(url));
    }
 
    IEnumerator LoadFromWeb(string url)
    {
        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        yield return wr.SendWebRequest();
        if(!(wr.isNetworkError || wr.isHttpError)) {
            Texture2D t = texDl.texture;
            Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height),
                                     Vector2.zero, 1f);
            _img.sprite = s;
        }
    }
}
```

## AssetBundle
```
using UnityEngine;
using UnityEngine.Networking; 
using System.Collections;
 
public class MyBehaviour : MonoBehaviour {
    void Start() {
        StartCoroutine(GetAssetBundle());
    }
 
    IEnumerator GetAssetBundle() {
        UnityWebRequest www = new UnityWebRequest("http://www.my-server.com");
        DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url, uint.MaxValue);
        www.downloadHandler = handler;
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            // 提取 AssetBundle
            AssetBundle bundle = handler.assetBundle;
        }
    }
}
```

## audioClip
```
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AudioDownloader : MonoBehaviour {

    void Start () {
        StartCoroutine(GetAudioClip());
    }

    IEnumerator GetAudioClip() {
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip("http://myserver.com/mysound.ogg", AudioType.OGGVORBIS)) {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError) {
                Debug.LogError(uwr.error);
                yield break;
            }

            AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
            // 使用音频剪辑
        }
    }   
}
```