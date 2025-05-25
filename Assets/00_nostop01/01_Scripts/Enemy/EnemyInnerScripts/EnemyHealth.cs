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

    // ����
    public int defense { get; set; }

    // �Ӽ��� ���� (0.0f = ������, 1.0f = ���� �鿪)
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
        Pool.Release(this.gameObject);
    }

    // ������ ���� �� ��ó��
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
