using UnityEngine;
using System.Collections;

// 第6课：敌人生成器
// 挂在一个空物体上，定时在屏幕上方随机位置生成敌人
public class EnemySpawner : MonoBehaviour
{
    [Header("敌人设置")]
    [SerializeField] private GameObject enemyPrefab;   // 敌人预制体
    [SerializeField] private float spawnInterval = 1.5f; // 生成间隔（秒）

    [Header("生成范围")]
    [SerializeField] private float spawnY = 6f;         // 生成的Y坐标（屏幕上方外面）
    [SerializeField] private float spawnRangeX = 7f;    // X轴随机范围（-7到+7）

    void Start()
    {
        // 用协程实现定时生成
        // 为什么用协程而不是 Update + 计时器？
        // 两种方式都行，但协程更清晰：
        // "等一段时间→做一件事→循环" 这种模式天然适合协程的 yield return 写法
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        // 无限循环 = 只要这个 GameObject 还活着就一直生成敌人
        while (true)
        {
            SpawnEnemy();
            // WaitForSeconds 暂停协程指定的秒数，不阻塞主线程
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner: 请在 Inspector 中设置 enemyPrefab！");
            return;
        }

        // Random.Range(min, max) 生成一个随机浮点数
        // 让每个敌人出现在不同的X位置，增加游戏变化性
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
