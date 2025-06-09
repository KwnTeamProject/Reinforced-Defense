using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f; // 이동 속도
    public float rayDistance = 0.25f;

    [SerializeField] private GameObject start_Pos;
    private Vector3 startPos;

    private Animator animator; // 애니메이터 컴포넌트 참조

    private List<Transform> targetPositions = new List<Transform>(); // 타겟 위치 리스트
    private int currentTargetIndex = 0; // 현재 타겟 인덱스

    private Vector3 previousPosition; // 이전 프레임 위치

    private void Awake()
    {
        start_Pos = GameObject.FindGameObjectWithTag("StartPos");

        startPos = start_Pos.transform.position;
    }

    private void Start()
    {
        // 애니메이터 가져오기
        animator = GetComponent<Animator>();

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
        if (MainSystem.mainSystemInstance.isPaused || MainSystem.mainSystemInstance.isGameEnd)
            return;

        if (targetPositions.Count == 0) return;

        Transform target = targetPositions[currentTargetIndex];
        Vector3 direction = (target.position - transform.position).normalized;

        // 이동 처리
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // 애니메이션 방향 설정
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

    // 방향 벡터에 따라 애니메이터 Bool 설정
    private void SetDirectionBools(Vector3 direction)
    {
        // 먼저 모든 방향 false로 초기화
        animator.SetBool("ToFront", false);
        animator.SetBool("ToBack", false);
        animator.SetBool("ToLeft", false);
        animator.SetBool("ToRight", false);

        // 가장 큰 축 기준으로 방향 판별 (단순화)
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0)
                animator.SetBool("ToFront", true);     // 위쪽
            else
                animator.SetBool("ToBack", true);    // 아래쪽
        }
        else
        {
            if (direction.x > 0)
                animator.SetBool("ToRight", true);    // 오른쪽
            else
                animator.SetBool("ToLeft", true);     // 왼쪽
        }
    }
}