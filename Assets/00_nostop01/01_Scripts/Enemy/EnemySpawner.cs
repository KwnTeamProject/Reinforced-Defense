using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject testEnemy;

    public void Update()
    {
        if (MainSystem.mainSystemInstance.isPaused)
            return;

        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            var EnemyGo = ObjectPoolManager.instance.GetGo("TestEnemy");
        }
    }
}
