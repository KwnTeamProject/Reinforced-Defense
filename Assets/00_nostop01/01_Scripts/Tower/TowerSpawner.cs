using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    public void TowerSpawn()
    {
        TowerManager.Instance.TowerSpawn();
    }

    public void TowerDeSpawn()
    {
        TowerManager.Instance.TowerDeSpawn();
    }
}
