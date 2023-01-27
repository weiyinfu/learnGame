# Android系统上 Unity的TestRunner的原理
创建一个虚拟场景，这个虚拟场景挂上测试脚本，在Android机器上运行这个场景。  


# 使用
在Window/General/TestRunner中， 点击TestRunner窗口的右上角：RunSelectedTests进行运行。也可以选择RunAllTests。这个测试过程也是需要编译的，比较慢。  
TestRunner有两种模式：
* Editor：编辑器模式
* Play：真机模式

# 创建测试用例
直接右键创建，选择Testing，可以选择EditorMode和PlayMode的C#脚本。  
在Window/General/TestRunner窗口中，底部也有创建测试用例的按钮。  

其实 `[Test]` 的注解就是普通的测试标签，`[UnityTest]`标签才是 PlayMode 测试用例的标签，同时该注解下的函数返回类型是个迭代器 IEnumerator，我们注意到该函数内部还有一条语句 yield return null。其实还有类似的写法，如 yield return new WaitForSeconds()、 yield return new WaitForEndOfFrame()、 yield return new WaitForFixedUpdate() 等。

在EditorMode下，一般使用`[Test]`属性较多。  

PlayMode下的测试用例为什么返回值是IEnumerator？因为Unity的测试用例是一帧一帧执行的。一旦执行yield return null;则后面的代码在下一帧的时候才会执行。这种执行机制与Unity游戏刷新机制相似，从而便于测试。  


# 从命令行运行单元测试
<https://docs.unity.cn/Packages/com.unity.test-framework@1.1/manual/reference-command-line.html>
```plain
Unity.exe -runTests -batchmode -projectPath PATH_TO_YOUR_PROJECT -testResults C:\temp\results.xml -testPlatform PS4
```