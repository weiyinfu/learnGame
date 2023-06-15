在Unity中如果文件夹以~结尾，则该文件夹不会被assetDatabase打包，因此最终也不会被打包到编译产物中。  
例如：
* Documentation~
* Samples~

PacakgeManager导入包的时候可以选择性地导入Samples。只需要在JSON中写上Sample列表即可。  

```json
{
  "name": "com.unity.inputsystem",
  "displayName": "Input System",
  "version": "1.3.0",
  "unity": "2019.4",
  "description": "A new input system which can be used as a more extensible and customizable alternative to Unity's classic input system in UnityEngine.Input.",
  "keywords": [
    "input",
    "events",
    "keyboard",
    "mouse",
    "gamepad",
    "touch",
    "vr",
    "xr"
  ],
  "dependencies": {
    "com.unity.modules.uielements": "1.0.0"
  },
  "upmCi": {
    "footprint": "9b3507e700a475d3fc7c6406be352fd3f6b2e8e9"
  },
  "repository": {
    "url": "https://github.com/Unity-Technologies/InputSystem.git",
    "type": "git",
    "revision": "76d3fd182183dff9e9793431de614fd1e69e363c"
  },
  "samples": [
    {
      "displayName": "Custom Binding Composite",
      "description": "Shows how to implement a custom composite binding.",
      "path": "Samples~/CustomComposite"
    },
    {
      "displayName": "Custom Device",
      "description": "Shows how to implement a custom input device.",
      "path": "Samples~/CustomDevice"
    },
    {
      "displayName": "Custom Device Usages",
      "description": "Shows how to tag devices with custom usage strings that can be used, for example, to distinguish multiple instances of the same type of device (e.g. 'Gamepad') based on how the device is used (e.g. 'Player1' vs 'Player2' or 'LeftHand' vs 'RightHand').",
      "path": "Samples~/CustomDeviceUsages"
    },
    {
      "displayName": "Gamepad Mouse Cursor",
      "description": "An example that shows how to use the gamepad for driving a mouse cursor for use with UIs.",
      "path": "Samples~/GamepadMouseCursor"
    },
    {
      "displayName": "In-Game Hints",
      "description": "Demonstrates how to create in-game hints in the UI which reflect current bindings and active control schemes.",
      "path": "Samples~/InGameHints"
    },
    {
      "displayName": "InputDeviceTester",
      "description": "A scene containing UI to visualize the controls on various supported input devices.",
      "path": "Samples~/InputDeviceTester"
    },
    {
      "displayName": "Input Recorder",
      "description": "Shows how to capture and replay input events. Also useful by itself to debug input event sequences.",
      "path": "Samples~/InputRecorder"
    },
    {
      "displayName": "On-Screen Controls",
      "description": "Demonstrates a simple setup for an on-screen joystick.",
      "path": "Samples~/OnScreenControls"
    },
    {
      "displayName": "Rebinding UI",
      "description": "An example UI component that demonstrates how to create UI for rebinding actions.",
      "path": "Samples~/RebindingUI"
    },
    {
      "displayName": "Simple Demo",
      "description": "A walkthrough of a simple character controller that demonstrates several techniques for working with the input system. See the README.md file in the sample for details.",
      "path": "Samples~/SimpleDemo"
    },
    {
      "displayName": "Simple Multiplayer",
      "description": "Demonstrates how to set up a simple local multiplayer scenario.",
      "path": "Samples~/SimpleMultiplayer"
    },
    {
      "displayName": "Touch Samples",
      "description": "A series of sample scenes for using touch input with the Input System package. This sample is not actually part of the package, but needs to be downloaded.",
      "path": "Samples~/TouchSamples"
    },
    {
      "displayName": "UI vs. Game Input",
      "description": "An example that shows how to deal with ambiguities that may arrise when overlaying interactive UI elements on top of a game scene.",
      "path": "Samples~/UIvsGameInput"
    },
    {
      "displayName": "Visualizers",
      "description": "Several example visualizations of input controls/devices and input actions.",
      "path": "Samples~/Visualizers"
    }
  ]
}

```




# Unity包的Samples

```
{
  "name": "com.unity.timeline",
  "displayName": "Timeline",
  "version": "1.6.4",
  "unity": "2019.3",
  "keywords": [
    "unity",
    "animation",
    "editor",
    "timeline",
    "tools"
  ],
  "description": "Use Unity Timeline to create cinematic content, game-play sequences, audio sequences, and complex particle effects.",
  "dependencies": {
    "com.unity.modules.director": "1.0.0",
    "com.unity.modules.animation": "1.0.0",
    "com.unity.modules.audio": "1.0.0",
    "com.unity.modules.particlesystem": "1.0.0"
  },
  "relatedPackages": {
    "com.unity.timeline.tests": "1.6.4"
  },
  "upmCi": {
    "footprint": "dcac7462836a5fa5813cc1e2c6080df1da992327"
  },
  "repository": {
    "url": "https://github.cds.internal.unity3d.com/unity/com.unity.timeline.git",
    "type": "git",
    "revision": "d7e1eb6805737974459309b7d6e7db58635dd167"
  },
  "samples": [
    {
      "displayName": "Customization Samples",
      "description": "This sample demonstrates how to create custom timeline tracks, clips, markers and actions.",
      "path": "Samples~/Customization"
    },
    {
      "displayName": "Gameplay Sequence Demo",
      "description": "This sample demonstrates how Timeline can be used to create a small in-game moment, using built-in Timeline tracks.",
      "path": "Samples~/GameplaySequenceDemo"
    }
  ]
}
```
