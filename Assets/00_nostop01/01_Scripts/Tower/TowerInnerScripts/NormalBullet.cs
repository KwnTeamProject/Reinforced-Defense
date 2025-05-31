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

        // 방향 계산
        Vector3 direction = (target.position - transform.position).normalized;

        // 이동
        transform.position += direction * speed * Time.deltaTime;

        // 회전 (Y+이 총알의 기본 방향인 경우)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == target)
        {
            // 예: 적에게 데미지 적용
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, attackType);
            }

            Destroy(gameObject);
        }
    }
}
