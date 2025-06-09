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
    [SerializeField] TMP_Text SceneText;

    [SerializeField] float fadeTime = 0.5f;

    const int CutSceneCount = 4;
    [SerializeField] Sprite[] SceneImageArray;
    [SerializeField] string[] SceneTextArray;
    [SerializeField] float[] SceneTime;

    float Ftime;
    int NowSceneNum;

    private void Start()
    {
        
    }


    public void StartCutScene()
    {

        SetShadowAlpha(1.0f);
        SceneImage.sprite = SceneImageArray[0];


        Ftime = 0f;
        NowSceneNum = 0;

        StartCoroutine(FadeIn());

    }

    public void Update()
    {

        if(NowSceneNum > CutSceneCount - 1)
        {   
            gameObject.SetActive(false);
            return;
        }

        Ftime += Time.deltaTime;

        if ( Ftime > SceneTime[NowSceneNum])
        {
            StartCoroutine(FadeOut());
            
            if(Ftime > SceneTime[NowSceneNum] + fadeTime)
            {
                SceneImage.sprite = SceneImageArray[NowSceneNum];
                SceneText.text = SceneTextArray[NowSceneNum];
                StartCoroutine(FadeIn());

                NowSceneNum++;
                Ftime = 0f;
            }

            

        }
        
    }


    // 알파값 0으로
    IEnumerator FadeIn()
    {
        Debug.Log("페이드 인");
        float t = fadeTime;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float alpha = t / fadeTime;
            SetShadowAlpha(alpha);
            yield return null;
        }
        SetShadowAlpha(0f);
    }

    //알파값 1로
    IEnumerator FadeOut()
    {
        Debug.Log("페이드 아웃");
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float alpha = t / fadeTime;
            SetShadowAlpha(alpha);
            yield return null;
        }
        SetShadowAlpha(1f);
    }

    void SetShadowAlpha(float alpha)
    {
        Color c = ShadowImg.color;
        c.a = Mathf.Clamp01(alpha);
        ShadowImg.color = c;
    }








}
