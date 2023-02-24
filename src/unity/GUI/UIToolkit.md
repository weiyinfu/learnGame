# VisualElement注册回调
```plain
public void RegisterCallback<TEventType>(
  EventCallback<TEventType> callback,
  TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
  where TEventType : EventBase<TEventType>, new()
  
public void RegisterCallback<TEventType, TUserArgsType>(
  EventCallback<TEventType, TUserArgsType> callback,
  TUserArgsType userArgs,
  TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
  where TEventType : EventBase<TEventType>, new()
```
TEventType是一种缩写，它表示`EventBase<TEventType>`。
`TEventType : EventBase<TEventType>`

# 自定义事件
```csharp
    private void CreateGUI()
    {
        var l = new Label("one");
        l.RegisterCallback<MyEvent>(e => { });
        rootVisualElement.Add(l);
    }

    class MyEvent : EventBase<MyEvent>
    {
    }
```

# 在一个线程里面执行任务，任务执行结束之后，把事件返回给UI线程
核心在于使用`SynchronizationContext mainThreadSyncContext;`把主线程的上下文保存下来，然后执行post。  
通过`Thread.CurrentThread.ManagedThreadId`查看线程ID，发现确实是符合预期。  
```csharp
        public class ValueCallback
        {
            private Action<string> act;
            public string data;
            SynchronizationContext mainThreadSyncContext;

            public ValueCallback()
            {
                mainThreadSyncContext = SynchronizationContext.Current;
            }

            public void OnValue(Action<string> act)
            {
                if (data != null)
                {
                    act.Invoke(data);
                    return;
                }

                this.act = act;
            }

            public void Call(string value)
            {
                this.data = value; //设置data供以后可能会用到
                if (this.act != null)
                {
                    //如果在主线程中更新样式和
                    mainThreadSyncContext.Post(_ =>
                    {
                        Debug.Log($"=========POST内部：{Thread.CurrentThread.ManagedThreadId} {value}");
                        this.act.Invoke(value);
                    }, null);
                }
            }
        }
```

# runInUIThread
```plain
        static SynchronizationContext mainThreadSyncContext;

        static void runInUIThread(Action act)
        {
            mainThreadSyncContext.Post(_ => { act(); }, null);
        }

```

# C#中的线程池
```plain
            ThreadPool.QueueUserWorkItem((o) =>
            {
                var p = Process.Start(file, arguments);
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                p.StartInfo.RedirectStandardOutput = true;
                var res = p.Start();
                p.WaitForExit();
                var content = p.StandardOutput.ReadToEnd();
                callback.Invoke(content);
            });
```



# 自定义事件
```plain
    private void CreateGUI()
    {
        var value = 1;
        var l = new Label("one");
        l.RegisterCallback<MyEvent>(e => { l.text = $"{value}"; });
        rootVisualElement.Add(l);
        var b = new Button(() => { l.SendEvent(new MyEvent()); });
        b.text = "点我加一";
        rootVisualElement.Add(l);
        rootVisualElement.Add(b);
    }

    class MyEvent : EventBase<MyEvent>
    {
    }
```