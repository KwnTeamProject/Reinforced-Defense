using System.Collections.Generic;
using UnityEngine;

public class TowerManager : PoolAble
{
    private readonly List<ITower> towers = new List<ITower>();

    // �̱���
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

        // ���� Ÿ�� ������Ʈ ��ȯ (Object Pool��)
        GameObject oldTowerGo = oldTransform.gameObject;
        UnregisterTower(oldTower); // ����Ʈ������ ����
        Pool.Release(oldTowerGo);  // Ǯ�� ��ȯ

        // �� Ÿ�� ������Ʈ ����
        GameObject newTowerGo = ObjectPoolManager.instance.GetGo(newTowerPoolName);
        newTowerGo.transform.SetPositionAndRotation(pos, rot);
        newTowerGo.transform.SetParent(parent);
    }
}
