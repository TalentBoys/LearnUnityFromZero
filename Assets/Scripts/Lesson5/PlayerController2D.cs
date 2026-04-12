using UnityEngine;

// 第5课：2D玩家控制器 — 学习 Rigidbody2D + SpriteRenderer
public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 获取输入（WASD 或方向键）
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 用 Vector2 而不是 Vector3（2D只有x,y）
        Vector2 moveDir = new Vector2(h, v).normalized;

        // 通过 Rigidbody2D 移动（不要直接改 transform）
        rb.velocity = moveDir * moveSpeed;

        // 根据移动方向翻转图片（代替做两套左右朝向的图）
        if (h > 0) spriteRenderer.flipX = false;
        if (h < 0) spriteRenderer.flipX = true;

        // 按空格变色（演示 SpriteRenderer.color）
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spriteRenderer.color = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );
        }
    }
}
