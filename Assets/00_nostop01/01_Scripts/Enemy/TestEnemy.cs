using UnityEngine;

public class TestEnemy : MonoBehaviour,IEnemy
{
    private void OnEnable()
    {
        EnemyManager.Register(this);
    }

    private void OnDisable()
    {
        EnemyManager.Unregister(this);
    }

    private void OnDestroy()
    {
        EnemyManager.Unregister(this);
    }

}
