Unity定义了一个输入系统，好处：
* 可以为每个输入起一个名字
* 不同设备上的输入不需要修改代码，只需要修改配置即可。  

在Edit/Project Settings/InputManager下面，可以添加或者删除Axes（在Axes上右键）。  

```
if (Input.GetButtonDown("Left"))
{
    if (MoveTilesLeft())
    {
        state = State.CheckingMatches;
    }
}
else if (Input.GetButtonDown("Right"))
{
    if (MoveTilesRight())
    {
        state = State.CheckingMatches;
    }
}
else if (Input.GetButtonDown("Up"))
{
    if (MoveTilesUp())
    {
        state = State.CheckingMatches;
    }
}
else if (Input.GetButtonDown("Down"))
{
    if (MoveTilesDown())
    {
        state = State.CheckingMatches;
    }
}
else if (Input.GetButtonDown("Reset"))
{
    Reset();
}
else if (Input.GetButtonDown("Quit"))
{
    Application.Quit();
}
```