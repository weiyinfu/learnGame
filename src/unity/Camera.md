# 获取camera
```
Camera mainCamera;//使用public字段，在Unity Editor里面绑定
GameObject gameObject=GameObject.Find("MainCamera");
mainCamera=gameObject.GetComponent<Camera>();
camera的位置和尺寸
camera.transform.posision//获取位置
camera.rect//视区的尺寸
```
