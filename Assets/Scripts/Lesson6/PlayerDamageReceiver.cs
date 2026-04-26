using UnityEngine;

// 第6课：敌人碰到玩家时造成伤害
// 挂在**玩家**身上
//
// 为什么不把这段逻辑和 DamageHandler 合并？
// 因为职责不同：DamageHandler 处理"敌人被子弹打"的逻辑，
// 这个脚本处理"玩家被敌人碰到"的逻辑。
// 每个脚本只负责一件事（单一职责原则），便于以后扩展
public class PlayerDamageReceiver : MonoBehaviour
{
    [SerializeField] private float invincibleTime = 1f; // 受击后无敌时间（秒）

    private PlayerHealth playerHealth;
    private float invincibleTimer;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        // 无敌倒计时
        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 碰到敌人时扣血
        if (other.CompareTag("Enemy") && invincibleTimer <= 0f)
        {
            playerHealth.TakeDamage(1);

            // 进入无敌状态，避免一帧内被多个敌人连续扣血
            // 大多数游戏受击后都有一小段无敌时间
            invincibleTimer = invincibleTime;

            // 销毁碰到玩家的敌人（撞上就同归于尽）
            Destroy(other.gameObject);
        }
    }
}
