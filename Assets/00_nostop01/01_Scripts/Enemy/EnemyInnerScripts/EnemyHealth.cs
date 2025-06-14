using System.Collections;
using UnityEngine;

// by 이상협: UI
using UnityEngine.UI;
// = = = = = = =

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
    public int defense { get; set; } = 10;

    // 속성별 내성 (0.0f = 무저항, 1.0f = 완전 면역)
    public float physicalResistance { get; set; } = 0.1f;
    public float magicalResistance { get; set; } = 0.5f;
    public float elementalResistance { get; set; } = 1.0f;

    private SpriteRenderer spriteRenderer;

    // by 이상협: HP bar
    [SerializeField] Slider HPbar;
    // = = = = = = = = =

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

        // by 이상협: HP bar value Setting
        
        float HPrate = (float)currentHealth / (float)maxHealth;
        HPbar.value = HPrate;
        Debug.LogFormat("current:{0} , max:{1} , rate:{2}", currentHealth, maxHealth, HPrate);
        // = = = = = = = = = = = = = = = =

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

        MainSystem.mainSystemInstance.PlusProduct(1);

        // 사망 처리 로직
        currentHealth = maxHealth;
        float HPrate = (float)currentHealth / (float)maxHealth;
        HPbar.value = HPrate;
        spriteRenderer.color = Color.white;
        Pool.Release(this.gameObject);
    }

    // 데미지 받은 후 후처리
    private IEnumerator TakeDamageAfter()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.05f);
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
