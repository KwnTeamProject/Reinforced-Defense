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
        // 0, 1, 2 중 랜덤 선택
        int randomIndex = Random.Range(0, 3); // upper bound는 포함되지 않으므로 0~2
        string selectedPoolName = upgradeTowerPoolNames[randomIndex];

        Debug.Log($"{name} 업그레이드! 선택된 업그레이드 번호: {randomIndex}");

        TowerManager.Instance.UpgradeTower(this, selectedPoolName);
    }

    // 오브젝트가 활성화될 때 TowerManager에 등록
    private void OnEnable()
    {
        TowerManager.Instance.RegisterTower(this);
    }

    // 오브젝트가 비활성화될 때 TowerManager에서 해제
    private void OnDisable()
    {
        TowerManager.Instance.UnregisterTower(this);
    }
}
