有一个EditorWindow类型，使用了新版的UnityGUI。需要加载UXML和CSS，UXML和CSS的路径与C#脚本在同一目录下面，如何不管脚本的位置，动态加载呢？
关键在于获取C#脚本文件所在的位置。

解决方案如下：
* 传入fileName（不带后缀名）。
* 在AssetDatabase中搜索fileName，得到GUID列表
* 如果找到的paths个数为0，表示没有找到；如果大于1表示找到了多个，文件名称起得不够特殊。
* 根据GUID寻找资源的详细路径

```csharp

    string getScriptPath(string fileName)
    {
        string[] paths = AssetDatabase.FindAssets(fileName);
        if (paths.Length > 1)
        {
            Debug.LogError("multi file is found");
            return null;
        }

        if (paths.Length == 0)
        {
            Debug.LogError($"cannot find {fileName}");
            return null;
        }

        string realPath = AssetDatabase.GUIDToAssetPath(paths[0]);
        Debug.Log($"getScriptPath {fileName} {realPath}");
        return Path.GetDirectoryName(realPath);
    }

```