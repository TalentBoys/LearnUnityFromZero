using UnityEngine;
using System.Collections;

/// <summary>
/// 第4课：Prefab 工作流 + ScriptableObject 实践
/// 根据 SpawnConfig 数据资产生成物体，演示 Prefab 实例化和 SO 数据驱动
/// </summary>
public class Lesson04_Spawner : MonoBehaviour
{
    [Header("=== 生成配置 ===")]
    [SerializeField] private SpawnConfig currentConfig;

    [Header("=== 切换配置 ===")]
    [SerializeField] private SpawnConfig[] allConfigs;
    private int configIndex = 0;

    void Start()
    {
        if (currentConfig == null)
        {
            Debug.LogError("[Lesson04] 没有指定 SpawnConfig！请在 Inspector 中拖入配置。");
            return;
        }
        Debug.Log($"[Lesson04] 当前配置：{currentConfig.configName}");
    }

    void Update()
    {
        // 按 F 键：根据当前配置生成一批物体
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentConfig != null && currentConfig.prefab != null)
            {
                StartCoroutine(SpawnBatch());
            }
            else
            {
                Debug.LogWarning("[Lesson04] 配置或 Prefab 未设置！");
            }
        }

        // 按 Tab 键：切换配置
        if (Input.GetKeyDown(KeyCode.Tab) && allConfigs.Length > 0)
        {
            configIndex = (configIndex + 1) % allConfigs.Length;
            currentConfig = allConfigs[configIndex];
            Debug.Log($"[Lesson04] 切换到配置：{currentConfig.configName}");
        }
    }

    IEnumerator SpawnBatch()
    {
        Debug.Log($"[Lesson04] 开始生成 {currentConfig.spawnCount} 个 {currentConfig.configName}");

        for (int i = 0; i < currentConfig.spawnCount; i++)
        {
            SpawnOne();
            yield return new WaitForSeconds(currentConfig.spawnInterval);
        }

        Debug.Log("[Lesson04] 生成完毕！");
    }

    void SpawnOne()
    {
        // 在生成器周围随机位置生成
        Vector2 randomCircle = Random.insideUnitCircle * currentConfig.spawnRadius;
        Vector3 spawnPos = transform.position + new Vector3(randomCircle.x, 2f, randomCircle.y);

        // 实例化 Prefab
        GameObject obj = Instantiate(currentConfig.prefab, spawnPos, Random.rotation);

        // 应用 SO 中的物理属性
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = currentConfig.useGravity;
            // 随机方向向上发射
            Vector3 launchDir = (Vector3.up + Random.insideUnitSphere * 0.5f).normalized;
            rb.AddForce(launchDir * currentConfig.launchForce, ForceMode.Impulse);
        }

        // 应用 SO 中的颜色
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = currentConfig.color;
        }

        // 定时销毁
        Destroy(obj, currentConfig.lifetime);
    }
}
