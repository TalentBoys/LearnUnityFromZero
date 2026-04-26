using UnityEngine;

// 第6课：子弹行为脚本
// 挂在子弹预制体上，控制子弹的飞行和自动销毁
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 15f;       // 子弹飞行速度
    [SerializeField] private float lifetime = 3f;     // 子弹最长存活时间（秒）

    private Vector2 direction; // 子弹飞行方向

    // 外部调用：设置子弹的飞行方向
    // 为什么用 public 方法而不是 SerializeField？
    // 因为方向是运行时由射击脚本动态传入的，不是在 Inspector 里固定的
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized; // normalized 确保方向向量长度为1，否则斜着打子弹会更快
    }

    void Start()
    {
        // Destroy(gameObject, lifetime) 是 Unity 的定时销毁功能
        // 为什么需要这个？如果子弹飞出屏幕永远不被销毁，
        // 场景里的 GameObject 会越来越多，内存和性能都会出问题
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 每帧沿 direction 方向移动
        // Time.deltaTime 让移动速度与帧率无关（30fps和60fps的移动距离一样）
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
