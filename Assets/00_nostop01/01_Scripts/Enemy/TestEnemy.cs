using UnityEngine;

public class TestEnemy : PoolAble, IEnemy
{
    public float speed { get; set; } = 2f;
    public float rayDistance { get; set; } = 0.5f;

    public void OnEnable()
    {
        EnemyManager.Register(this);
    }

    public void OnDisable()
    {
        EnemyManager.Unregister(this);
    }

    private void OnDestroy()
    {
        EnemyManager.Unregister(this);
    }
}
