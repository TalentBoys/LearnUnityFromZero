using UnityEngine;

// 第6课：伤害处理器 — 处理子弹和敌人的碰撞
// 挂在**敌人**身上，使用 Tag 判断碰到的是什么
//
// 为什么把碰撞逻辑放在敌人身上而不是子弹上？
// 两种都行，但放在敌人上更好管理：
// 敌人是"被打"的一方，在自己身上处理"被打后该怎么办"更符合直觉
// 而且以后不同敌人可能有不同的受击反应（比如Boss需要打多下）
public class DamageHandler : MonoBehaviour
{
    [SerializeField] private int health = 1; // 敌人血量（普通敌人1下就死）

    // OnTriggerEnter2D：当另一个 Collider2D 进入自己的 Trigger 范围时触发
    // 前提条件（重要！）：
    // 1. 自己或对方至少有一个 Rigidbody2D
    // 2. 自己的 Collider2D 勾选了 "Is Trigger"
    void OnTriggerEnter2D(Collider2D other)
    {
        // CompareTag 比直接比较字符串 tag == "Bullet" 更高效
        // Unity 内部做了优化，而且如果 Tag 不存在会报错提醒你
        if (other.CompareTag("Bullet"))
        {
            // 销毁子弹（击中就消失）
            Destroy(other.gameObject);

            // 敌人扣血
            health--;
            if (health <= 0)
            {
                Destroy(gameObject); // 血量归零，敌人销毁
            }
        }
    }
}
