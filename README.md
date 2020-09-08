# AnimMap Baker For Animated Characters 
![GitHub](https://img.shields.io/github/license/chenjd/Render-Crowd-Of-Animated-Characters)
![](https://img.shields.io/badge/platform-win--64-brightgreen)
![](https://img.shields.io/badge/unity-2019.4%2B-brightgreen)
![Build my project 😎](https://github.com/chenjd/Render-Crowd-Of-Animated-Characters/workflows/Build%20my%20project%20%F0%9F%98%8E/badge.svg)

#### Description：
Updated to Unity 2019 or later.

Using GPU to achieve large number of animated characters rendering.

* anim map for vertex shader to modify the vertex position of the mesh at runtime.

* use gpu instancing to reduce draw call.

#### Model Resource Link：

[RTS Mini Legion Footman Handpainted](https://www.assetstore.unity3d.com/en/#!/content/86576)

---
#### Bake legacy animation info into anim map：

![1201111111111111.gif](http://upload-images.jianshu.io/upload_images/1372105-004a0ddd0f256df1.gif?imageMogr2/auto-orient/strip)



![120111111111111.gif](http://upload-images.jianshu.io/upload_images/1372105-35954dfd4ca03f7b.gif?imageMogr2/auto-orient/strip)
animated characters without animator & skinnedmeshrender.

#### GPU Instancing & mesh render for large number of animated characters：
![1372105-fa2cb8df2d12c0b3.gif](http://upload-images.jianshu.io/upload_images/1372105-310c57df8cfc83bc.gif?imageMogr2/auto-orient/strip)
10,000 animated characters with 22 drawcalls.

#### BlogPost

[How To Render 10,000 Animated Characters With 20 Draw Calls In Unity](https://medium.com/@chen_jd/how-to-render-10-000-animated-characters-with-20-draw-calls-in-unity-e30a3036349a)

