using UnityEngine;
using Defines;
using UnityEngine.UI;


public class MainSceneUIMaster : MonoBehaviour
{

    GameObject MainUIObj;
    GameObject UpgradeUIObj;
    GameObject ResearchUIObj;


    void Start()
    {

        MainUIObj = transform.Find("MainUIs").gameObject;
        UpgradeUIObj = transform.Find("UpgradeUIs").gameObject;
        ResearchUIObj = transform.Find("ResearchUIs").gameObject;

    }

    
    public void ChangeUI(MainSceneUIName type)
    {
                MainUIObj.SetActive(MainSceneUIName.MainUI == type);
                UpgradeUIObj.SetActive(MainSceneUIName.UpgradeUI == type);
                ResearchUIObj.SetActive(MainSceneUIName.ResearchUI == type);

        return;
    }




}
