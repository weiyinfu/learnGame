https://learn.unity.com/tutorial/shi-yao-shi-dots-wei-shi-yao-shuo-dotsfei-chang-zhong-yao?language=zh#

DOTS：Data Oriented Tech Stack，面向数据的技术栈。

DoD：Data Oriented Design。

OOD：Object Oriented Design。

作用：提升游戏性能，目前的架构阻碍了游戏性能的优化。

定位：Unity的下一代架构，是Unity的未来架构、最终架构。不仅仅是Unity，Unreal也正在转向DOTS。DOTS系统是游戏开发领域的开发范式的一次革命，影响巨大。DOTS之后，会产生新的一批有影响力的大型游戏。

在现在的游戏开发中，每一个GameObject都有自己的数据和行为，这叫面向对象。DOTS认为，一个游戏只需要定义一大堆全局变量就足够了，游戏逻辑通过函数控制。说白了，DOTS等于面向过程的卷土重来。

包括三部分，可以看出这三部分都是为了优化性能：

- ECS：Entity Component System，组件化、数据化的开发模式。它的主要思想就是对数据结构加以组织，提高缓存命中率。Entity+Component都是数据，System是行为。

- Job System：更加安全、简便地实现多线程。充分利用多核。目前Unity渲染都是单线程的，没有充分利用多核进行渲染。

- Burst编译：基于LLVM的高效编译器

那么DOTS到底能够提升多大的性能呢？根据官方Demo估算，大概有10倍性能提升。

在DOTS的三个核心部分中，只有ECS会影响到开发者的代码。这一部分的主要思路就是：放弃面向对象，改为面向数据，大量地使用全局变量，游戏画面只是针对这些全局变量的渲染。

Unity正在使用DOTS革自己的命，为什么要放弃面向对象？

1. 多人游戏场景下，游戏对象太多非常复杂，影响性能。

2. DOTS能够模拟大量的、成千上万的Agent。

3. 面向对象的GameObject总是在渲染，而DOTS则能够做到局部渲染，只渲染用户视野范围内的物体即可。

# Megacity

巨型都市，Unity的一个Demo，用于演示DOTS的强大功能。

# 包

DOTS系统三个包，分别对应DOTS的三个重要组成部分：

Burst：对应Burst

Entities：对应Entity

Jobs：对应Jobs
