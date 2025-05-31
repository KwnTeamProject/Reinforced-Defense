using UnityEngine;

public class NormalTower : PoolAble, ITower
{
    public int AttackPower { get; set; } = 40;
    public int AttackSpeed { get; set; } = 2;
    public float AttackRange { get; set; } = 2.25f;

    [SerializeField] private string[] upgradeTowerPoolNames;

    public GameObject normalTowerBullet;

    private float attackCooldown = 0f;

    public Transform GetTransform() => transform;

    void Update()
    {
        if (attackCooldown <= 0f)
        {
            Attack();
            attackCooldown = 1f / AttackSpeed; // ���� �ӵ� ����
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public void TowerDeSpawn()
    {
        Pool.Release(this.gameObject);
    }

    public void Attack()
    {
        // 1. AttackRange �� �� ���� (2D)
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, AttackRange, LayerMask.GetMask("Enemy"));

        if (hits.Length == 0)
            return;

        // 2. ���� ���� ���� ��(����Ʈ ù ��°)�� �켱 Ÿ��
        Transform target = hits[0].transform;

        // 3. �Ѿ� ���� �� Ÿ�� ����
        GameObject bullet = Instantiate(normalTowerBullet, transform.position, Quaternion.identity);

        NormalBullet bulletComp = bullet.GetComponent<NormalBullet>();
        if (bulletComp != null)
        {
            bulletComp.SetTarget(target);
            bulletComp.SetDamage(AttackPower);
        }
    }

    public void Upgrade()
    {
        // 0, 1, 2 �� ���� ����
        int randomIndex = Random.Range(0, 3); // upper bound�� ���Ե��� �����Ƿ� 0~2
        string selectedPoolName = upgradeTowerPoolNames[randomIndex];

        Debug.Log($"{name} ���׷��̵�! ���õ� ���׷��̵� ��ȣ: {randomIndex}");

        TowerManager.Instance.UpgradeTower(this, selectedPoolName);
    }

    // ������Ʈ�� Ȱ��ȭ�� �� TowerManager�� ���
    private void OnEnable()
    {
        TowerManager.Instance.RegisterTower(this);
    }

    // ������Ʈ�� ��Ȱ��ȭ�� �� TowerManager���� ����
    private void OnDisable()
    {
        TowerManager.Instance.UnregisterTower(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
