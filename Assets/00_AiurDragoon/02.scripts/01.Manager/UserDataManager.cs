using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UserDataManager : MonoBehaviour
{

    public static UserDataManager UserDataManagerInstance { get; private set; }


    //�ӽ�
    [SerializeField] Slider volSlider;
    [SerializeField] Slider briSlider;




    //���� ������
    public string Name { get; private set; } = "Player";
    public int Gold { get; private set; } = 0;
    public float HighRecord { get; private set; } = -1f;

    public float Volume { get; private set; }
    public float Brightness { get; private set; }

    float TmpVolume = 0.5f, TmpBrightness = 0.5f;

    public bool firstPlay { get; set; } = true;

    private void Awake()
    {
        UserDataManagerInstance = this;
        
    }

    private void Start()
    {
        DontDestroyOnLoad(UserDataManagerInstance);

        LoadData();

        GameObject MainUIs = GameObject.Find("MainUIs");
        MainMenuUI MMUIs = MainUIs.GetComponent<MainMenuUI>();

        MMUIs.SetContents(Name, Gold);

    }


    // Data Change

    public void ChangeName(string data)
    {
        Name = data;
    }

    public void AddGold(int value)
    {
        Gold += value;
    }

    public void UpdateHighRecord(float Record)
    {
        HighRecord = Record;
    }

    //������
    public void ChangeTmpVolume()
    {
        TmpVolume = volSlider.value;
    }
    public void ChangeTmpBright()
    {
        TmpBrightness = briSlider.value;
    }
    public void SaveSettings()
    {
        Volume = TmpVolume;
        Brightness = TmpBrightness;
    }
    public void UndoSettings()
    {
        TmpVolume = Volume;
        TmpBrightness = Brightness;

        volSlider.value = Volume;
        briSlider.value = Volume;

    }




    // Save & Load
    void SaveData()
    {
        //�̸�
        PlayerPrefs.SetString("Name", Name);
        
        //���
        PlayerPrefs.SetInt("Gold", Gold);

        //�ְ���
        #region �ְ���

        PlayerPrefs.SetFloat("HighRecord", HighRecord);

        #endregion �ְ���

    }

    void LoadData()
    {
        if(!PlayerPrefs.HasKey("Name"))
        {
            Debug.Log("No Data Detected...");
            return;
        }
        Name = PlayerPrefs.GetString("Name");

        Gold = PlayerPrefs.GetInt("Gold");

        HighRecord = PlayerPrefs.GetFloat("HighRecord");

    }



}
