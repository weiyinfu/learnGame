# 脚本方面
1. 不需要高频调用的，使用InvokeRepeating或者Time.frameCount%n，总之添加一些频控。
2. Dictionary使用TryGet代替ContainsKey
3. 使用对象池