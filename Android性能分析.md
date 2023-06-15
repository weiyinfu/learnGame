# 分析的分类

时间分析

空间分析：内存分析

# 内存分析

VSS：Vitual Set Size，虚拟耗用内存，一个进程能访问的所有内存空间地址的大小。不具有参考意义。

RSS：Resident Set Size，实际使用内存，一个进程在RAM中实际持有的内存大小。因为它包含了所有该进程使用的共享库所占用的内存，一个被加载到内存中的共享库可能有很多进程会使用它。RSS不是单个进程使用内存量的精确表示。

PSS：Proportional Set Size，实际使用的物理内存，它与RSS不同，它会按比例分配共享库所占用的内存。例如，如果有三个进程共享一个占30页内存控件的共享库，每个进程在计算PSS的时候，只会计算10页。PSS是一个非常有用的数值，如果系统中所有的进程的PSS相加，所得和即为系统占用内存的总和。当一个进程被杀死后，它所占用的共享库内存将会被其他仍然使用该共享库的进程所分担。在这种方式下，PSS也会带来误导，因为当一个进程被杀后，PSS并不代表系统回收的内存大小。

USS：Unique Set Size，进程独自占用的物理内存。这部分内存完全是该进程独享的。USS是一个非常有用的数值，因为它表明了运行一个特定进程所需的真正内存成本。当一个进程被杀死，USS就是所有系统回收的内存。USS是用来检查进程中是否有内存泄露的最好选择。

综上可知，RSS、PSS、USS都在围绕着共享库的大小进行讨论。RSS是完全包含了共享库的大小，PSS是把共享库的大小分开计算，USS是完全不算共享库的大小。

- USS<PSS<RSS

- RSS高，USS低，说明共享库有点大

- PSS跟RSS非常接近，说明这个共享库几乎是被独占的，并没有被多个应用共享

# Android内存统计工具

## dumpsys meminfo

adb shell dumpsys meminfo --package com.tencent.xxx

## 腾讯的perfdog

## 字节的gameperf

https://gtl.bytedance.com/LoginPage

## AndroidStudio Tools菜单中的分析

# 参考资料

https://news.16p.com/865025.html
