# 每帧检查
如一下代码所示，每一秒钟执行一次。  
因为Update()不够准确，所以这种定时方式时间长了就会不准。  
```csharp
public float timer = 1.0f;
// Update is called once per frame
void Update() {
    timer -= Time.deltaTime;
    if (timer <= 0) {
        Debug.Log(string.Format("Timer1 is up !!! time=${0}", Time.time));
        timer = 1.0f;
    }
}
```

# 使用协程
```csharp
void Start() {
    StartCoroutine(Timer());
}
IEnumerator Timer() {
    while (true) {
        yield return new WaitForSeconds(1.0f);
        Debug.Log(string.Format("Timer2 is up !!! time=${0}", Time.time));
    }
}
```

# 使用协程的延时调用

```csharp
void Start() {
    Invoke("Timer", 1.0f);
}
void Timer() {
    Debug.Log(string.Format("Timer3 is up !!! time=${0}", Time.time));
    Invoke("Timer", 1.0f);
}
```