using UnityEngine;
using UnityEngine.UI;

public class NormalTower : PoolAble, ITower
{
    public int AttackPower { get; set; } = 30;
    public int AttackSpeed { get; set; } = 1;
    public float AttackRange { get; set; } = 2.5f;

    private float needUpgradeProductCount = 10;
    public string TowerName { get; set; } = "Normal Tower";


    [SerializeField] private string[] upgradeTowerPoolNames;

    public GameObject normalTowerBullet;

    public GameObject needToProduct;
    public Text needToProductText;

    private Material defaultMat;
    public Material selectedMat;

    private SpriteRenderer spriteRenderer;

    private float attackCooldown = 0f;

    public Transform GetTransform() => transform;

    void Awake()
    {
        needToProduct = GameObject.Find("DonotHaveProductPanel");
        needToProductText = GameObject.Find("DonotHaveProductText").GetComponent<Text>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
        needUpgradeProductCount = 10;
    }

    void Update()
    {
        if (MainSystem.mainSystemInstance.isPaused || MainSystem.mainSystemInstance.isGameEnd)
            return;

        if (attackCooldown <= 0f)
        {
            Attack();
            attackCooldown = 1f / AttackSpeed; // 공격 속도 적용
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

    public void OnSelected()
    {
        spriteRenderer.material = selectedMat;
    }

    public void OnDeselected()
    {
        spriteRenderer.material = defaultMat;
    }
    

    public void Attack()
    {
        // 1. AttackRange 내 적 감지 (2D)
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, AttackRange, LayerMask.GetMask("Enemy"));

        if (hits.Length == 0)
            return;

        // 2. 가장 먼저 들어온 적(리스트 첫 번째)을 우선 타겟
        Transform target = hits[0].transform;

        // 3. 총알 생성 및 타겟 설정
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
        if (MainSystem.mainSystemInstance.byProductCount < needUpgradeProductCount)
        {
            float sum = needUpgradeProductCount - MainSystem.mainSystemInstance.byProductCount;

            needToProduct.SetActive(true);
            needToProductText.text = "부산물이 부족합니다.\n" + "(부족한 부산물 수 : " + Mathf.Abs(sum).ToString() + ")";

            return;
        }

        MainSystem.mainSystemInstance.MinusProduct(needUpgradeProductCount);

        // 0, 1, 2 중 랜덤 선택
        //int randomIndex = Random.Range(0, 3); // upper bound는 포함되지 않으므로 0~2
        int randomIndex = Random.Range(0, 2); // 임시 테스트용
        string selectedPoolName = upgradeTowerPoolNames[randomIndex];

        Debug.Log($"{name} 업그레이드! 선택된 업그레이드 번호: {randomIndex}");

        TowerManager.Instance.UpgradeTower(selectedPoolName);
        Pool.Release(this.gameObject);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
