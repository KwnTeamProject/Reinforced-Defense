using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject testEnemy;

    public float spawnCooldown = 3f;
    public float spawnTimer = 3f;

    public void Update()
    {
        if (MainSystem.mainSystemInstance.isPaused)
            return;

        if (spawnCooldown <= 0f)
        {
            SpawnEnemy();
            spawnCooldown = 1f/spawnTimer;
        }
        else
        {
            spawnCooldown -= Time.deltaTime;
        }
    }

    private void SpawnEnemy()
    {
        var EnemyGo = ObjectPoolManager.instance.GetGo("TestEnemy");
    }
}
