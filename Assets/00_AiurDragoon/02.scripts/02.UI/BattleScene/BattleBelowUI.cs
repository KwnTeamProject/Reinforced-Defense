using UnityEngine;

public class BattleBelowUI : MonoBehaviour
{

    [SerializeField] int TowerCount = 10;
    [SerializeField] GameObject InfoBlockPref;
    [SerializeField] GameObject Contents;


    private void Update()
    {
        SetInit();
    }


    public void SetInit()
    {
        RectTransform RT = Contents.GetComponent<RectTransform>();
        RT.sizeDelta = new Vector2(RT.sizeDelta.x, 14 + (87 * TowerCount));

        for (int i = 0; i < TowerCount; i++)
        {
            Instantiate(InfoBlockPref, Contents.transform);
        }

    }

    

}
