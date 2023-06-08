Unity做的相当一般，很多模块都是残次品，有点抛砖引玉的感觉。  

# Json
Unity自带的JSON基本上不可用，对于`public readonly`字段无法序列化出来，对于数组类型也无法序列化。  

# UGUI
GUI界面简单，只能用来做简单的游戏。  

# 对象树许多关键函数设计不合理
很多常见的函数是缺失的。  
* 从子物体中寻找物体
* 从inactive物体中寻找物体

# so
如果so中导致崩溃，unity editor会直接崩溃。  
原因是unity的editor进程与运行游戏的进程是同一个进程，游戏的卡顿会导致editor卡顿。  

# 工程的路径
unity对于包含中文的路径会报错，不能把unity项目放在包含中文的路径中。  

# Unity的Collider没有圆柱体的collider