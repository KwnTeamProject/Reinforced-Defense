    using UnityEngine;

public class BattleBelowUI : MonoBehaviour
{

    [SerializeField] int TowerCount = 10;
    [SerializeField] GameObject InfoBlockPref;
    [SerializeField] GameObject Contents;


    public GameObject[] TowerBlockArray { get; set; }


    private void Start()
    {
        SetInit();
    }


    public void SetInit()
    {
        RectTransform RT = Contents.GetComponent<RectTransform>();
        RT.sizeDelta = new Vector2(RT.sizeDelta.x, 30 + (155 * TowerCount));

        TowerBlockArray = new GameObject[TowerCount];

        for (int i = 0; i < TowerCount; i++)
        {
            GameObject go = Instantiate(InfoBlockPref, Contents.transform);
            go.GetComponent<TowerUpgradeBlock>().SetInit("Dummy Name", null);

            TowerBlockArray[i] = go;

        }

    }

}
