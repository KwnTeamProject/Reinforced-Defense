using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f; // �̵� �ӵ�
    public float rayDistance = 0.25f;

    [SerializeField] private GameObject start_Pos;
    private Vector3 startPos;

    private Animator animator; // �ִϸ����� ������Ʈ ����

    private List<Transform> targetPositions = new List<Transform>(); // Ÿ�� ��ġ ����Ʈ
    private int currentTargetIndex = 0; // ���� Ÿ�� �ε���

    private Vector3 previousPosition; // ���� ������ ��ġ

    private void Awake()
    {
        start_Pos = GameObject.FindGameObjectWithTag("StartPos");

        startPos = start_Pos.transform.position;
    }

    private void Start()
    {
        // �ִϸ����� ��������
        animator = GetComponent<Animator>();

        // "TargetGroup"�̶�� �̸��� ���� �θ� ������Ʈ ������ Ÿ�ٵ��� ������� �߰�
        GameObject group = GameObject.Find("TargetGroup");
        if (group != null)
        {
            foreach (Transform child in group.transform)
            {
                targetPositions.Add(child);
            }
        }

        if (targetPositions.Count == 0)
        {
            Debug.LogWarning("TargetGroup�� �ڽ��� �������� �ʰų� �׷��� ã�� �� �����ϴ�.");
        }
    }

    private void OnDisable()
    {
        transform.position = startPos;

        currentTargetIndex = 0;
    }

    private void Update()
    {
        if (MainSystem.mainSystemInstance.isPaused || MainSystem.mainSystemInstance.isGameEnd)
            return;

        if (targetPositions.Count == 0) return;

        Transform target = targetPositions[currentTargetIndex];
        Vector3 direction = (target.position - transform.position).normalized;

        // �̵� ó��
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // �ִϸ��̼� ���� ����
        if (animator != null)
        {
            SetDirectionBools(direction);
        }

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentTargetIndex = (currentTargetIndex + 1) % targetPositions.Count;
        }

        previousPosition = transform.position;
    }

    // ���� ���Ϳ� ���� �ִϸ����� Bool ����
    private void SetDirectionBools(Vector3 direction)
    {
        // ���� ��� ���� false�� �ʱ�ȭ
        animator.SetBool("ToFront", false);
        animator.SetBool("ToBack", false);
        animator.SetBool("ToLeft", false);
        animator.SetBool("ToRight", false);

        // ���� ū �� �������� ���� �Ǻ� (�ܼ�ȭ)
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0)
                animator.SetBool("ToFront", true);     // ����
            else
                animator.SetBool("ToBack", true);    // �Ʒ���
        }
        else
        {
            if (direction.x > 0)
                animator.SetBool("ToRight", true);    // ������
            else
                animator.SetBool("ToLeft", true);     // ����
        }
    }
}