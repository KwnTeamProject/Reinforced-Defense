using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class StartCutSceneUI : MonoBehaviour
{
    [SerializeField] Image ShadowImg;

    [SerializeField] Image SceneImage;
    [SerializeField] Text SceneText;

    [SerializeField] float fadeTime = 0.5f;

    const int CutSceneCount = 4;
    [SerializeField] Sprite[] SceneImageArray;
    [SerializeField] string[] SceneTextArray;
    [SerializeField] float[] SceneTime;

    float Ftime;
    int NowSceneNum;

    bool cutScenePlaying = false;

    private void Start()
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        PlayCutScene();
    }

    void PlayCutScene()
    {
        StartCoroutine(StartCutScene());

    }

    public void Update()
    {

        if(!cutScenePlaying)
        {
            return;
        }

        Ftime += Time.deltaTime;
        //Debug.Log(Ftime);

        if(Ftime > SceneTime[NowSceneNum])
        {
            ++NowSceneNum;

            if (NowSceneNum >= CutSceneCount)
            {
                StopCutScene();
                return;
            }

            SceneImage.sprite = SceneImageArray[NowSceneNum];
            SceneText.text = SceneTextArray[NowSceneNum];
            
            Ftime = 0f;

        }

    }

    void StopCutScene()
    {
        //Debug.Log("Finishing CutScene");

        cutScenePlaying = false;

        StartCoroutine(FinishCutScene());

    }


    IEnumerator StartCutScene()
    {
        //Debug.Log("StartCutScene");

        ShadowImg.gameObject.SetActive(true);
        
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float alpha = t / fadeTime;
            SetShadowAlpha(alpha);
            yield return null;
        }
        SetShadowAlpha(1f);

        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        SceneImage.sprite = SceneImageArray[0];
        SceneText.text = SceneTextArray[0];

        t = fadeTime;
        while (t > 0)
        {
            t -= Time.deltaTime;
            float alpha = t / fadeTime;
            SetShadowAlpha(alpha);
            yield return null;
        }
        SetShadowAlpha(0f);

        cutScenePlaying = true;

        Ftime = 0f;
        NowSceneNum = 0;

    }

    IEnumerator FinishCutScene()
    {

        //Debug.Log("FinishCutScene");

        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float alpha = t / fadeTime;
            SetShadowAlpha(alpha);
            yield return null;
        }
        SetShadowAlpha(1f);

        for (int i = 0; i < transform.childCount; ++i)
        {

            GameObject go = transform.GetChild(i).gameObject;
            if (go.name == "FadeShadow")
            {
                continue;
            }
            go.SetActive(false);
        }

        SceneImage.sprite = SceneImageArray[0];
        SceneText.text = SceneTextArray[0];

        t = fadeTime;
        while (t > 0)
        {
            t -= Time.deltaTime;
            float alpha = t / fadeTime;
            SetShadowAlpha(alpha);
            yield return null;
        }
        SetShadowAlpha(0f);

        UserDataManager.UserDataManagerInstance.firstPlay = false;

        gameObject.SetActive(false);
    }


    void SetShadowAlpha(float alpha)
    {
        Color c = ShadowImg.color;
        c.a = Mathf.Clamp01(alpha);
        //Debug.Log("Alpha :" + c.a.ToString());
        ShadowImg.color = c;
    }








}
