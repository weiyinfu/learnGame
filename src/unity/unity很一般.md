Unity做的相当一般，很多模块都是残次品，有点抛砖引玉的感觉。  

# Json
Unity自带的JSON基本上不可用，对于`public readonly`字段无法序列化出来，对于数组类型也无法序列化。  

# UGUI
GUI界面原始，非常难用。  

# 对象树许多关键函数设计不合理
* 从子物体中寻找物体
* 从inactive物体中寻找物体

# so
如果so中导致崩溃，unity editor会直接崩溃。  

# 工程的路径
unity对于包含中文的路径会报错，不能把unity项目放在包含中文的路径中。  