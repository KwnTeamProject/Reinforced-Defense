using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    private int i = 0;

    public void TowerSpawn()
    {
        if(TowerManager.Instance.towerPos.Length <= i)
        {
            Debug.Log("�ִ� Ÿ�� �����Դϴ�.");
        }

        else
        {
            var TowerGo = ObjectPoolManager.instance.GetGo("NormalTower");
            i++;
        }
        
    }

    public void TowerDeSpawn()
    {
        TowerManager.Instance.TowerDeSpawn(0);
        i--;
    }
}
