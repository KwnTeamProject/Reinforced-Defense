using UnityEngine;

public class NormalTower : PoolAble, ITower
{
    public int AttackPower { get; set; } = 0;
    public int AttackSpeed { get; set; } = 2;
    public int AttackRange { get; set; } = 2;

    [SerializeField] private string[] upgradeTowerPoolNames;

    public GameObject normalTowerBullet;

    public Transform GetTransform() => transform;

    void Update()
    {

    }

    public void Attack()
    {
        Instantiate(normalTowerBullet);
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
}
