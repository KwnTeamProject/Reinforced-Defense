using System.Collections;
using UnityEngine;

// by �̻���: UI
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

    // ����
    public int defense { get; set; } = 10;

    // �Ӽ��� ���� (0.0f = ������, 1.0f = ���� �鿪)
    public float physicalResistance { get; set; } = 0.5f;
    public float magicalResistance { get; set; } = 1.0f;
    public float elementalResistance { get; set; } = 1.0f;

    private SpriteRenderer spriteRenderer;

    // by �̻���: HP bar
    [SerializeField] Slider HPbar;
    // = = = = = = = = =

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    /// <summary>
    /// �������κ��� �������� �޴� �Լ�
    /// </summary>
    /// <param name="rawDamage">�⺻ ������ ��</param>
    /// <param name="damageType">�Ӽ� Ÿ��</param>
    public void TakeDamage(int rawDamage, DamageType damageType)
    {
        float resistance = GetResistanceByType(damageType);

        // ������ ���: (������ * (1 - ����)) - ����
        float reducedByResistance = rawDamage * (1f - resistance);
        int finalDamage = Mathf.Max(Mathf.RoundToInt(reducedByResistance - defense), 1);  // �ּ� 1 ����

        currentHealth -= finalDamage;

        // by �̻���: HP bar value Setting
        
        float HPrate = (float)currentHealth / (float)maxHealth;
        HPbar.value = HPrate;
        Debug.LogFormat("current:{0} , max:{1} , rate:{2}", currentHealth, maxHealth, HPrate);
        // = = = = = = = = = = = = = = = =

        Debug.Log($"[{damageType}] �Ӽ� ����: ���� ������ {finalDamage}, ���� ü��: {currentHealth}");

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
    /// �Ӽ� Ÿ�Կ� ���� ���� ��ȯ
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
        Debug.Log("���� ����߽��ϴ�.");

        // ��� ó�� ����
        currentHealth = maxHealth;
        spriteRenderer.color = Color.white;
        Pool.Release(this.gameObject);
    }

    // ������ ���� �� ��ó��
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
