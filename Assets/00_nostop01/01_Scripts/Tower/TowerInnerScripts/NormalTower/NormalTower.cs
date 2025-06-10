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
        if (MainSystem.mainSystemInstance.byProductCount < needUpgradeProductCount)
        {
            float sum = needUpgradeProductCount - MainSystem.mainSystemInstance.byProductCount;

            needToProduct.SetActive(true);
            needToProductText.text = "�λ깰�� �����մϴ�.\n" + "(������ �λ깰 �� : " + Mathf.Abs(sum).ToString() + ")";

            return;
        }

        MainSystem.mainSystemInstance.MinusProduct(needUpgradeProductCount);

        // 0, 1, 2 �� ���� ����
        //int randomIndex = Random.Range(0, 3); // upper bound�� ���Ե��� �����Ƿ� 0~2
        int randomIndex = Random.Range(0, 2); // �ӽ� �׽�Ʈ��
        string selectedPoolName = upgradeTowerPoolNames[randomIndex];

        Debug.Log($"{name} ���׷��̵�! ���õ� ���׷��̵� ��ȣ: {randomIndex}");

        TowerManager.Instance.UpgradeTower(selectedPoolName);
        Pool.Release(this.gameObject);
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
