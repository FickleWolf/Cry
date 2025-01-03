using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyDatabase", menuName = "Database/EnemyDatabase")]
public class EnemyDatabase : ScriptableObject
{
    public EnemyData[] enemies;

    public EnemyData GetEnemyByName(string enemyName)
    {
        EnemyData enemy = enemies.FirstOrDefault(enemy => enemy.enemyName == enemyName);

        if (enemy == null)
        {
            string errorMessage = $"EnemyDatabase: 名前 '{enemyName}' の EnemyData が見つかりませんでした。";
            Debug.LogError(errorMessage);
            throw new KeyNotFoundException(errorMessage);
        }

        return enemy;
    }

    public EnemyData GetRandomEnemy(string groundName)
    {
        var targetEnemies = enemies
            .Where(enemy => enemy != null && (enemy.spawnGround == groundName || enemy.spawnGround == "ALL"))
            .SelectMany(enemy => Enumerable.Repeat(enemy, enemy.spawnRate))
            .ToList();

        if (targetEnemies.Count == 0)
        {
            Debug.LogWarning($"EnemyDatabase: 名前 '{groundName}' の対象敵が存在しません。");
            return null;
        }

        return targetEnemies[Random.Range(0, targetEnemies.Count)];
    }
}
