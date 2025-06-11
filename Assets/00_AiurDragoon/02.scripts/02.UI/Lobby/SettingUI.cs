using NUnit.Framework.Internal.Commands;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{

    [SerializeField] Slider volSlider;
    [SerializeField] Slider briSlider;


    public float AudVolume { get; private set; }
    public float Brightness { get; private set; }

    float TmpVolume = 0.5f, TmpBrightness = 1.0f;

    private void Start()
    {
        AudVolume = UserDataManager.UserDataManagerInstance.Volume;
        Brightness = UserDataManager.UserDataManagerInstance.Brightness;

        volSlider.value = AudVolume;
        briSlider.value = Brightness;
    }


    public void ChangeTmpVolume()
    {
        TmpVolume = volSlider.value;

        GameObject.Find("BGMObject").GetComponent<AudioSource>().volume = TmpVolume;
    }
    public void ChangeTmpBright()
    {
        TmpBrightness = briSlider.value;

        Image IMG = GameObject.Find("BrightnessImage").GetComponent<Image>();
        float alpha = Mathf.Clamp01(1.0f - TmpBrightness - 0.1f);

        Color c = IMG.color;
        //Debug.LogFormat("Color:{0}", c);
        c.a = alpha;
        //Debug.LogFormat("alpha:{0}", c.a);
        IMG.color = c;

    }
    public void SaveSettings()
    {
        AudVolume = TmpVolume;
        Brightness = TmpBrightness;

        TmpVolume = AudVolume;
        TmpBrightness = Brightness;

        UserDataManager.UserDataManagerInstance.Volume = AudVolume;
        UserDataManager.UserDataManagerInstance.Brightness = Brightness;

        //Debug.LogFormat("Saved Settings : {0}, {1}", UserDataManager.UserDataManagerInstance.Volume, UserDataManager.UserDataManagerInstance.Brightness);
        //Debug.Log("= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =");

    }


    public void UndoSettings()
    {
        //Debug.LogFormat("UserData Settings : {0}, {1}", AudVolume, Brightness);
        //Debug.LogFormat("Apply Settings : {0}, {1}", TmpVolume, Brightness);

        TmpVolume = AudVolume;
        TmpBrightness = Brightness;

        //Debug.Log("Undoing Temp Setting");
        //Debug.LogFormat("Apply Settings : {0}, {1}", TmpVolume, Brightness);

        volSlider.value = AudVolume;
        briSlider.value = Brightness;



    }




}
