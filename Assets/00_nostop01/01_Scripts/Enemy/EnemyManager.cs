using System.Collections.Generic;
using UnityEngine;

public class EnemyManager:MonoBehaviour
{
    private static readonly List<IEnemy> _enemies = new List<IEnemy>();
    public static IReadOnlyList<IEnemy> AllEnemies => _enemies;
    public static int EnemyCount => _enemies.Count;

    public static void Register(IEnemy enemy)
    {
        if (!_enemies.Contains(enemy))
            _enemies.Add(enemy);

        MainSystem.mainSystemInstance.currentEnemyCount = EnemyCount;
    }

    public static void Unregister(IEnemy enemy)
    {
        _enemies.Remove(enemy);
    }
}
