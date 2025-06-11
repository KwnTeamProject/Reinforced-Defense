using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResultWindow : MonoBehaviour
{
    [SerializeField] Text TitleText;
    [SerializeField] Text RecordText;
    [SerializeField] Text GoldText;
    [SerializeField] Text KillText;

    public GameObject winningAudio;
    public GameObject defeatAudio;

    public void PopupWindow(bool isWin, float record, int gold, int kill)
    {
        if(isWin)
        {
            winningAudio.SetActive(true);


            TitleText.text = "VICTORY";
            TitleText.color = new Color(1, 1, 0);
           
            string Rtxt = "";

            int Min = ((int)record) / 60;
            Rtxt += ((Min < 10) ? "0" + Min.ToString() : Min.ToString()) + ":";
            int Sec = ((int)record) % 60;
            Rtxt += ((Sec < 10) ? "0" + Sec.ToString() : Sec.ToString()) + ".";
            int MSec = (int)((record - (int)record) * 100);
            Rtxt += ((MSec < 10) ? "0" + MSec.ToString() : MSec.ToString());

            RecordText.text = Rtxt;

        }

        else if(!isWin)
        {
            defeatAudio.SetActive(true);

            TitleText.text = "DEFEAT";
            TitleText.color = new Color(0.43f, 0, 0.55f);

            RecordText.text = "Ŭ���� ����";
        }

        GoldText.text = gold.ToString();

        KillText.text = kill.ToString() + "/" + "999"/*������ ��������*/;





    }



}
