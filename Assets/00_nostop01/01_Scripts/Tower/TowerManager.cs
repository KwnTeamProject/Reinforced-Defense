using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TowerManager : PoolAble
{
    private readonly List<ITower> towers = new List<ITower>();

    [SerializeField, Tooltip("������ - ���� ��ϵ� Ÿ���� (�����Ϳ����� �����)")]
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

        // ����� ��ġ �켱 ó��
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
                // ���� Ÿ���� �ٽ� ������ ��� �� ���� ����
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
        // ���� ���� �� ó�� ���� (��: UI �ݱ� ��)
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
        // �ִ� ���� ���� üũ
        if (towerPos.Length <= towers.Count)
        {
            Debug.Log("�ִ� Ÿ�� �����Դϴ�.");
            return;
        }

        // �տ������� ��� �ִ� ��ġ ã��
        for (int i = 0; i < towerPos.Length; i++)
        {
            // �ش� ��ġ�� �̹� ��� ������ Ȯ��
            bool positionOccupied = false;

            foreach (var tower in towers)
            {
                if (tower != null && tower.GetTransform().position == towerVec[i])
                {
                    positionOccupied = true;
                    break;
                }
            }

            // ��� �ִٸ� Ÿ�� ��ġ
            if (!positionOccupied)
            {
                GameObject towerGo = ObjectPoolManager.instance.GetGo("NormalTower");
                towerGo.transform.SetPositionAndRotation(towerVec[i], Quaternion.identity);
                towerGo.transform.SetParent(transform); // �Ǵ� ���ϴ� �θ�

                ITower tower = towerGo.GetComponent<ITower>();
                if (tower != null)
                {
                    RegisterTower(tower);
                }

                return; // �� ĭ�� ��ġ�ϰ� ����
            }
        }

        Debug.Log("��� ��ġ�� �̹� Ÿ���� ä���� �ֽ��ϴ�.");

    }

    public void TowerDeSpawn()
    {
        if (selectedTower == null)
        {
            Debug.LogWarning("[TowerManager] ���õ� Ÿ���� �����ϴ�.");
            return;
        }

        // ��� �� null ó�� ���� �۾�
        ITower towerToRemove = selectedTower;

        int index = towers.IndexOf(towerToRemove);
        if (index >= 0)
        {
            // ���� ����Ʈ���� ����
            UnregisterTower(towerToRemove);

            // Ǯ ��ȯ (GameObject ����)
            towerToRemove.TowerDeSpawn();

            // �������� ���� ����
            DeselectTower();
        }
        else
        {
            Debug.LogWarning("[TowerManager] ���õ� Ÿ���� towers ����Ʈ�� �������� �ʽ��ϴ�.");
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

        // ���� Ÿ�� ������Ʈ ��ȯ (Object Pool��)
        GameObject oldTowerGo = oldTransform.gameObject;
        UnregisterTower(selectedTower); // ����Ʈ������ ����

        // �� Ÿ�� ������Ʈ ����
        GameObject newTowerGo = ObjectPoolManager.instance.GetGo(newTowerPoolName);
        newTowerGo.transform.SetPositionAndRotation(pos, rot);
        newTowerGo.transform.SetParent(parent);

        ITower newTower = newTowerGo.GetComponent<ITower>();
        if (newTower == null)
        {
            Debug.LogError($"[UpgradeTower] �� Ÿ���� ITower�� �����ϰ� ���� �ʽ��ϴ�: {newTowerGo.name}");
            return;
        }

        // �ε��� ���� ���� (����Ʈ ����ȭ)
        if (index >= 0 && index <= towers.Count)
        {
            towers.Insert(index, newTower);

#if UNITY_EDITOR

            MonoBehaviour mb = newTower as MonoBehaviour;
            if (mb != null)
            {
                debugTowers.Insert(index, mb); // ���� �ε����� ����
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
                debugTowers.Add(mb); //���� �� �� �ڿ� �߰�
            }
#endif
        }

        selectedTower = newTower;
        selectedTower.OnSelected(); // ���� ���� ����
    }
}
