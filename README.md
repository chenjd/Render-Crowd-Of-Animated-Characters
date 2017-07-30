# AnimMap Baker For Animated Characters 
----
#### 描述 Description：
利用GPU实现大规模动画角色的渲染。
* anim map for vertex shader to modify the vertex position of the mesh at runtime.

* use gpu instancing to reduce draw call.

#### 模型资源 ResLink：

[RTS Mini Legion Footman Handpainted](https://www.assetstore.unity3d.com/en/#!/content/86576)

---
#### Bake legacy animation info into anim map：

![1201111111111111.gif](http://upload-images.jianshu.io/upload_images/1372105-004a0ddd0f256df1.gif?imageMogr2/auto-orient/strip)



![120111111111111.gif](http://upload-images.jianshu.io/upload_images/1372105-35954dfd4ca03f7b.gif?imageMogr2/auto-orient/strip)
animated characters without animator & skinnedmeshrender.

#### GPU Instancing & mesh render for large number of animated characters：
![1372105-fa2cb8df2d12c0b3.gif](http://upload-images.jianshu.io/upload_images/1372105-310c57df8cfc83bc.gif?imageMogr2/auto-orient/strip)
10,000 animated characters with 22 drawcalls.

