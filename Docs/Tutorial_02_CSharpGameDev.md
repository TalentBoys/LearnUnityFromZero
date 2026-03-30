# 第2课：C# 游戏开发基础

> 本课目标：掌握 Unity 中最常用的 C# 模式，学会用 Vector3 做数学运算，理解协程的工作原理，并完成一个"追逐小游戏"。

---

## 2.1 Inspector 面板组织：\[Header\] 与 \[Tooltip\]

当一个脚本的参数越来越多时，Inspector 面板会变得混乱。Unity 提供了一些特性（Attribute）来组织面板。

### \[Header\] — 分类标题

```csharp
[Header("移动设置")]
[SerializeField] private float moveSpeed = 5f;
[SerializeField] private float arrivalDistance = 0.5f;

[Header("巡逻设置")]
[SerializeField] private float patrolRadius = 5f;
[SerializeField] private float waitTime = 1f;
```

效果：Inspector 中会显示粗体分类标题，参数一目了然。

### \[Tooltip\] — 鼠标悬停提示

```csharp
[Tooltip("物体每秒移动的单位数")]
[SerializeField] private float moveSpeed = 5f;
```

鼠标悬停在参数名上时，会显示提示文字。

### \[Range\] — 限制数值范围

```csharp
[Range(0f, 20f)]
[SerializeField] private float moveSpeed = 5f;
```

Inspector 中会显示滑块，防止设置不合理的值。

---

## 2.2 引用其他物体

游戏中物体之间经常需要交互（敌人追踪玩家、子弹飞向目标等）。Unity 中引用其他物体有两种主要方式：

### 方式1：Inspector 拖拽（推荐）

```csharp
[SerializeField] private Transform target;
```

在 Inspector 中把目标物体**拖到这个字段上**，就建立了引用关系。

**优势**：直观、安全、性能好。

### 方式2：代码查找

```csharp
void Start()
{
    // 按名称查找（性能较差，只在 Start 中用）
    GameObject found = GameObject.Find("Player");

    if (found != null)
        target = found.transform;
}
```

**注意**：`GameObject.Find` 会遍历场景中所有物体，性能开销大。**绝对不要在 Update 中使用它**。只在 Start/Awake 中用一次。

---

## 2.3 Vector3 核心数学

`Vector3` 是 Unity 中用于表示 3D 坐标和方向的结构体。掌握 Vector3 的几个核心操作是游戏开发的基础。

### 常用的预定义向量

```csharp
Vector3.zero     // (0, 0, 0) — 原点
Vector3.one      // (1, 1, 1) — 常用于缩放
Vector3.up       // (0, 1, 0) — 上方
Vector3.down     // (0, -1, 0)
Vector3.forward  // (0, 0, 1) — 前方
Vector3.right    // (1, 0, 0) — 右方
```

### 距离计算

```csharp
float distance = Vector3.Distance(posA, posB);
```

计算两点之间的直线距离。最常见的用途：判断两个物体是否"足够近"。

### 方向计算

```csharp
// 从 A 到 B 的方向向量
Vector3 direction = targetPos - myPos;
```

这是向量减法：**目标位置 - 当前位置 = 指向目标的方向**。

### 归一化（Normalize）

```csharp
Vector3 moveDir = direction.normalized;
```

`.normalized` 把向量变成**长度为 1**的单位向量，只保留方向信息。

**为什么要归一化？** 如果目标离你 100 米远，`direction` 的长度就是 100，直接用来移动会飞过去。归一化后长度恒为 1，乘以速度就能匀速前进。

### 完整示例：向目标移动

```csharp
void MoveToward(Vector3 destination)
{
    Vector3 direction = destination - transform.position; // 方向
    direction.y = 0;                                       // 锁定Y轴
    Vector3 moveDir = direction.normalized;                // 归一化

    transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
}
```

---

## 2.4 Quaternion 基础：让物体面朝目标

旋转在 3D 中比较复杂，Unity 用 **Quaternion（四元数）** 来表示旋转。你不需要理解四元数的数学原理，只需要记住几个常用 API：

### 面朝某个方向

```csharp
// 根据方向向量生成旋转
Quaternion targetRotation = Quaternion.LookRotation(moveDir);

// 平滑旋转过渡（Slerp = 球面插值，比 Lerp 更适合旋转）
transform.rotation = Quaternion.Slerp(
    transform.rotation,     // 当前旋转
    targetRotation,         // 目标旋转
    Time.deltaTime * 10f    // 过渡速度
);
```

### Slerp vs Lerp

| 插值方式 | 适用场景 |
|----------|----------|
| `Vector3.Lerp` | 位置的平滑过渡 |
| `Quaternion.Slerp` | 旋转的平滑过渡（沿球面插值，更自然） |

---

## 2.5 协程（Coroutine）

协程是 Unity 处理**"等待"和"延时"**的核心机制。

### 问题场景

如果你想让敌人到达巡逻点后**等待 2 秒**再继续巡逻，该怎么做？

**方案A：在 Update 里写计时器**

```csharp
private float timer;
private bool isWaiting;

void Update()
{
    if (isWaiting)
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            isWaiting = false;
            // 继续巡逻...
        }
        return;
    }
    // 正常逻辑...
}
```

这可行，但当你有很多"等一下再做"的逻辑时，代码会变成一团意大利面。

**方案B：使用协程（推荐）**

```csharp
IEnumerator WaitAndPatrol()
{
    isWaiting = true;

    yield return new WaitForSeconds(2f); // 等待2秒，游戏不卡

    PickNewPatrolPoint();
    isWaiting = false;
}
```

### 协程的关键概念

1. **不是多线程**：协程运行在主线程上，只是在 `yield` 处"暂停"，下一帧/N秒后"恢复"
2. **游戏不会卡住**：等待期间其他物体照常运行
3. **用 `StartCoroutine` 启动**：

```csharp
StartCoroutine(WaitAndPatrol());
```

### 常用的 yield 语句

| yield 语句 | 作用 |
|------------|------|
| `yield return null` | 等一帧 |
| `yield return new WaitForSeconds(n)` | 等 n 秒 |
| `yield return new WaitUntil(() => condition)` | 等到条件为真 |
| `yield return new WaitWhile(() => condition)` | 条件为真期间一直等 |

### 实际例子：闪烁效果

```csharp
IEnumerator Blink(int times)
{
    var renderer = GetComponent<Renderer>();
    for (int i = 0; i < times; i++)
    {
        renderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        renderer.enabled = true;
        yield return new WaitForSeconds(0.1f);
    }
}
```

---

## 2.6 Debug 可视化工具

调试 AI 行为、移动路径、攻击范围时，仅靠 Console 日志很难判断问题。Unity 提供了强大的可视化调试工具。

### Debug.DrawLine / Debug.DrawRay

在 **Scene 视图**中画线（Game 视图中看不到）：

```csharp
void Update()
{
    // 从自身画线到目标点（红色）
    Debug.DrawLine(transform.position, targetPosition, Color.red);

    // 从自身向前方画射线
    Debug.DrawRay(transform.position, transform.forward * 5f, Color.green);
}
```

### Gizmos — 在 Scene 中画形状

Gizmos 可以画球体、立方体、线框等，非常适合可视化范围：

```csharp
// OnDrawGizmosSelected: 选中物体时才显示
void OnDrawGizmosSelected()
{
    // 画巡逻范围（黄色线框球体）
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, patrolRadius);

    // 画追踪范围（红色线框球体）
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, 8f);

    // 画当前目标点（绿色实心球）
    if (Application.isPlaying)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(patrolPoint, 0.3f);
    }
}
```

### OnDrawGizmos vs OnDrawGizmosSelected

| 方法 | 显示时机 |
|------|----------|
| `OnDrawGizmos` | 始终显示（即使未选中物体） |
| `OnDrawGizmosSelected` | 仅在选中物体时显示 |

---

## 2.7 实践：追逐小游戏

现在把本课所有知识点整合，做一个"追逐小游戏"——一个 Sphere 在场景中巡逻，当 Cube（玩家）靠近时会追踪过来。

### 场景搭建

1. **创建玩家**：Hierarchy → 3D Object → **Cube**
   - 挂上 `Lesson02_PlayerInput` 脚本
2. **创建巡逻 AI**：Hierarchy → 3D Object → **Sphere**
   - 挂上 `Lesson02_CSharpPatterns` 脚本
3. **创建地面**（可选）：Hierarchy → 3D Object → **Plane**
   - Scale 设为 (5, 1, 5)

### 脚本1：Lesson02_PlayerInput.cs（玩家控制）

```csharp
using UnityEngine;

public class Lesson02_PlayerInput : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 8f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(h, 0, v);

        // 防止斜向移动速度变快（对角线长度是 1.41）
        if (moveDir.magnitude > 1f)
            moveDir.Normalize();

        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("[Input] 空格键按下！");
        }
    }
}
```

**知识点回顾**：
- `moveDir.magnitude`：向量的长度。斜向移动时 (1,0,1) 的长度是 √2 ≈ 1.41
- `Normalize()`：把向量长度变为 1，避免斜向移动更快
- `Space.World`：使用世界坐标系移动

### 脚本2：Lesson02_CSharpPatterns.cs（巡逻AI）

```csharp
using UnityEngine;
using System.Collections;

public class Lesson02_CSharpPatterns : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float arrivalDistance = 0.5f;

    [Header("巡逻设置")]
    [SerializeField] private float patrolRadius = 5f;
    [SerializeField] private float waitTime = 1f;

    [Header("引用")]
    [SerializeField] private Transform target;

    [Header("调试")]
    [SerializeField] private bool showDebugInfo = true;
    [SerializeField] private Color gizmoColor = Color.yellow;

    private Vector3 patrolPoint;
    private bool isWaiting;
    private bool isChasing;

    void Start()
    {
        // 如果没手动指定 target，自动查找 Cube
        if (target == null)
        {
            GameObject found = GameObject.Find("Cube");
            if (found != null)
                target = found.transform;
        }

        PickNewPatrolPoint();
        Debug.Log($"[Lesson02] 开始巡逻，半径: {patrolRadius}");
    }

    void Update()
    {
        if (isWaiting) return;

        // 检测目标是否在追踪范围内
        if (target != null)
        {
            float distToTarget = Vector3.Distance(transform.position, target.position);
            isChasing = distToTarget < 8f;
        }

        if (isChasing && target != null)
        {
            MoveToward(target.position);
        }
        else
        {
            float distToPatrol = Vector3.Distance(transform.position, patrolPoint);
            if (distToPatrol < arrivalDistance)
            {
                StartCoroutine(WaitAndPatrol());
            }
            else
            {
                MoveToward(patrolPoint);
            }
        }

        // 调试画线
        if (showDebugInfo)
        {
            Debug.DrawLine(
                transform.position,
                isChasing ? target.position : patrolPoint,
                Color.red);
        }
    }

    void MoveToward(Vector3 destination)
    {
        Vector3 direction = destination - transform.position;
        direction.y = 0;
        Vector3 moveDir = direction.normalized;

        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

        // 面朝移动方向
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    IEnumerator WaitAndPatrol()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        PickNewPatrolPoint();
        isWaiting = false;
    }

    void PickNewPatrolPoint()
    {
        Vector3 randomOffset = Random.insideUnitSphere * patrolRadius;
        randomOffset.y = 0;
        patrolPoint = transform.position + randomOffset;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 8f);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(patrolPoint, 0.3f);
        }
    }
}
```

### 运行效果

1. 点击 **Play**
2. 用 **WASD** 控制 Cube 移动
3. 观察 Sphere 的行为：
   - **远距离**：Sphere 在巡逻点之间来回走（黄色圈范围内）
   - **靠近（8米内）**：Sphere 转为追踪你的 Cube
   - **拉开距离**：Sphere 恢复巡逻
4. 在 Scene 视图中选中 Sphere，可以看到 Gizmos 画的范围圈
5. 红线显示 Sphere 当前正在追踪的目标（玩家或巡逻点）

### 调参建议

在 Inspector 中尝试调整以下参数，观察效果变化：

| 参数 | 默认值 | 尝试 | 效果 |
|------|--------|------|------|
| Move Speed | 5 | 8 | AI 移动更快，更难逃脱 |
| Patrol Radius | 5 | 10 | 巡逻范围更大 |
| Wait Time | 1 | 0.2 | 到达巡逻点后几乎不停留 |
| Arrival Distance | 0.5 | 2 | 更早判定"到达" |

---

## 2.8 本课总结

| 概念 | 一句话总结 |
|------|-----------|
| [Header] / [Tooltip] / [Range] | 让 Inspector 面板更有组织性 |
| Inspector 拖拽引用 | 最推荐的物体间引用方式 |
| Vector3.Distance | 计算两点之间的距离 |
| direction.normalized | 只保留方向，去掉距离信息 |
| Quaternion.LookRotation + Slerp | 让物体平滑转向目标方向 |
| 协程 + yield return | Unity 处理等待/延时的方式 |
| Debug.DrawLine / Gizmos | 在 Scene 视图中可视化调试 |

### 第1课 vs 第2课 对比

| | 第1课 | 第2课 |
|--|-------|-------|
| 控制方式 | 玩家直接控制物体 | AI 自主决策（巡逻+追踪） |
| 移动 | 简单方向移动 | 朝目标点移动 + 面朝转向 |
| 数学 | 基础 Vector3 | 距离、方向、归一化 |
| 时间控制 | Time.deltaTime | 协程 WaitForSeconds |
| 调试 | OnGUI 文字 | Gizmos 可视化 |

---

## 下一课

接下来是 [第3课：物理系统](Tutorial_03_Physics.md)，我们将学习 Rigidbody、Collider、碰撞检测，让物体之间产生真实的物理交互。
