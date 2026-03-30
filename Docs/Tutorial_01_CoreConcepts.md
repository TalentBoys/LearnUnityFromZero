# 第1课：Unity 核心概念

> 本课目标：理解 Unity 的核心架构（Scene → GameObject → Component），编写第一个 C# 脚本，让物体动起来。

---

## 1.1 Unity 的世界观：Scene → GameObject → Component

Unity 的整个架构可以用三层来理解：

```
Scene（场景）
 └── GameObject（游戏对象）
      ├── Transform（变换组件 — 每个物体必有）
      ├── MeshRenderer（渲染组件 — 让物体可见）
      ├── Collider（碰撞组件 — 让物体有物理体积）
      └── YourScript（你的脚本 — 自定义行为）
```

### Scene（场景）

场景就是一个"关卡"或"界面"。一个游戏通常有多个场景：

- 主菜单场景
- 游戏关卡场景
- 设置界面场景

你当前项目里有一个默认场景：`Assets/Scenes/SampleScene.unity`

### GameObject（游戏对象）

场景中的**一切**都是 GameObject——角色、地面、灯光、摄像机，甚至看不见的"管理器"。

GameObject 本身只是一个**空容器**，它什么都不会做。它的能力完全取决于挂载了哪些 Component。

### Component（组件）

组件是赋予 GameObject 功能的模块。Unity 的设计哲学是**组合优于继承**：

| 常见组件 | 作用 |
|----------|------|
| Transform | 位置、旋转、缩放（每个 GameObject 自动拥有） |
| MeshFilter + MeshRenderer | 3D 模型的形状和渲染 |
| Camera | 摄像机，决定玩家看到什么 |
| Light | 灯光 |
| Rigidbody | 物理模拟（重力、碰撞） |
| Collider | 碰撞体积 |
| AudioSource | 播放声音 |
| **你写的 C# 脚本** | **自定义行为** |

> **关键理解**：你写的每个 C# 脚本也是一个组件。把脚本"挂到"物体上，就是给物体添加了一个自定义组件。

---

## 1.2 Transform：每个物体的"灵魂"

Transform 是最特殊的组件——每个 GameObject **必定**有一个 Transform，而且不能删除。

它控制三个属性：

| 属性 | 含义 | 代码访问 |
|------|------|----------|
| **Position** | 世界坐标位置 (x, y, z) | `transform.position` |
| **Rotation** | 旋转角度 | `transform.eulerAngles` 或 `transform.rotation` |
| **Scale** | 缩放比例 | `transform.localScale` |

### 坐标系

Unity 使用**左手坐标系**：

```
    Y (上)
    |
    |
    +---- X (右)
   /
  Z (前)
```

- **X 轴**：左右（正 = 右）
- **Y 轴**：上下（正 = 上）
- **Z 轴**：前后（正 = 前/远离屏幕）

---

## 1.3 生命周期函数

Unity 脚本中的函数不是你手动调用的，而是 Unity **引擎自动**在特定时刻调用的。这些函数叫做**生命周期函数（Lifecycle Methods）**。

### 最重要的四个

```
场景加载
  │
  ├─ Awake()        ← 最早，对象创建时调用（即使脚本被禁用）
  │
  ├─ Start()        ← 第一帧之前调用（只调用一次）
  │
  │  ┌─────── 游戏循环（每帧重复） ───────┐
  │  │                                    │
  ├──├─ FixedUpdate()  ← 物理更新（固定间隔，默认0.02秒）
  │  │                                    │
  ├──├─ Update()       ← 每帧调用一次（帧率不固定！）
  │  │                                    │
  ├──├─ LateUpdate()   ← Update 之后调用（常用于摄像机跟随）
  │  │                                    │
  │  └────────────────────────────────────┘
  │
  └─ OnDestroy()    ← 对象被销毁时调用
```

### 使用原则

| 函数 | 何时使用 |
|------|----------|
| `Awake()` | 初始化自身数据（不依赖其他脚本） |
| `Start()` | 初始化需要引用其他脚本的数据 |
| `Update()` | 处理输入、非物理相关的每帧逻辑 |
| `FixedUpdate()` | 物理相关操作（移动 Rigidbody 等） |
| `LateUpdate()` | 摄像机跟随、需要在所有 Update 执行完后处理的逻辑 |

---

## 1.4 Time.deltaTime：帧率无关的关键

`Time.deltaTime` 是上一帧到当前帧经过的秒数。

**为什么重要？** 假设你写 `transform.Translate(Vector3.forward)`：
- 60fps 的电脑 → 每秒移动 60 个单位
- 30fps 的电脑 → 每秒移动 30 个单位

这显然不对。乘以 `Time.deltaTime` 后：
- 60fps → 每帧移动 1/60 单位 → 每秒 1 单位
- 30fps → 每帧移动 1/30 单位 → 每秒 1 单位

**规则：在 Update 中的移动/旋转/缩放必须乘以 `Time.deltaTime`**

```csharp
// ❌ 错误：移动速度取决于帧率
transform.Translate(Vector3.forward * speed);

// ✅ 正确：无论帧率多少，每秒移动 speed 个单位
transform.Translate(Vector3.forward * speed * Time.deltaTime);
```

---

## 1.5 \[SerializeField\]：连接代码与编辑器

`[SerializeField]` 是 Unity 最常用的特性之一，它让**私有变量**出现在 Inspector 面板中。

```csharp
// 在 Inspector 中可见，可以拖动调整
[SerializeField] private float moveSpeed = 3f;

// 在 Inspector 中不可见（普通私有变量）
private float internalTimer;

// 在 Inspector 中可见（但不推荐，因为任何代码都能随意修改）
public float healthPoints = 100f;
```

**最佳实践**：用 `[SerializeField] private` 而不是 `public`，这样既能在编辑器里调参，又保持了代码的封装性。

### 为什么这很重要？

开发游戏时你需要不停调整参数（移动速度、跳跃高度、攻击伤害等）。有了 `[SerializeField]`，你可以：

1. **不停止游戏就能调参数**（在 Play 模式下拖动 Inspector 里的值）
2. **不改代码就能尝试不同数值**
3. **策划/美术也能直接改参数**（不需要懂代码）

---

## 1.6 Input：获取玩家输入

Unity 内置的输入系统（旧版）用起来很简单：

### Input.GetAxis — 轴向输入（连续值）

```csharp
// 返回 -1 到 1 的浮点数，自带平滑过渡
float horizontal = Input.GetAxis("Horizontal"); // A/D 或 ←/→
float vertical = Input.GetAxis("Vertical");     // W/S 或 ↑/↓
```

### Input.GetKey — 按键检测

```csharp
Input.GetKey(KeyCode.Space)       // 按住期间每帧返回 true
Input.GetKeyDown(KeyCode.Space)   // 按下的那一帧返回 true（只触发一次）
Input.GetKeyUp(KeyCode.Space)     // 松开的那一帧返回 true
```

### 三者的区别

| 方法 | 触发时机 | 典型用途 |
|------|----------|----------|
| `GetKey` | 按住期间每帧 | 持续移动、开火 |
| `GetKeyDown` | 按下那一帧 | 跳跃、交互、切换 |
| `GetKeyUp` | 松开那一帧 | 松开弓弦射箭、蓄力释放 |

---

## 1.7 实践：编写第一个脚本

现在把上面的知识点整合起来，写一个让物体可以移动、旋转、缩放的脚本。

### 步骤1：创建脚本文件

1. 在 Project 窗口，右键 `Assets/Scripts` 文件夹（没有就先创建）
2. **Create → C# Script**
3. 命名为 `Lesson01_HelloWorld`

> **注意**：脚本文件名必须与类名完全一致，否则 Unity 会报错。

### 步骤2：编写代码

双击脚本文件，在代码编辑器中打开，替换为以下内容：

```csharp
using UnityEngine;

public class Lesson01_HelloWorld : MonoBehaviour
{
    // [SerializeField] 让私有变量在 Inspector 面板中可见
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotateSpeed = 100f;

    // Start: 游戏开始时调用一次
    void Start()
    {
        Debug.Log($"[HelloWorld] {gameObject.name} 已就绪！位置: {transform.position}");
    }

    // Update: 每帧调用一次
    void Update()
    {
        // --- 移动 ---
        float h = Input.GetAxis("Horizontal"); // A/D 或 ←/→
        float v = Input.GetAxis("Vertical");   // W/S 或 ↑/↓

        // 构造移动方向：x=左右, y=0(不飞), z=前后
        Vector3 movement = new Vector3(h, 0f, v) * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.Self);

        // --- 旋转 ---
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

        // --- 缩放（按住空格"呼吸"效果） ---
        if (Input.GetKey(KeyCode.Space))
        {
            float scale = 1f + Mathf.Sin(Time.time * 5f) * 0.2f;
            transform.localScale = Vector3.one * scale;
        }
        else
        {
            // 松开后平滑恢复原始大小
            transform.localScale = Vector3.Lerp(
                transform.localScale, Vector3.one, Time.deltaTime * 5f);
        }
    }

    // OnGUI: 屏幕上显示调试信息（正式UI不用这个）
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 400, 100),
            "WASD: 移动 | Q/E: 旋转 | Space: 缩放呼吸\n" +
            $"位置: {transform.position:F1}\n" +
            $"旋转: {transform.eulerAngles:F1}");
    }
}
```

### 步骤3：挂载脚本到物体上

1. 在 Hierarchy 中，右键 → **3D Object → Cube** 创建一个立方体
2. 选中 Cube，在 Inspector 底部点击 **Add Component**
3. 搜索 `Lesson01_HelloWorld`，点击添加

> **或者更快的方式**：直接把 Project 窗口中的脚本**拖拽**到 Cube 上。

### 步骤4：运行测试

1. 点击 **▶ Play**
2. 用 **WASD** 移动，**Q/E** 旋转，**空格** 看缩放效果
3. 观察左上角的位置和旋转信息实时变化
4. 打开 Console 窗口，确认看到了 `[HelloWorld] Cube 已就绪！` 的日志

### 步骤5：在 Inspector 中调参

1. **保持 Play 状态**，选中 Cube
2. 在 Inspector 中找到 `Lesson01_HelloWorld` 组件
3. 拖动 **Move Speed** 滑块，观察移动速度的变化
4. 尝试改变 **Rotate Speed**

> **再次提醒**：Play 模式下的修改在停止后会**重置**！如果找到了满意的参数，记得停止后再改一次。

---

## 1.8 代码解析

### MonoBehaviour

```csharp
public class Lesson01_HelloWorld : MonoBehaviour
```

所有 Unity 脚本都必须继承 `MonoBehaviour`。继承它之后你才能：
- 使用生命周期函数（Start、Update 等）
- 把脚本挂到 GameObject 上
- 使用 `transform`、`gameObject` 等属性

### transform.Translate vs transform.position

| 方式 | 代码 | 含义 |
|------|------|------|
| Translate | `transform.Translate(direction)` | 相对移动（在当前位置上移动多少） |
| position | `transform.position = newPos` | 绝对定位（直接设置到某个位置） |
| position | `transform.position += offset` | 也是相对移动，但始终基于世界坐标 |

`Space.Self` vs `Space.World`：
- `Space.Self`：相对于物体自身方向（物体旋转后，"前方"会变）
- `Space.World`：相对于世界坐标（"前方"永远是 Z 轴正方向）

### Vector3.Lerp — 线性插值

```csharp
Vector3.Lerp(当前值, 目标值, 过渡速度)
```

Lerp 是 "Linear Interpolation"（线性插值），在游戏开发中极其常用。它让值从 A **平滑过渡**到 B，而不是瞬间跳变。

### Debug.Log 与字符串插值

```csharp
Debug.Log($"位置: {transform.position}");
```

`$"..."` 是 C# 的字符串插值语法，`{表达式}` 会被替换为表达式的值。`:F1` 是格式说明符，表示保留 1 位小数。

---

## 1.9 本课总结

你在这一课中学到了：

| 概念 | 一句话总结 |
|------|-----------|
| Scene → GameObject → Component | Unity 的三层架构，组合优于继承 |
| Transform | 每个物体必有，控制位置/旋转/缩放 |
| 生命周期函数 | Start 执行一次，Update 每帧执行 |
| Time.deltaTime | 保证移动速度与帧率无关 |
| [SerializeField] | 让 Inspector 能调私有参数 |
| Input.GetAxis / GetKey | 获取玩家键盘输入 |

---

## 下一课

接下来进入 [第2课：C# 游戏开发基础](Tutorial_02_CSharpGameDev.md)，我们将学习协程、Vector3 数学、Gizmos 调试，并做一个追逐小游戏。
