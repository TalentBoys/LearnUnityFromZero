using UnityEngine;

/// <summary>
/// 第4课：ScriptableObject 数据定义
/// 定义一个"生成物配置"，可以在编辑器中创建多个不同配置
/// </summary>
[CreateAssetMenu(fileName = "NewSpawnConfig", menuName = "Lesson04/Spawn Config")]
public class SpawnConfig : ScriptableObject
{
    [Header("基本信息")]
    public string configName = "默认配置";

    [Header("生成设置")]
    public GameObject prefab;          // 要生成的预制体
    public int spawnCount = 5;         // 生成数量
    public float spawnInterval = 0.5f; // 生成间隔（秒）
    public float spawnRadius = 5f;     // 生成范围半径

    [Header("物理属性")]
    public float launchForce = 5f;     // 发射力度
    public bool useGravity = true;     // 是否受重力
    public float lifetime = 5f;        // 存活时间（秒）

    [Header("外观")]
    public Color color = Color.white;  // 颜色
}
