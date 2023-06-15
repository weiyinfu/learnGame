# 动画的实现方式

1. 在Unity引擎中使用Animator系列制作动画，Unity编辑器提供了一系列动画制作工具

2. 使用外部工具3dmax、maya制作动画，导入到Unity

3. 使用代码直接修改GameObject的Transform实现动画，通常使用协程实现。布娃娃系统是使用代码实现动画的典型例子。使用代码控制动画的好处是能够实现最大的灵活性，例如让一个小球在某个半径内随机运动，如果使用关键帧动画就无法做到完全随机。

本文只介绍在Unity引擎中制作动画。



# Unity旧版动画系统

Unity是一个不断发展的游戏引擎，它的很多模块都在与时俱进。

1. 输入系统从InputManager到InputSystem

2. UI系统从Unity UI到Element新版UI

3. 基础架构从面向对象到DOTS

Unity的动画系统也有两套，旧版animation动画系统和新版的Animator动画系统，其中Animator动画系统也叫Mecanim动画系统。因为旧版的动画系统叫animation，新版的动画系统只能找一个动画的同义词，也就是mecanim。旧版的animation比新版的动画系统简单很多，个人认为不该删掉，两者并存更好。

旧版的Animation组件直接持有一个Animation对象，新版的Animator持有一个AnimateController。



# Macanim动画系统的特点

1. 面向动画应用的动画系统，使用3dmax、maya可以导出fbx文件，unity中直接导入动画文件

2. 基于状态机的动画控制系统

3. 核心是关键帧动画。

Mecanim最初与人形角色动画密切相关，后来经过扩充可以适用于其它动画。

# 动画相关的组件

- Animate：也叫AnimationClip，动画本身，是一种资源文件，后缀名为.animate，像视频一样。这种资源有两种来源：1. 使用Animation窗口进行编辑；2.从外部导入动画。

- AnimateController：一个有限状态自动机，持有若干个Animate。AnimateController也是一种资源文件，后缀名为.controller.可以设置进行状态转移的条件。

- Animator：MonoBehavior，一个GameObject需要持有一个Animator才能播放动画。Animator依赖AnimateController。

Animator持有AnimateController，AnimateController是一个状态机，每个结点都是Animate动画。AnimateController和Animate都是资源文件。

# 动画与Playables与Timeline的关系

Playables API是动画系统的底层接口，Animator是基于Playables API的封装。

AnimateController底层实现基于PlayableGraph，所以可以使用状态机描述。

- Playables API 允许动态动画混合。这意味着对象在**场景** 可以提供自己的动画。例如，武器、箱子和陷阱的动画可以动态添加到 PlayableGraph 并使用一定的持续时间。

- Playables API 允许您轻松播放单个动画，而无需创建和管理 AnimatorController 资产所涉及的开销。

- Playables API 允许用户动态创建混合图并直接逐帧控制混合权重。

- PlayableGraph 可以在运行时创建，根据需要添加可播放节点。与启用和禁用节点的巨大“一刀切”图不同，PlayableGraph 可以进行定制以适应当前情况的要求。

# Animation窗口

选择GameObject，然后添加属性，创建关键帧。

有两种模式：

1. 录制模式，可以录制物体的属性变化

2. 关键帧模式，手动创建关键帧，直接编辑属性

在底部，Animation窗口有两种视图，一个是Dopesheet（关键帧），另一个是Curbes（曲线模式）。可以使用曲线编辑器编辑动画。

# AnimationController窗口

Entry为入口

Exit为出口

Entry会默认连接一个结点，这个默认结点就是最初的动画，这条线为橙色。可以右键结点，选择Set as Layer default State.

Any State：任意状态，是一个始终存在的特殊状态。

SubState Machine：子状态机，相当于包含多个结点的一个子状态机，它是为了便于整理状态。

Solo和Mute功能，这个功能主要是用于测试，使用Solo仅启用勾选了Solo的动画过渡。

为什么需要AnyState？例如角色有一个死亡状态，从跑步、吃饭、运动三个状态都可以到达死亡状态，那么需要画三条线：跑步-死亡，吃饭-死亡，运动-死亡。有了AnyState只需要画一条线，AnyState-死亡。

AnimationController中的每一个结点都有一些属性：

1. Motion：结点对应的动画

2. Speed：动画播放速度

3. Multiplier：用于平滑播放动画

4. Mirror：仅适用于人形动画，表示是否使用动画的镜像

5. Cycle Offset：动画其实偏移量，取值0到1

6. Foot IK：人形动画是否启用脚部IK

7. Transision：该状态向其它状态的转移列表，其中的Solo和Mute两个参数用于调试。开启Solo之后表示该State只能进行这一种过渡，其它过渡暂时关闭；Mute表示这个过渡暂时关闭，只能进行其它过渡。

8. AddBehaviour：用于向状态机添加行为

# Animator

ApplyRootMotion：是否应用根对象的位移。勾选后, 在动画播放期间, 物体的运动相关参数完全由动画本身接管, 此时脚本控制无效. 取消勾选后, 则是由脚本来控制物体的运动参数。脚本中实现了OnAnimatorMove，相当于勾选了ApplyRootMotion，可以在脚本中控制位置和旋转。



```
deltaPosition：相对上一帧的位置变化量（必须允许根运动才能被计算）
deltaRotation：相对上一帧的角度变化量（必须允许根运动才能被计算）
    void OnAnimatorMove()
    {
        var transform1 = transform;
        transform1.position += animator.deltaPosition;
        transform1.rotation *= animator.deltaRotation;
    }
```

CullingMode：剔除模式，可用于提升性能。取值如下：

- Always Animate：始终渲染

- Cull Update Transforms：当物体不被摄像机可见时，仅计算根节点的位移，只保证位置正确

- Cull Completely：当物体不可见时，完全停止动画。

# 骨骼-蒙皮动画

人类的很多研究都是针对人类本身进行的，例如人体骨骼动画，人类面部识别。

游戏中有很多角色，每个角色有自己的样式。

一个角色包括：骨骼、蒙皮、动画。角色的动画系统非常复杂，可能是多个骨骼同时运动。

动画控制的是骨骼的关键帧，蒙皮跟着动，角色就动起来了。



Animation的时序

- ProcessAnimations 读取骨骼信息

- FireAnimationEventsAndBehaviours 读取动画事件

- ApplyOnAnimatorMove 根节点应用动画信息

- WriteAnimatedValues 动作数值写入

- DirtySceneObjects 对骨骼的transform进行更新写入

- MeshSkinning.CalcMatrices 计算蒙皮矩阵

- ScheduleGeometryJobs 子线程处理

- MeshSkinning.Skin 计算模型，网格顶点位置

- MeshSkinning.Render 渲染
  
  - PutGeometryJobFench 几何计算
  
  - Mesh.DrawVBO DrawCall调用

角色模型性能优化：

1. 导入人形动画时，如果不需要IK，使用Avatar遮罩将其移除

2. 取消掉Update When Offscreen，在不可见时不用更新动画。

# Unity外部资源

使用其它软件制作复杂模型，然后导入Unity，这是一种常见的工作方式。

Blender、AutoDesk、Maya是三种比较常见的建模软件。

fbx文件是一种常见的3D文件格式。

# 动画复用：Avatar化身系统

在制作骨骼蒙皮动画的时候，可以为骨骼设置动画。能否将一套骨骼动画应用于两个不同的角色？可以，这就是Avatar化身系统，它的作用是将一套动画应用于多个模型。

# 分层和遮罩

动画分层意思是把两个动画进行组合，例如上半身动画和下半身动画分开播放。

# Animator API

GetCurrentAnimatorStateInfo

GetNextAnimatorStateInfo

animator.Play("动画状态名称")

animator.Update(duration) 将动画时间更新到duration之后

animatorController结点的动画重写：

```
Animator animator = GetComponent<Animator>();
AnimatorOverrideController overrideController = new AnimatorOverrideController();
overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
overrideController["name"] = newAnimationClip;
animator.runtimeAnimatorController = overrideController;
```

检查动画的状态

```
//检查是否正在播放jump动画.
AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);   
bool playingJump = stateinfo.IsName("jump");
if(playingJump)
{
    if(stateinfo.normalizedTime < 1.0f)
    {
        //正在播放
    }
    else
    {
        //播放结束
    }
     
}
```



为AnimatorController中的每个结点添加脚本

AnimatorController的每个状态都可以挂载脚本，只需要继承StateMachineBehaviour

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class JumpState : StateMachineBehaviour
{
    private GameObject player;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 正在played的状态的第一帧被调用
        Debug.Log("------OnStateEnter------------");
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 转换到另一个状态的最后一帧 被调用
        Debug.Log("-------------OnStateExit-----------------");
    }
    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            // 在OnAnimatorMove之前被调用 
        
    }
    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 在OnAnimatorIK之后调用，用于在播放状态时的每一帧的monobehavior。
        // 需要注意的是，OnStateIK只有在状态位于具有IK pass的层上时才会被调用。
        // 默认情况下，图层没有IK通道，所以这个函数不会被调用
        // 关于IK的使用，可以看看这篇文章《Animator使用IK实现头部及身体跟随》
        // https://www.jianshu.com/p/ae6d65563efa
    }
}

```

使用代码控制播放速度

```
Animator ator = go1.GetComponent<Animator>();
var stateinfo = ator.GetCurrentAnimatorStateInfo(0);
if(stateinfo.IsName("Jump"))
{
    ator.speed = 2;
}
```

# 动画事件

动画事件用于通知GameObject当前正在播放何种动画，可以携带float、int、string、object四个参数。

# Animator IK

在动画中应用逆向骨骼。

# MatchTarget

当人的手抓住某个位置时，人的手位置固定，动画继续播放。

例如，扣篮的时候手与篮筐相对静止，身体绕着篮筐旋转。



```
    public Animator ani;
    public Transform LeftHand;
    bool hasJump = false;
    void Start () {
        ani = GetComponent<Animator>();
    }
    
    
    void Update () {
        if (ani)
        {
            AnimatorStateInfo info = ani.GetCurrentAnimatorStateInfo(0);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ani.SetBool("Jump", true);
              
            }
            if (info.IsName("Base Layer.Vault"))
            {
                ani.SetBool("Jump", false);
                // 第一个参数动作位置，第二个参数角色旋转，第三个是做动作的某个身体部位，第四个是权重信息，第五六参数是获取动画曲线
                ani.MatchTarget(LeftHand.position, LeftHand.rotation, AvatarTarget.LeftFoot, new MatchTargetWeightMask(new Vector3(1, 1, 1), 0), ani.GetFloat("StartA"), ani.GetFloat("EndA"));
                hasJump = true;
            }
        }
    }
}
```

# 使用代码根据fbx创建状态机

```
using System.Collections;
using UnityEditor;
using UnityEditor.Animations;//5.0改变 UnityEditorInternal;并不能用了。
 
public class CreateAnimatorController : Editor 
{
    [MenuItem("ModelConfig/创建Controller")]
    static void DoCreateAnimationAssets()
    {
        //创建Controller
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath("Assets/animation.controller");
        //得到它的Layer
        AnimatorControllerLayer layer = animatorController.layers[0];
        //将动画保存到 AnimatorController中
        AddStateTransition("Assets/Art Resources/Character/moster-002/basic/moster-002@run.FBX", layer);
        AddStateTransition("Assets/Art Resources/Character/moster-002/basic/moster-002@stand.FBX", layer);
        AddStateTransition("Assets/Art Resources/Character/moster-002/basic/moster-002@born.FBX", layer);
    }
 
    private static void AddStateTransition(string path, AnimatorControllerLayer layer)
    {
        AnimatorStateMachine sm = layer.stateMachine;
        //根据动画文件读取它的AnimationClip对象
        AnimationClip newClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
        
        ////取出动画名子 添加到state里面
        AnimatorState state = sm.AddState(newClip.name);
        //5.0改变
        state.motion = newClip;
        Debug.Log(state.motion);
        //把state添加在layer里面
        AnimatorStateTransition trans = sm.AddAnyStateTransition(state);
    }
}
```

# 参考资料

https://zhuanlan.zhihu.com/p/492136094

https://www.jb51.net/article/221837.htm

Unity官方文档：https://docs.unity3d.com/cn/2021.3/Manual/AnimationOverview.html
