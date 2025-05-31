using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f; // �̵� �ӵ�
    public float rayDistance = 0.25f;

    [SerializeField] private GameObject start_Pos;
    private Vector3 startPos;

    private List<Transform> targetPositions = new List<Transform>(); // Ÿ�� ��ġ ����Ʈ
    private int currentTargetIndex = 0; // ���� Ÿ�� �ε���

    private void Awake()
    {
        start_Pos = GameObject.FindGameObjectWithTag("StartPos");

        startPos = start_Pos.transform.position;
    }

    private void Start()
    {
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
        if (MainSystem.mainSystemInstance.isPaused)
            return;

        // Ÿ���� ������ �̵����� ����
        if (targetPositions.Count == 0) return;

        // ���� Ÿ�� ��ġ
        Transform target = targetPositions[currentTargetIndex];

        // Ÿ���� ���� �̵�
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // �����ߴ��� Ȯ�� �� ���� Ÿ������ �̵�
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentTargetIndex = (currentTargetIndex + 1) % targetPositions.Count; // ��ȯ ����
        }
    }
}