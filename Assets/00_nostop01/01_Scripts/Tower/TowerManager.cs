using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TowerManager : PoolAble
{
    private readonly List<ITower> towers = new List<ITower>();

    [SerializeField, Tooltip("디버깅용 - 현재 등록된 타워들 (에디터에서만 보기용)")]
    private List<MonoBehaviour> debugTowers = new List<MonoBehaviour>();

    public static TowerManager Instance { get; private set; }

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

        for (int i = 0; i < towerPos.Length; i++)
        {
            if(towerPos[i] != null)
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

    public void TowerDeSpawn(int count)
    {
        towers[count].TowerDeSpawn();
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
            towers.Remove(tower);

#if UNITY_EDITOR
            MonoBehaviour mb = tower as MonoBehaviour;
            if (mb != null)
            {
                debugTowers.Remove(mb);
            }
#endif
        }
    }

    public void UpgradeTower(ITower oldTower, string newTowerPoolName)
    {
        if (oldTower == null) return;

        Transform oldTransform = oldTower.GetTransform();
        Vector3 pos = oldTransform.position;
        Quaternion rot = oldTransform.rotation;
        Transform parent = oldTransform.parent;

        // 기존 타워 오브젝트 반환 (Object Pool로)
        GameObject oldTowerGo = oldTransform.gameObject;
        UnregisterTower(oldTower); // 리스트에서도 제거
        Pool.Release(oldTowerGo);  // 풀로 반환

        // 새 타워 오브젝트 생성
        GameObject newTowerGo = ObjectPoolManager.instance.GetGo(newTowerPoolName);
        newTowerGo.transform.SetPositionAndRotation(pos, rot);
        newTowerGo.transform.SetParent(parent);
    }
}
