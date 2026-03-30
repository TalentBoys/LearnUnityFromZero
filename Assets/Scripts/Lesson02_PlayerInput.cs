using UnityEngine;

/// <summary>
/// 第2课补充：玩家输入控制
///
/// 知识点：
/// 1. Input.GetAxis — 获取轴向输入（WASD / 方向键）
/// 2. Input.GetKeyDown — 检测单次按键
/// 3. Space.Self vs Space.World — 本地坐标 vs 世界坐标
///
/// 使用方法：
/// 把这个脚本挂到 Cube 上，Play 后用 WASD 移动
/// </summary>
public class Lesson02_PlayerInput : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 8f;

    void Update()
    {
        // --- Input.GetAxis ---
        // "Horizontal" → A/D 或 ←/→，返回 -1 到 1
        // "Vertical"   → W/S 或 ↑/↓，返回 -1 到 1
        // 自带平滑过渡，不是硬切 0/1
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 组合成移动方向（X = 左右, Z = 前后, Y 不动）
        Vector3 moveDir = new Vector3(h, 0, v);

        // 防止斜向移动速度变快（对角线长度是 1.41）
        if (moveDir.magnitude > 1f)
            moveDir.Normalize();

        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

        // --- Input.GetKeyDown ---
        // GetKeyDown: 按下的那一帧返回 true（只触发一次）
        // GetKey: 按住期间每帧都返回 true
        // GetKeyUp: 松开的那一帧返回 true
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("[Input] 空格键按下！（可以用来触发跳跃、攻击等）");
        }
    }
}
