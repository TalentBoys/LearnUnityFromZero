**本教程人写版发布于B站专栏，搜索“金鱼大王吧”，markdown大部分都是AI写的，只做略微修改**

# Unity 游戏开发教程大纲

> 本教程面向有编程基础、首次接触游戏开发的学习者，使用 AI 辅助完成从零到上架 Steam 的全流程。

---

## 阶段1：Unity基础 + C#入门（2-4周）

### 第1课：Unity 核心概念
- [ ] Scene（场景）、GameObject（游戏对象）、Component（组件）概念
- [ ] Transform、生命周期函数（Awake/Start/Update/FixedUpdate）
- [ ] 编写第一个 C# 脚本，让物体动起来
- [ ] 理解 Inspector 面板和组件参数调节

### 第2课：C# 游戏开发基础
- [ ] Unity 中常用的 C# 模式（MonoBehaviour、SerializeField、协程）
- [ ] Vector3、Quaternion 基础数学
- [ ] 输入系统（Input System）
- [ ] Debug 与日志

### 第3课：物理系统
- [ ] Rigidbody / Rigidbody2D
- [ ] Collider 与触发器（Trigger）
- [ ] 碰撞检测（OnCollisionEnter / OnTriggerEnter）
- [ ] 物理材质

### 第4课：预制体与资源管理
- [ ] Prefab 工作流（创建、实例化、变体）
- [ ] 资源文件夹结构规范
- [ ] ScriptableObject 数据驱动
- [ ] Asset Store / 免费资源获取

---

## 阶段2：2D 游戏实战（3-4周）

> 目标：完成一个完整的 2D 小游戏（平台跳跃 或 弹幕射击 二选一）

### 第5课：2D 基础
- [ ] Sprite 与 SpriteRenderer
- [ ] 2D 物理（Rigidbody2D、BoxCollider2D）
- [ ] Sprite 动画（Animation、Animator）
- [ ] Sorting Layer 与渲染顺序

### 第6课：2D 角色控制
- [ ] 角色移动与跳跃
- [ ] 状态机（Idle/Run/Jump/Fall）
- [ ] 地面检测（Raycast / OverlapCircle）
- [ ] 镜头跟随（Cinemachine 2D）

### 第7课：2D 关卡设计
- [ ] Tilemap 系统
- [ ] 关卡元素（平台、陷阱、收集物）
- [ ] 敌人 AI（巡逻、追踪）
- [ ] 关卡切换（SceneManager）

### 第8课：UI 与游戏管理
- [ ] Canvas 与 UI 组件（Text、Button、Image）
- [ ] 血条、分数、计时器
- [ ] 游戏状态管理（开始/暂停/结束）
- [ ] 音效与背景音乐（AudioSource）

### 第9课：2D 游戏打磨与打包
- [ ] 粒子效果
- [ ] 屏幕震动、击中反馈
- [ ] PlayerPrefs 存档
- [ ] Build 打包为 Windows 可执行文件

---

## 阶段3：3D 游戏实战（4-6周）

> 目标：完成一个完整的 3D 小游戏（第一人称探索 或 第三人称动作 二选一）

### 第10课：3D 基础
- [ ] 3D 模型导入（FBX/OBJ）与材质（Material）
- [ ] 光照系统（Directional/Point/Spot Light、烘焙）
- [ ] Shader 基础与 URP/HDRP 简介
- [ ] 天空盒与环境设置

### 第11课：3D 角色控制
- [ ] CharacterController vs Rigidbody 方案对比
- [ ] 第一人称 / 第三人称摄像机
- [ ] 角色动画（Animator Controller、混合树）
- [ ] Cinemachine 3D 摄像机

### 第12课：3D 世界构建
- [ ] Terrain 系统（地形、纹理绘制、树木/草丛）
- [ ] ProBuilder 快速建模
- [ ] NavMesh 导航（敌人寻路）
- [ ] 触发区域与交互系统

### 第13课：3D 战斗与交互
- [ ] 射线检测（Raycast）交互
- [ ] 武器系统 / 攻击判定
- [ ] 敌人 AI（状态机 / Behavior Tree）
- [ ] 伤害系统与死亡处理

### 第14课：3D 游戏打磨
- [ ] 后处理效果（Post Processing）
- [ ] 粒子系统（3D）
- [ ] LOD 与性能优化基础
- [ ] Build 打包

---

## 附录

### A. 推荐工具
- 代码编辑器：Visual Studio / VS Code / Rider
- 2D 美术：Aseprite（像素）、Krita（绘画）
- 3D 建模：Blender（免费）
- 音效：Audacity、freesound.org
- AI 辅助：Claude Code（代码编写与问题解决）

### B. 推荐免费资源
- Unity Asset Store 免费资源
- Kenney.nl（免费游戏素材）
- Mixamo（免费3D角色与动画）
- OpenGameArt.org

### C. 学习进度记录
| 课程 | 状态 | 完成日期 | 备注 |
|------|------|----------|------|
| 第1课 | ✅ 完成 | 2026-03-28 | 核心概念、生命周期、Transform、Input、SerializeField |
| 第2课 | ✅ 完成 | 2026-03-31 | C#模式、Vector3、协程、Gizmos、Input、追逐小游戏 |
| 第3课 | ✅ 完成 | 2026-04-05 | Rigidbody、Collider、Trigger、碰撞检测、物理材质 |
| 第4课 | ✅ 完成 | 2026-04-07 | Prefab工作流、资源结构、ScriptableObject、免费资源 |
| 第5课 | ✅ 完成 | 2026-04-12 | Sprite、SpriteRenderer、2D物理、Sorting Layer、Animation |
