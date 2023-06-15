Timeline可以在UnityEditor中进行时间调度。

一个Timeline=若干个Track

一个Track=一个Track对象+若干个TrackClip，Track对象就是TrackClip所作用的对象。

# 概念

TimeLine是Unity中比较综合的概念，从TimeLine出发可以牵扯出许多Unity概念。

Track Clip：片段，一个片段有起始时间、结束时间。

Track：轨道的概念，一个轨道控制一件事情，在一个轨道上可以添加多个片段。

Player Director：播放导演，是一个组件，持有一个PlayableAsset。Player Director=Timeline+Bindings。

Signal Emitter：发射信号的组件

Signal Receiver：接收信号的组件

Signal：是一种资源文件，描述了一种信号。Signal Emitter发送信号，Signal Receiver接收信号。

Playable：Playable是PlayerDirector所持有的主要对象，Timeline就是一个Playable对象。PlayableAsset和PlayableBehaviour也都是Playable对象。Timeline是PlayableAsset对象。

PlayableBehaviour：包含一堆回调函数

PlayableAsset：PlayableBehaviour的工厂方法。

Animator：负责播放动画，接收一个Animate资源文件

Animator Controller：负责动画之间的切换

Animate：一种资源文件

# 基本步骤

1. 创建Timeline资源，编辑Timeline的右侧时间条

2. 在一个GameObject上创建PlayerDirector，在PlayerDirector上绑定Timeline和bindings。

# 轨道类型

* TrackGroup

* ActivationTrack

* AudioTrack

* ControlTrack

* PlayableTrack

* SignalTrack



1. Activation Track，与游戏对象的Activate属性进行绑定。

2. Animation Track，与动画进行绑定，到达时间之后游戏开启某种动画

3. Audio Track，设置一个AudioSource，表示声音从何处播放。一个Audio Track可以设置多个AudioClip。

4. Control Track

5. Playable Track：用户自定义Playable事件

6. Signal Track：可以发射信号，触发事件。右键在轨道上添加SignalEmitter，SignalEmitter绑定一个Signal。当SignalTrack触发信号的时候，只有当前Track绑定的GameObject会收到信号，可以指定这个GameObject收到信号的回调函数。

Timeline有两类自定义：

1. 使用PlayableTrack：自定义轨道的Playable

2. 自定义Track：这是一种更为彻底的定制。

Timeline提供的这些Track基本上覆盖了大多数情况

使用SignalTrack可以设置回调函数，这基本上就足够使用了。

# PlayableDirector

WrapMode：循环模式

- hold：播放完一遍就停止且停止在最后一帧

- loop：无限循环

- None：播放完成后回到第一帧并停止播放

# 自定义轨道

自定义轨道是对Unity Editor进行定制。

继承TrackAsset就会在Timeline中出现这个可选的Track。

```C#
using UnityEngine; 
using UnityEngine.Timeline;  
public class DialogueTrack : TrackAsset {        } 
```

TrackAsset有三个注解：

1. TrackBindingTypeAttribute：向轨道拖放无提示，执行绑定类型检查。例如，若物体不含有Light组件，则在拖放物体时，自动添加绑定的组件类型。

```C#
using UnityEngine; 
using UnityEngine.Timeline;  
[TrackBindingType(typeof(Light), TrackBindingFlags.AllowCreateComponent)] 
public class LightTrack : TrackAsset {        }
```

2. TrackClipType：可以拖放的片段类型，在Timeline编辑器的右侧。

```C#
using UnityEngine; 
using UnityEngine.Timeline;  
[TrackClipType(typeof(DialogueClip))] 
public class DialogueTrack : TrackAsset {       } 
```

3. TrackColor：指定轨道边界的颜色

```C#
using UnityEngine; 
using UnityEngine.Timeline;  
[TrackColor(1.0f, 0.0f, 0.0f)]            
// red public class DialogueTrack : TrackAsset {       
}
```

# Playable Track

实现PlayableBehavior

```
[System.Serializable]
  public class LightData : PlayableBehaviour{
    
      public float range;
      public Color color;
      public float intensity;
      [HideInInspector]
      public Transform lookTarget;

      public override void OnPlayableCreate(Playable playable){

          var duration = playable.GetDuration();

          if(Mathf.Approximately((float)duration, 0)){
              throw new UnityException("A Clip Cannot have a duration of zero");
          }

      }
  }

```



实现PlayableAsset

```
  [System.Serializable]
  public class LightClip : PlayableAsset {

      public LightData templete = new LightData();
      public ExposedReference<Transform> lookTarget;

      // Factory method that generates a playable based on this asset
      public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
          var playable = ScriptPlayable<LightData>.Create(graph, templete); 
          LightData clone = playable.GetBehaviour();
          clone.lookTarget = lookTarget.Resolve(graph.GetResolver());
          return playable;
      }

  }
```

# 参考资料

https://zhuanlan.zhihu.com/p/513872343
