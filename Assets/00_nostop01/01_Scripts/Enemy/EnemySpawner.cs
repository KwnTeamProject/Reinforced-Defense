using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject testEnemy;

    public void Update()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(testEnemy);
        }
    }
}
