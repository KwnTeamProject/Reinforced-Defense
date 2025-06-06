using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeButtonSetting : MonoBehaviour
{
    [SerializeField] private Button actionButton;

    void Start()
    {
        actionButton.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        var selected = TowerManager.Instance.selectedTower;

        if (selected != null)
        {
            Debug.Log(selected.ToString());
            selected.Upgrade();
        }

        else
        {
            Debug.Log("선택된 타워가 없습니다.");
        }
    }
}
