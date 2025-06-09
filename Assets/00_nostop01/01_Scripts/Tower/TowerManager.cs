using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TowerManager : PoolAble
{
    private readonly List<ITower> towers = new List<ITower>();

    [SerializeField, Tooltip("디버깅용 - 현재 등록된 타워들 (에디터에서만 보기용)")]
    private List<MonoBehaviour> debugTowers = new List<MonoBehaviour>();

    public static TowerManager Instance { get; private set; }

    Camera mainCam;
    public ITower selectedTower;

    public GameObject[] towerPos;
    public Vector3[] towerVec;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        mainCam = Camera.main;

        for (int i = 0; i < towerPos.Length; i++)
        {
            if (towerPos[i] != null)
            {
                towerVec[i] = towerPos[i].transform.position;
            }
            else
            {
                return;
            }

        }
    }

    public void Update()
    {
        TowerPosSet();

        // 모바일 터치 우선 처리
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            HandleInput(Input.GetTouch(0).position);
        }

        else if (Input.GetMouseButtonDown(0))
        {
            HandleInput(Input.mousePosition);
        }
    }

    void HandleInput(Vector2 screenPosition)
    {
        Vector2 worldPos = mainCam.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
        {
            ITower tower = hit.collider.GetComponent<ITower>();
            if (tower != null)
            {
                // 같은 타워를 다시 선택한 경우 → 선택 해제
                if (selectedTower == tower)
                {
                    DeselectTower();
                }
                else
                {
                    SelectTower(tower);
                }
                return;
            }
        }
    }

    void SelectTower(ITower tower)
    {
        if (selectedTower != null && selectedTower != tower)
        {
            DeselectTower();
        }

        selectedTower = tower;
        selectedTower.OnSelected();
    }

    void DeselectTower()
    {
        selectedTower.OnDeselected();

        selectedTower = null;
        // 선택 해제 시 처리 로직 (예: UI 닫기 등)
    }

    public void TowerPosSet()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i] != null)
            {
                towers[i].GetTransform().position = towerVec[i];
            }
        }
    }

    public void TowerSpawn()
    {
        // 최대 개수 제한 체크
        if (towerPos.Length <= towers.Count)
        {
            Debug.Log("최대 타워 개수입니다.");
            return;
        }

        // 앞에서부터 비어 있는 위치 찾기
        for (int i = 0; i < towerPos.Length; i++)
        {
            // 해당 위치가 이미 사용 중인지 확인
            bool positionOccupied = false;

            foreach (var tower in towers)
            {
                if (tower != null && tower.GetTransform().position == towerVec[i])
                {
                    positionOccupied = true;
                    break;
                }
            }

            // 비어 있다면 타워 배치
            if (!positionOccupied)
            {
                GameObject towerGo = ObjectPoolManager.instance.GetGo("NormalTower");
                towerGo.transform.SetPositionAndRotation(towerVec[i], Quaternion.identity);
                towerGo.transform.SetParent(transform); // 또는 원하는 부모

                ITower tower = towerGo.GetComponent<ITower>();
                if (tower != null)
                {
                    RegisterTower(tower);
                }

                return; // 한 칸만 배치하고 종료
            }
        }

        Debug.Log("모든 위치가 이미 타워로 채워져 있습니다.");

    }

    public void TowerDeSpawn()
    {
        if (selectedTower == null)
        {
            Debug.LogWarning("[TowerManager] 선택된 타워가 없습니다.");
            return;
        }

        // 백업 후 null 처리 전에 작업
        ITower towerToRemove = selectedTower;

        int index = towers.IndexOf(towerToRemove);
        if (index >= 0)
        {
            // 먼저 리스트에서 제거
            UnregisterTower(towerToRemove);

            // 풀 반환 (GameObject 기준)
            towerToRemove.TowerDeSpawn();

            // 마지막에 선택 해제
            DeselectTower();
        }
        else
        {
            Debug.LogWarning("[TowerManager] 선택된 타워가 towers 리스트에 존재하지 않습니다.");
        }
    }

    public void RegisterTower(ITower tower)
    {
        if (tower != null && !towers.Contains(tower))
        {
            towers.Add(tower);

#if UNITY_EDITOR
            MonoBehaviour mb = tower as MonoBehaviour;
            if (mb != null && !debugTowers.Contains(mb))
            {
                debugTowers.Add(mb);
            }
#endif
        }
    }

    public void UnregisterTower(ITower tower)
    {
        if (tower != null)
        {
            int index = towers.IndexOf(tower);

            if (index >= 0)
            {
                towers.RemoveAt(index);
#if UNITY_EDITOR
                if (index < debugTowers.Count)
                {
                    debugTowers.RemoveAt(index);
                }
#endif
            }
        }
    }

    public void UpgradeTower(string newTowerPoolName)
    {
        if (selectedTower == null) return;

        Transform oldTransform = selectedTower.GetTransform();
        Vector3 pos = oldTransform.position;
        Quaternion rot = oldTransform.rotation;
        Transform parent = oldTransform.parent;
        int index = towers.IndexOf(selectedTower);

        // 기존 타워 오브젝트 반환 (Object Pool로)
        GameObject oldTowerGo = oldTransform.gameObject;
        UnregisterTower(selectedTower); // 리스트에서도 제거

        // 새 타워 오브젝트 생성
        GameObject newTowerGo = ObjectPoolManager.instance.GetGo(newTowerPoolName);
        newTowerGo.transform.SetPositionAndRotation(pos, rot);
        newTowerGo.transform.SetParent(parent);

        ITower newTower = newTowerGo.GetComponent<ITower>();
        if (newTower == null)
        {
            Debug.LogError($"[UpgradeTower] 새 타워가 ITower를 구현하고 있지 않습니다: {newTowerGo.name}");
            return;
        }

        // 인덱스 기준 삽입 (리스트 동기화)
        if (index >= 0 && index <= towers.Count)
        {
            towers.Insert(index, newTower);

#if UNITY_EDITOR

            MonoBehaviour mb = newTower as MonoBehaviour;
            if (mb != null)
            {
                debugTowers.Insert(index, mb); // 동일 인덱스에 삽입
            }
#endif
        }

        else
        {
            towers.Add(newTower);

#if UNITY_EDITOR

            MonoBehaviour mb = newTower as MonoBehaviour;
            if (mb != null)
            {
                debugTowers.Add(mb); //예외 시 맨 뒤에 추가
            }
#endif
        }

        selectedTower = newTower;
        selectedTower.OnSelected(); // 선택 상태 유지
    }
}
