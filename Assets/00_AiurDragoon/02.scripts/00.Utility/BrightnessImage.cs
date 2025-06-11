using UnityEngine;
using UnityEngine.UI;

public class BrightnessImage : MonoBehaviour
{

    void Start()
    {
        Image IMG = gameObject.GetComponent<Image>();
        float alpha = Mathf.Clamp01(UserDataManager.UserDataManagerInstance.Brightness - 0.1f);

        Color c = IMG.color;
        Debug.LogFormat("Color:{0}", c);
        c.a = alpha;
        Debug.LogFormat("alpha:{0}", c.a);
        IMG.color = c;
    }

    private void OnEnable()
    {
        Image IMG = gameObject.GetComponent<Image>();
        float alpha = Mathf.Clamp01(UserDataManager.UserDataManagerInstance.Brightness - 0.1f);

        Color c = IMG.color;
        Debug.LogFormat("Color:{0}", c);
        c.a = alpha;
        Debug.LogFormat("alpha:{0}", c.a);
        IMG.color = c;
    }


}
