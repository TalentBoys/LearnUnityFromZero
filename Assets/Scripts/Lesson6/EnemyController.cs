using UnityEngine;

// 第6课：敌人行为脚本
// 敌人生成后向下移动（从屏幕上方飞向玩家方向）
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;       // 移动速度
    [SerializeField] private float lifetime = 10f;   // 最长存活时间

    void Start()
    {
        // 和子弹一样，飞出屏幕的敌人必须自动销毁
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 敌人向下移动（俯视角中，"下"就是朝向玩家的方向）
        // Vector2.down = new Vector2(0, -1)
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
}
