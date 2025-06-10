using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("����")]
    public float guardSpawnCooldown = 3f;
    public float guardSpawnTimer = 6f;

    [Header("���")]
    public float goblinSpawnCooldown = 2f;
    public float goblinSpawnTimer = 4f;

    public void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        if (MainSystem.mainSystemInstance.isGameEnd || MainSystem.mainSystemInstance.isPaused)
            return;

        if (guardSpawnCooldown <= 0f)
        {
            GuardSpawnEnemy();
            guardSpawnCooldown = guardSpawnTimer;
        }
        else
        {
            guardSpawnCooldown -= Time.deltaTime;
        }

        if (goblinSpawnCooldown <= 0f)
        {
            GoblinSpawnEnemy();
            goblinSpawnCooldown = goblinSpawnTimer;
        }
        else
        {
            goblinSpawnCooldown -= Time.deltaTime;
        }
    }

    private void GuardSpawnEnemy()
    {
        var EnemyGo = ObjectPoolManager.instance.GetGo("GuardEnemy");
    }

    private void GoblinSpawnEnemy()
    {
        var EnemyGo = ObjectPoolManager.instance.GetGo("GoblinEnemy");
    }
}
