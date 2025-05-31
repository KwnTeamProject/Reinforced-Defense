using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    public DamageType attackType = DamageType.Physical;

    private Transform target;
    private int damage;
    public float speed = 5f;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    void Update()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
            return;
        }

        // ���� ���
        Vector3 direction = (target.position - transform.position).normalized;

        // �̵�
        transform.position += direction * speed * Time.deltaTime;

        // ȸ�� (Y+�� �Ѿ��� �⺻ ������ ���)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == target)
        {
            // ��: ������ ������ ����
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, attackType);
            }

            Destroy(gameObject);
        }
    }
}
