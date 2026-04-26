using UnityEngine;

// 第6课：弹幕射击专用移动控制
// 和第5课的区别：去掉了翻转和变色，只保留纯移动
// 为什么不复用第5课的脚本？每课的场景独立，脚本也独立，
// 这样回头复习某一课时不会发现内容被后面的课改掉了
public class ShooterMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = new Vector2(h, v).normalized;
        rb.velocity = moveDir * moveSpeed;
    }
}
