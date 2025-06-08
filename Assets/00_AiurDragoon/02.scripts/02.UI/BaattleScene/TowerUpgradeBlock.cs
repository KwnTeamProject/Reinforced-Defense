using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeBlock : MonoBehaviour
{


    [SerializeField] Image IconImage;
    [SerializeField] Text NameText;
    [SerializeField] Text LevelText;
    [SerializeField] Text CostText;


    int Level = 1;
    int Cost = 500;
    string Name = "Tower Name";
    
    
    
    public void SetInit(string name, Sprite Icon)
    {
        NameText.text = name;
        LevelText.text = "Lv." + Level.ToString();
        CostText.text = Cost.ToString();

        IconImage.sprite = Icon;

    }

    public void LevelUp()
    {

        Level++;
        LevelText.text = "Lv." + Level.ToString();

        Cost =  (int)(Cost * 1.5);
        CostText.text = Cost.ToString();

    }









}
