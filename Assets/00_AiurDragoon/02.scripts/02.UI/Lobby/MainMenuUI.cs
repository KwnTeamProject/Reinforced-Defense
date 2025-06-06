using UnityEngine;
using UnityEngine.UI;


public class MainMenuUI : MonoBehaviour
{

    // Content Objects

    [SerializeField] Text UserNameText;
    [SerializeField] Text GoldText;
    [SerializeField] Image BGImage;

    public void SetContents(string userName, int gold)
    {
        UserNameText.text = userName;
        GoldText.text = gold.ToString();

        // BGImage = 

    }






}
