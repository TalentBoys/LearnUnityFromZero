using UnityEngine;

// 第6课：玩家血量管理
// 挂在玩家身上，管理生命值
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;  // 最大血量

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"玩家血量：{currentHealth}/{maxHealth}");
    }

    // 被敌人碰到时调用
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"受到 {damage} 点伤害！剩余血量：{currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("玩家死亡！游戏结束");
        // 暂时用 SetActive(false) 让玩家消失
        // 后面第8课做UI时会改成正式的 GameOver 画面
        gameObject.SetActive(false);
    }
}
