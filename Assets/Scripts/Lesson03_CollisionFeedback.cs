using UnityEngine;

/// <summary>
/// 第3课辅助脚本：被碰撞时改变颜色
/// 挂在场景中的目标物体上，演示碰撞检测的实际用途
/// </summary>
public class Lesson03_CollisionFeedback : MonoBehaviour
{
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float resetTime = 0.5f;

    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rend == null) return;

        // 碰撞强度影响颜色：速度越快越红
        float impact = collision.relativeVelocity.magnitude;
        Debug.Log($"[反馈] {gameObject.name} 被 {collision.gameObject.name} 撞击，力度={impact:F1}");

        rend.material.color = hitColor;

        // 一段时间后恢复原色
        CancelInvoke(nameof(ResetColor));
        Invoke(nameof(ResetColor), resetTime);
    }

    void ResetColor()
    {
        if (rend != null)
        {
            rend.material.color = originalColor;
        }
    }
}
