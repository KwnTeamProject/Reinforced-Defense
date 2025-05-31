using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f; // 이동 속도
    public float rayDistance = 0.25f;

    [SerializeField] private GameObject start_Pos;
    private Vector3 startPos;

    private List<Transform> targetPositions = new List<Transform>(); // 타겟 위치 리스트
    private int currentTargetIndex = 0; // 현재 타겟 인덱스

    private void Awake()
    {
        start_Pos = GameObject.FindGameObjectWithTag("StartPos");

        startPos = start_Pos.transform.position;
    }

    private void Start()
    {
        // "TargetGroup"이라는 이름을 가진 부모 오브젝트 하위의 타겟들을 순서대로 추가
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
            Debug.LogWarning("TargetGroup의 자식이 존재하지 않거나 그룹을 찾을 수 없습니다.");
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

        // 타겟이 없으면 이동하지 않음
        if (targetPositions.Count == 0) return;

        // 현재 타겟 위치
        Transform target = targetPositions[currentTargetIndex];

        // 타겟을 향해 이동
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // 도달했는지 확인 후 다음 타겟으로 이동
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentTargetIndex = (currentTargetIndex + 1) % targetPositions.Count; // 순환 구조
        }
    }
}