Unity Test Framework是Unity的测试模块，简称UTF。  

UTF提供了一些定制能力，可以在[模块介绍](https://docs.unity.cn/Packages/com.unity.test-framework@1.1/manual/extension-retrieve-test-list.html)中找到。  

# 获取测试用例的列表
```csharp
var api = ScriptableObject.CreateInstance<TestRunnerApi>();
api.RetrieveTestList(TestMode.EditMode, (testRoot) =>
{
    Debug.Log(string.Format("Tree contains {0} tests.", testRoot.TestCaseCount));
});
```

# 获取测试结果
创建一个TestRunnerApi对象，然后给这个对象注册一个Callback，在用例运行的时候处理这些回调。
例如在测试结束的时候可以获取测试结果。  
```csharp
public void SetupListeners()
{
   var api = ScriptableObject.CreateInstance<TestRunnerApi>();
   api.RegisterCallbacks(new MyCallbacks());
}

private class MyCallbacks : ICallbacks
{
    public void RunStarted(ITestAdaptor testsToRun)
    {

    }

    public void RunFinished(ITestResultAdaptor result)
    {

    }

    public void TestStarted(ITestAdaptor test)
    {

    }

    public void TestFinished(ITestResultAdaptor result)
    {
        if (!result.HasChildren && result.ResultState != "Passed")
        {
            Debug.Log(string.Format("Test {0} {1}", result.Test.Name, result.ResultState));
        }
    }
}
```


# 通过程序运行测试用例而不是通过TestRunner窗口
只运行Play模式下的用例
```csharp
var testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
var filter = new Filter()
{
    testMode = TestMode.PlayMode
};
testRunnerApi.Execute(new ExecutionSettings(filter));

```
指定用例名称
```csharp
var api = ScriptableObject.CreateInstance<TestRunnerApi>();
api.Execute(new ExecutionSettings(new Filter()
{
    testNames = new[] {"MyTestClass.NameOfMyTest", "SpecificTestFixture.NameOfAnotherTest"}
}));

```
通过Assembly和用例名称同时过滤测试用例。在此例中，选择满足assemby和testNames两个条件的用例来运行。  
```csharp
var api = ScriptableObject.CreateInstance<TestRunnerApi>();
api.Execute(new ExecutionSettings(new Filter()
{
    assemblyNames = new [] {"MyTestAssembly"},
    testNames = new [] {"MyTestClass.NameOfMyTest", "MyTestClass.AnotherNameOfATest"}
}));

```

ExecutionSettings可以指定多个Filter，多个Filter之间满足任何一个就能够执行。
```csharp
var api = ScriptableObject.CreateInstance<TestRunnerApi>();
api.Execute(new ExecutionSettings(
    new Filter()
    {
        assemblyNames = new[] {"MyTestAssembly"},
    },
    new Filter()
    {
        testNames = new[] {"MyTestClass.NameOfMyTest", "MyTestClass.AnotherNameOfATest"}
    }
));
```

# 修改编译选项
实现ITestPlayerBuildModifier接口。
```csharp
using UnityEditor;
using UnityEditor.TestTools;

[assembly:TestPlayerBuildModifier(typeof(BuildModifier))]
public class BuildModifier : ITestPlayerBuildModifier
{
    public BuildPlayerOptions ModifyOptions(BuildPlayerOptions playerOptions)
    {
        if (playerOptions.target == BuildTarget.iOS)
        {
            playerOptions.options |= BuildOptions.SymlinkLibraries; // Enable symlink libraries when running on iOS
        }

        playerOptions.options |= BuildOptions.AllowDebugging; // Enable allow Debugging flag on the test Player.
        return playerOptions;
    }
}
```
只进行编译生成apk，不运行
```csharp
using System;
using System.IO;
using System.Linq;
using Tests;
using UnityEditor;
using UnityEditor.TestTools;
using UnityEngine;
using UnityEngine.TestTools;

[assembly:TestPlayerBuildModifier(typeof(HeadlessPlayModeSetup))]
[assembly:PostBuildCleanup(typeof(HeadlessPlayModeSetup))]

namespace Tests
{
    public class HeadlessPlayModeSetup : ITestPlayerBuildModifier, IPostBuildCleanup
    {
        private static bool s_RunningPlayerTests;
        public BuildPlayerOptions ModifyOptions(BuildPlayerOptions playerOptions)
        {
            // Do not launch the player after the build completes.
            playerOptions.options &= ~BuildOptions.AutoRunPlayer;

            // Set the headlessBuildLocation to the output directory you desire. It does not need to be inside the project.
            var headlessBuildLocation = Path.GetFullPath(Path.Combine(Application.dataPath, ".//..//PlayModeTestPlayer"));
            var fileName = Path.GetFileName(playerOptions.locationPathName);
            if (!string.IsNullOrEmpty(fileName))
            {
                headlessBuildLocation = Path.Combine(headlessBuildLocation, fileName);
            }
            playerOptions.locationPathName = headlessBuildLocation;

            // Instruct the cleanup to exit the Editor if the run came from the command line. 
            // The variable is static because the cleanup is being invoked in a new instance of the class.
            s_RunningPlayerTests = true;
            return playerOptions;
        }

        public void Cleanup()
        {
            if (s_RunningPlayerTests && IsRunningTestsFromCommandLine())
            {
                // Exit the Editor on the next update, allowing for other PostBuildCleanup steps to run.
                EditorApplication.update += () => { EditorApplication.Exit(0); };
            }
        }

        private static bool IsRunningTestsFromCommandLine()
        {
            var commandLineArgs = Environment.GetCommandLineArgs();
            return commandLineArgs.Any(value => value == "-runTests");
        }
    }
}
```

