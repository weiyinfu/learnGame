
# 进度条更新太快
不要使用onvaluechange，而要使用OnEventTriggerEvent，它表示结束拖动滚动条。

onValueChanged变化太快：
```plain
        progress.onValueChanged.AddListener(v =>
        {
            var p = videoPlayerObj.GetComponent<VideoPlayer>();
            if (p.canSetTime)
            {
                p.time = v * p.length;
            }
        });
```

C#中使用扩展方法为所有的UI空间指定事件回调：
```plain

public static class UIBehaviourExtension
{
    /// <summary>
    /// 添加EventTrigger事件监听
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="type"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static T OnEventTriggerEvent<T>(this T self, EventTriggerType type, UnityAction<BaseEventData> action) where T : UIBehaviour
    {
        var eventTrigger = self.GetComponent<EventTrigger>() ?? self.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        eventTrigger.triggers.Add(entry);
        return self;
    }
}
```
