using UnityEngine;

// 第6课：玩家射击控制器
// 挂在玩家身上，按键发射子弹
public class PlayerShooter : MonoBehaviour
{
    [Header("子弹设置")]
    [SerializeField] private GameObject bulletPrefab; // 子弹预制体，从 Inspector 拖入
    [SerializeField] private float fireRate = 0.15f;  // 射击间隔（秒），越小射速越快

    [Header("发射点")]
    [SerializeField] private Transform firePoint; // 子弹从哪个位置发射（空物体作为标记点）

    private float fireTimer; // 射击冷却计时器

    void Update()
    {
        // 计时器倒计时
        // 为什么需要冷却？如果不限制，按住鼠标每帧都发射，一秒60发子弹太夸张了
        fireTimer -= Time.deltaTime;

        // 按住鼠标左键（或 Ctrl）连续射击
        // GetButton 是"按住持续触发"，GetButtonDown 是"按下那一刻才触发一次"
        // 弹幕射击游戏一般用 GetButton，让玩家按住就能连射
        if (Input.GetButton("Fire1") && fireTimer <= 0f)
        {
            Fire();
            fireTimer = fireRate; // 重置冷却
        }
    }

    void Fire()
    {
        // 如果没配置子弹预制体或发射点，打印警告并跳过
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("PlayerShooter: 请在 Inspector 中设置 bulletPrefab 和 firePoint！");
            return;
        }

        // Instantiate = 实例化（克隆）一个预制体到场景中
        // 参数：要克隆的对象、生成位置、生成旋转
        // Quaternion.identity = 不旋转（朝默认方向）
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // 设置子弹方向为"向上"（俯视角弹幕游戏里，上方就是前方）
        // 如果以后改成全方向射击，这里改成鼠标方向即可
        bullet.GetComponent<Bullet>().SetDirection(Vector2.up);
    }
}
