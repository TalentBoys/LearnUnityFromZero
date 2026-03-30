using UnityEngine;
using System.Collections;

/// <summary>
/// 第2课：Unity C# 核心模式
///
/// 本课覆盖：
/// 1. [SerializeField] 与 [Header] — 组织 Inspector 面板
/// 2. 协程（Coroutine） — Unity 的"定时器"/"延迟执行"方案
/// 3. Vector3 数学 — 方向、距离、归一化
/// 4. 多个物体之间的交互 — 通过 public 引用或 Find 查找
/// 5. Debug 工具 — DrawRay、Gizmos
///
/// 使用方法：
/// 1. 创建一个 Sphere，挂上这个脚本
/// 2. 创建一个 Cube 作为"目标点"（不挂脚本）
/// 3. 在 Inspector 中把 Cube 拖到 Sphere 的 "Target" 字段
/// 4. Play 观察行为
/// </summary>
public class Lesson02_CSharpPatterns : MonoBehaviour
{
    // ========== Inspector 面板组织 ==========
    // [Header] 在 Inspector 中显示分类标题，让参数一目了然
    // 正式项目中参数多了不分组会很混乱

    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float arrivalDistance = 0.5f;

    [Header("巡逻设置")]
    [SerializeField] private float patrolRadius = 5f;
    [SerializeField] private float waitTime = 1f;

    [Header("引用")]
    [SerializeField] private Transform target; // 在 Inspector 中拖入目标物体

    [Header("调试")]
    [SerializeField] private bool showDebugInfo = true;
    [SerializeField] private Color gizmoColor = Color.yellow;

    // 私有状态（不需要在 Inspector 中显示）
    private Vector3 patrolPoint;
    private bool isWaiting;
    private bool isChasing;

    void Start()
    {
        // 如果没有手动指定 target，自动查找名为 "Cube" 的物体
        // GameObject.Find 性能较差，只适合在 Start 中用一次，不要在 Update 里调用
        if (target == null)
        {
            GameObject found = GameObject.Find("Cube");
            if (found != null)
                target = found.transform;
        }

        // 生成第一个巡逻点
        PickNewPatrolPoint();

        Debug.Log($"[Lesson02] 开始巡逻，半径: {patrolRadius}");
    }

    void Update()
    {
        if (isWaiting) return; // 协程等待中，不做移动

        // --- Vector3 核心用法 ---
        // 判断目标是否在范围内
        if (target != null)
        {
            // Vector3.Distance: 两点之间的距离
            float distToTarget = Vector3.Distance(transform.position, target.position);

            // 目标进入 8 米范围 → 追踪模式
            isChasing = distToTarget < 8f;
        }

        if (isChasing && target != null)
        {
            MoveToward(target.position);
        }
        else
        {
            // 巡逻模式：走向随机点，到达后等一会再换下一个
            float distToPatrol = Vector3.Distance(transform.position, patrolPoint);

            if (distToPatrol < arrivalDistance)
            {
                // 到达巡逻点 → 启动协程等待
                StartCoroutine(WaitAndPatrol());
            }
            else
            {
                MoveToward(patrolPoint);
            }
        }

        // --- Debug 可视化 ---
        if (showDebugInfo)
        {
            // Debug.DrawLine 在 Scene 视图中画线（Game 视图看不到）
            // 对调试AI行为非常有用
            Debug.DrawLine(transform.position, isChasing ? target.position : patrolPoint, Color.red);
        }
    }

    /// <summary>
    /// 朝目标点移动（提取成方法避免重复代码）
    /// </summary>
    void MoveToward(Vector3 destination)
    {
        // 方向 = 目标位置 - 当前位置
        Vector3 direction = destination - transform.position;
        direction.y = 0; // 锁定Y轴，不飞起来

        // .normalized 把向量变成长度为1的单位向量（只保留方向，去掉距离）
        // 这是游戏开发中最常用的操作之一
        Vector3 moveDir = direction.normalized;

        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

        // 让物体面朝移动方向
        if (moveDir != Vector3.zero)
        {
            // Quaternion.LookRotation: 根据方向生成旋转
            // Quaternion.Slerp: 球面插值，平滑旋转（比Lerp更适合旋转）
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    /// <summary>
    /// 协程：Unity 处理"等待"的方式
    ///
    /// 协程不是多线程！它是在主线程上"暂停-恢复"的机制：
    /// - yield return null → 等一帧
    /// - yield return new WaitForSeconds(n) → 等n秒
    /// - yield return new WaitUntil(() => condition) → 等到条件成立
    ///
    /// 比 Update 里写计时器更清晰
    /// </summary>
    IEnumerator WaitAndPatrol()
    {
        isWaiting = true;

        Debug.Log($"[Lesson02] 到达巡逻点，等待 {waitTime} 秒...");

        // 等待指定秒数（游戏不会卡住，其他物体继续运行）
        yield return new WaitForSeconds(waitTime);

        PickNewPatrolPoint();
        isWaiting = false;

        Debug.Log($"[Lesson02] 新巡逻点: {patrolPoint}");
    }

    /// <summary>
    /// 在初始位置周围随机选一个巡逻点
    /// </summary>
    void PickNewPatrolPoint()
    {
        // Random.insideUnitSphere: 单位球体内的随机点
        Vector3 randomOffset = Random.insideUnitSphere * patrolRadius;
        randomOffset.y = 0; // 保持在地面

        patrolPoint = transform.position + randomOffset;
    }

    /// <summary>
    /// OnDrawGizmosSelected: 在 Scene 视图中画辅助图形
    /// 只有选中物体时才画（OnDrawGizmos 则始终画）
    /// 非常适合可视化调试范围、路径等
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;

        // 画巡逻范围圆圈
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        // 画追踪范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 8f);

        // 如果游戏运行中，画出当前目标点
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(patrolPoint, 0.3f);
        }
    }
}
