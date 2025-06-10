using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TowerInfoUI : MonoBehaviour
{
    public Text towerName;
    
    [Header("아이콘 리스트")]
    public GameObject defaultIcon;
    public GameObject normalIcon;
    public GameObject swordIcon;
    public GameObject magicIcon;
    public GameObject fairyIcon;
    public GameObject upgradeText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultIcon.SetActive(true);

        normalIcon.SetActive(false);
        swordIcon.SetActive(false);
        fairyIcon.SetActive(false);
        magicIcon.SetActive(false);
        upgradeText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SetIcon();
    }

    public void SetIcon()
    {
        towerName.text = TowerManager.Instance.selectedTower.TowerName;

        if(TowerManager.Instance.selectedTower == null)
        {
            towerName.text = "Tower not selected";
        }


        if(TowerManager.Instance.selectedTower.TowerName == "Normal Tower")
        {
            normalIcon.SetActive(true);

            defaultIcon.SetActive(false);
            swordIcon.SetActive(false);
            fairyIcon.SetActive(false);
            magicIcon.SetActive(false);
            upgradeText.SetActive(true);
        }

        else if (TowerManager.Instance.selectedTower.TowerName == "Sword Tower")
        {
            swordIcon.SetActive(true);

            defaultIcon.SetActive(false);
            normalIcon.SetActive(false);
            fairyIcon.SetActive(false);
            magicIcon.SetActive(false);
            upgradeText.SetActive(false);
        }

        else if (TowerManager.Instance.selectedTower.TowerName == "Magic Tower")
        {
            magicIcon.SetActive(true);

            defaultIcon.SetActive(false);
            normalIcon.SetActive(false);
            fairyIcon.SetActive(false);
            swordIcon.SetActive(false);
            upgradeText.SetActive(false);
        }

        else if (TowerManager.Instance.selectedTower.TowerName == "Fairy Tower")
        {
            fairyIcon.SetActive(true);

            defaultIcon.SetActive(false);
            normalIcon.SetActive(false);
            swordIcon.SetActive(false);
            magicIcon.SetActive(false);
            upgradeText.SetActive(false);
        }

        else if (TowerManager.Instance.selectedTower == null)
        {
            defaultIcon.SetActive(true);

            swordIcon.SetActive(false);
            normalIcon.SetActive(false);
            fairyIcon.SetActive(false);
            magicIcon.SetActive(false);
            upgradeText.SetActive(false);
        }
    }
}
