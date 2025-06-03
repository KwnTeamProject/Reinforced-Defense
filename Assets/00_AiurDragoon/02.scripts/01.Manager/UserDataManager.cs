using System.Globalization;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{

    public static UserDataManager UserDataManagerInstance { get; private set; }



    //���� ������
    public string Name { get; private set; } = "";
    public int Gold { get; private set; } = 0;
    public float HighRecord { get; private set; } = -1f;


    private void Awake()
    {
        UserDataManagerInstance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(UserDataManagerInstance);
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
