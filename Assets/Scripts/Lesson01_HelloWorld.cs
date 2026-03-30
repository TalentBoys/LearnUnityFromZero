using UnityEngine;

/// <summary>
/// 第1课：你的第一个 Unity 脚本
///
/// 这个脚本演示了 Unity 最核心的概念：
/// 1. MonoBehaviour — 所有 Unity 脚本的基类
/// 2. Start() — 游戏开始时执行一次
/// 3. Update() — 每帧执行一次（游戏的"心跳"）
/// 4. Transform — 每个 GameObject 都有，控制位置/旋转/缩放
///
/// 使用方法：
/// 1. 在 Unity 中创建一个 3D 物体（Cube / Sphere）
/// 2. 把这个脚本拖到物体上
/// 3. 点击 Play，观察物体的行为
/// </summary>
public class Lesson01_HelloWorld : MonoBehaviour
{
    // [SerializeField] 让私有变量在 Unity Inspector 面板中可见可调
    // 你可以在不停止游戏的情况下，直接在面板里拖动数值看效果
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotateSpeed = 100f;

    // Start: 场景加载后、第一帧之前调用，只调用一次
    void Start()
    {
        // Debug.Log 是 Unity 的 console.log / print
        // 输出会显示在 Unity 编辑器底部的 Console 窗口
        Debug.Log($"[HelloWorld] {gameObject.name} 已就绪！位置: {transform.position}");
    }

    // Update: 每帧调用一次
    // 帧率60fps → 每秒调用60次，帧率不固定所以要乘 Time.deltaTime
    void Update()
    {
        // --- 移动 ---
        // Time.deltaTime = 上一帧到这一帧经过的秒数
        // 乘以它可以让移动速度与帧率无关（这是游戏开发的基本常识）
        //
        // Input.GetAxis("Horizontal") → A/D 或 左/右方向键，返回 -1 到 1
        // Input.GetAxis("Vertical")   → W/S 或 上/下方向键，返回 -1 到 1
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 构造移动方向向量：x=左右，y=0（不飞），z=前后
        Vector3 movement = new Vector3(h, 0f, v) * moveSpeed * Time.deltaTime;

        // transform.Translate 让物体沿着指定方向移动
        // Space.Self 表示相对于物体自身方向（不是世界方向）
        transform.Translate(movement, Space.Self);

        // --- 旋转 ---
        // 按 Q/E 旋转（用 GetKey 检测按键是否按住）
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }

        // --- 缩放 ---
        // 按空格键时物体会"呼吸"（缩放变化），松开恢复
        if (Input.GetKey(KeyCode.Space))
        {
            // Mathf.Sin 产生 -1 到 1 的波动，Time.time 是游戏运行总时间
            float scale = 1f + Mathf.Sin(Time.time * 5f) * 0.2f;
            transform.localScale = Vector3.one * scale;
        }
        else
        {
            // 松开空格，平滑恢复到原始大小
            // Vector3.Lerp 是线性插值，常用于平滑过渡
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 5f);
        }
    }

    // OnGUI: Unity 的即时模式 GUI（简单调试用，正式UI不用这个）
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 400, 100),
            "WASD: 移动 | Q/E: 旋转 | Space: 缩放呼吸\n" +
            $"位置: {transform.position:F1}\n" +
            $"旋转: {transform.eulerAngles:F1}");
    }
}
