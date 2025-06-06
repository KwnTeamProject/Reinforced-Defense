using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SwordTower : PoolAble, ITower
{
    public int AttackPower { get; set; } = 50;
    public int AttackSpeed { get; set; } = 2;
    public float AttackRange { get; set; } = 2.25f;

    public GameObject hitEffectPrefab;

    private Material defaultMat;
    public Material selectedMat;

    private SpriteRenderer spriteRenderer;

    private DamageType damageType = DamageType.Physical;
    private float attackCooldown = 0f;

    public Transform GetTransform() => transform;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
    }

    void Update()
    {
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

    public void Upgrade()
    {

    }

    public void Attack()
    {
        // 1. AttackRange 내 적 감지 (2D)
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, AttackRange, LayerMask.GetMask("Enemy"));

        if (hits.Length == 0)
            return;

        // 2. 공격 시점 기준 공격 범위 내 모든 적에게 공격 실행
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].GetComponent<EnemyHealth>().TakeDamage(AttackPower, damageType);

            Vector3 hitPosition = hits[i].transform.position;
            GameObject effect = Instantiate(hitEffectPrefab, hitPosition, Quaternion.identity);
        }

        // 3. 총알 생성 및 타겟 설정
        
    }

    // 오브젝트가 활성화될 때 TowerManager에 등록
    private void OnEnable()
    {
        
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
