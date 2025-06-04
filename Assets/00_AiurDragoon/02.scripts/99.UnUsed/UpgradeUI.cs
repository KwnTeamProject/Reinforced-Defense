using UnityEngine;
using UnityEngine.UI;
using Defines;


public class UpgradeUI : MonoBehaviour
{

    [SerializeField] GameObject ScrollViewContent;

    GameObject[] TowerBlocks;

    int TowerCount = 0;


    private void Start()
    {
        



    }

    public void InitContents()
    {

        TowerBlocks = new GameObject[TowerCount];

        Vector3 SVContentSize = ScrollViewContent.GetComponent<RectTransform>().localScale;
        ScrollViewContent.GetComponent<RectTransform>().localScale = new Vector3(SVContentSize.x, 230 * TowerCount + 60, 0);



    }
    



}
