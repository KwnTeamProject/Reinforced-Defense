using System.Collections.Generic;
using UnityEngine;

public class TowerManager : PoolAble
{
    private readonly List<ITower> towers = new List<ITower>();

    // 싱글톤
    public static TowerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RegisterTower(ITower tower)
    {
        if (tower != null && !towers.Contains(tower))
        {
            towers.Add(tower);
        }
    }

    public void UnregisterTower(ITower tower)
    {
        if (tower != null)
        {
            towers.Remove(tower);
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
