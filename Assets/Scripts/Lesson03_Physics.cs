using UnityEngine;

/// <summary>
/// 第3课：物理系统
/// 演示 Rigidbody、Collider、碰撞检测、触发器、物理材质
/// </summary>
public class Lesson03_Physics : MonoBehaviour
{
    [Header("=== 力的控制 ===")]
    [SerializeField] private float pushForce = 5f;
    [SerializeField] private float jumpForce = 8f;

    [Header("=== 发射设置 ===")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float launchForce = 15f;

    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError($"[Lesson03] {gameObject.name} 缺少 Rigidbody 组件！");
            return;
        }

        // 打印当前物理属性
        Debug.Log($"[Lesson03] 质量={rb.mass}, 阻力={rb.drag}, 使用重力={rb.useGravity}");
    }

    void FixedUpdate()
    {
        // FixedUpdate 中处理物理操作，默认每 0.02 秒调用一次
        // 这样物理模拟不受帧率影响

        if (rb == null) return;

        // WASD 施力移动
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            Vector3 force = new Vector3(h, 0, v) * pushForce;
            rb.AddForce(force, ForceMode.Force);

            // ForceMode 有四种：
            // Force        — 持续力，受质量影响（默认）
            // Acceleration — 持续力，不受质量影响
            // Impulse      — 瞬间力，受质量影响（适合跳跃、爆炸）
            // VelocityChange — 瞬间力，不受质量影响
        }
    }

    void Update()
    {
        // 跳跃 — 用 Impulse 模式施加瞬间力
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            Debug.Log("[Lesson03] 跳跃！");
        }

        // 鼠标左键发射物体
        if (Input.GetMouseButtonDown(0) && projectilePrefab != null)
        {
            LaunchProjectile();
        }
    }

    void LaunchProjectile()
    {
        // 在物体前方生成投射物（用世界前方 Vector3.forward，不受球体旋转影响）
        Vector3 spawnPos = transform.position + Vector3.forward * 1.5f + Vector3.up * 0.5f;
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        // 给投射物施加力（固定朝世界 Z+ 方向）
        Rigidbody projRb = proj.GetComponent<Rigidbody>();
        if (projRb != null)
        {
            projRb.AddForce(Vector3.forward * launchForce, ForceMode.Impulse);
        }

        // 3秒后自动销毁
        Destroy(proj, 3f);
    }

    // ========== 碰撞检测回调 ==========

    // 碰撞开始（两个物体实际撞在一起）
    void OnCollisionEnter(Collision collision)
    {
        // collision.gameObject — 撞到的对象
        // collision.contacts   — 碰撞接触点数组
        // collision.relativeVelocity — 相对速度

        Debug.Log($"[碰撞] 与 {collision.gameObject.name} 发生碰撞！" +
                  $" 接触点数={collision.contactCount}" +
                  $" 相对速度={collision.relativeVelocity.magnitude:F1}");

        // 落地检测：碰撞点在物体下方 → 认为着地
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                Debug.Log("[碰撞] 着地了！");
                break;
            }
        }
    }

    // 碰撞持续中（每帧调用，性能敏感场景慎用）
    void OnCollisionStay(Collision collision)
    {
        // 这里可以做持续接触的逻辑，比如站在传送带上被推动
    }

    // 碰撞结束
    void OnCollisionExit(Collision collision)
    {
        Debug.Log($"[碰撞] 与 {collision.gameObject.name} 分离");
    }

    // ========== 触发器回调 ==========

    // 进入触发区域（Collider 的 Is Trigger = true）
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[触发器] 进入了 {other.gameObject.name} 的触发区域");

        // 示例：如果进入的是 "KillZone"，重置位置
        if (other.gameObject.CompareTag("Respawn"))
        {
            Debug.Log("[触发器] 掉入死亡区域，重置位置！");
            transform.position = new Vector3(0, 3, 0);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log($"[触发器] 离开了 {other.gameObject.name} 的触发区域");
    }
}
