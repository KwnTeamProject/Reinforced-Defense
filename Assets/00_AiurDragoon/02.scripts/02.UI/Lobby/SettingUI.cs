using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{

    [SerializeField] Slider volSlider;
    [SerializeField] Slider briSlider;


    public float Volume { get; private set; }
    public float Brightness { get; private set; }

    float TmpVolume = 0.5f, TmpBrightness = 0.0f;

    private void Start()
    {
        volSlider.value = UserDataManager.UserDataManagerInstance.Volume;
        briSlider.value = 1.0f - UserDataManager.UserDataManagerInstance.Brightness;
    }


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

        UserDataManager.UserDataManagerInstance.Volume = Volume;
        UserDataManager.UserDataManagerInstance.Brightness = 1.0f - Brightness;

        GameObject.Find("BGMObject").GetComponent<AudioSource>().volume = Volume;


        Image IMG = GameObject.Find("BrightnessImage").GetComponent<Image>();
        float alpha = Mathf.Clamp01(UserDataManager.UserDataManagerInstance.Brightness - 0.1f);

        Color c = IMG.color;
        Debug.LogFormat("Color:{0}", c);
        c.a = alpha;
        Debug.LogFormat("alpha:{0}", c.a);
        IMG.color = c;

    }
    public void UndoSettings()
    {
        TmpVolume = Volume;
        TmpBrightness = Brightness;

        volSlider.value = Volume;
        briSlider.value = Volume;

    }




}
