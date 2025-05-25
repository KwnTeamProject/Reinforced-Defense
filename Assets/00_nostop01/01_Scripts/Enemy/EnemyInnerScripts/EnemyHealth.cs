using System.Collections;
using UnityEngine;

public enum DamageType
{
    Physical,
    Magical,
    Elemental
}

public class EnemyHealth : PoolAble, IEnemy
{
    public int maxHealth { get; set; } = 100;
    public int currentHealth { get; set; }

    // 방어력
    public int defense { get; set; }

    // 속성별 내성 (0.0f = 무저항, 1.0f = 완전 면역)
    public float physicalResistance { get; set; }
    public float magicalResistance { get; set; }
    public float elementalResistance { get; set; }

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    /// <summary>
    /// 공격으로부터 데미지를 받는 함수
    /// </summary>
    /// <param name="rawDamage">기본 데미지 값</param>
    /// <param name="damageType">속성 타입</param>
    public void TakeDamage(int rawDamage, DamageType damageType)
    {
        float resistance = GetResistanceByType(damageType);

        // 데미지 계산: (데미지 * (1 - 저항)) - 방어력
        float reducedByResistance = rawDamage * (1f - resistance);
        int finalDamage = Mathf.Max(Mathf.RoundToInt(reducedByResistance - defense), 1);  // 최소 1 보장

        currentHealth -= finalDamage;

        Debug.Log($"[{damageType}] 속성 공격: 최종 데미지 {finalDamage}, 남은 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(TakeDamageAfter());
        }
    }

    /// <summary>
    /// 속성 타입에 따른 내성 반환
    /// </summary>
    private float GetResistanceByType(DamageType type)
    {
        switch (type)
        {
            case DamageType.Physical:
                return physicalResistance;
            case DamageType.Magical:
                return magicalResistance;
            case DamageType.Elemental:
                return elementalResistance;
            default:
                return 0f;
        }
    }

    private void Die()
    {
        Debug.Log("적이 사망했습니다.");
        // 사망 처리 로직
        Pool.Release(this.gameObject);
    }

    // 데미지 받은 후 후처리
    private IEnumerator TakeDamageAfter()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }


    public void OnEnable()
    {
        EnemyManager.Register(this);
    }

    public void OnDisable()
    {
        EnemyManager.Unregister(this);
    }
}
