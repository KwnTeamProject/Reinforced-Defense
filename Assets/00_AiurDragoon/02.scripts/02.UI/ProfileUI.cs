using UnityEngine;
using UnityEngine.UI;


public class ProfileUI : MonoBehaviour
{

    [SerializeField] Text PlayerNameText;
    [SerializeField] Text HighRecordText;

    void SetUI()
    {
        // Name Card
        PlayerNameText.text = UserDataManager.UserDataManagerInstance.Name;

        // Record Board
        float record = UserDataManager.UserDataManagerInstance.HighRecord;
        string txt = "스테이지 최고기록\n";
        int Min = ((int)record) / 60;
        txt += ((Min < 10) ? "0" + Min.ToString() : Min.ToString()) + ":";
        int Sec = ((int)record) % 60;
        txt += ((Sec < 10) ? "0" + Sec.ToString() : Sec.ToString()) + ":";
        int MSec = (int)((record - (int)record) * 100);
        txt += ((MSec < 10) ? "0" + MSec.ToString() : MSec.ToString()) + ":";

        HighRecordText.text = txt;

    }

    public void SaveChangedName(string CName)
    {
        UserDataManager.UserDataManagerInstance.ChangeName(CName);
    }




}
