using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UserDataManager : MonoBehaviour
{
    static volatile UserDataManager _uniqueInstance;
    static volatile GameObject _uniqueObject;


    public UserDataManager()
    {
        _uniqueInstance = null;
        _uniqueObject = null;
    }

    public static UserDataManager UserDataManagerInstance
    {
        get
        {
            if (_uniqueInstance == null)
            {
                lock (typeof(UserDataManager))
                {
                    if (_uniqueInstance == null && _uniqueObject == null)
                    {
                        _uniqueObject = new GameObject(typeof(UserDataManager).Name, typeof(UserDataManager));
                        _uniqueInstance = _uniqueObject.GetComponent<UserDataManager>();
                        _uniqueInstance.Init();
                    }
                }
            }
            return _uniqueInstance;
        }
    }

    protected virtual void Init()
    {
        DontDestroyOnLoad(gameObject);

        LoadData();

    }


    //���� ������
    public string Name { get; private set; } = "Player";
    public int Gold { get; private set; } = 0;
    public float HighRecord { get; private set; } = -1f;

    public float Volume { get; set; }
    public float Brightness { get; set; }

    public bool firstPlay { get; set; } = true;

    private void Start()
    {

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

    // Setting

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

        //��Ÿ ���� ������

        //  ù �÷��� ����
        PlayerPrefs.SetInt("FirstPlay", (firstPlay) ? 1 : 0);

        //  ������
        PlayerPrefs.SetFloat("Volume", Volume);
        PlayerPrefs.SetFloat("Bright", Brightness);


    }

    void LoadData()
    {
        //������ �˻�
        if(!PlayerPrefs.HasKey("Name"))
        {
            Debug.Log("No Data Detected...");

            Name = "User0000";
            Gold = 0;
            HighRecord = -1;

            firstPlay = true;

            Volume = 0.5f;
            Brightness = 1.0f;
            //Debug.LogFormat("Failed Load Settings : {0}, {1}", Volume, Brightness);

            return;
        }

        //�̸�
        Name = PlayerPrefs.GetString("Name");

        //���
        Gold = PlayerPrefs.GetInt("Gold");

        //�ְ���
        HighRecord = PlayerPrefs.GetFloat("HighRecord");

        //���� �÷��� ����
        if(PlayerPrefs.GetInt("FirstPlay") == 1)
        {
            firstPlay = true;
        }
        else
        {
            firstPlay= false;
        }
    }



}
