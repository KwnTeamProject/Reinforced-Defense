using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using System;

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

    [SerializeField] float TypingDelay = 0.05f;
    [SerializeField] float TypingAfterDelay = 1.5f;


    float Ftime;
    int NowSceneNum;

    bool cutScenePlaying = false;

    bool isTextTyping = false;

    Action FinishCutSceneAction = null;


    Coroutine typingCoroutine;



    private void Start()
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    //private void OnEnable()
    //{
    //    PlayCutScene();
    //}

    public void PlayCutScene(Action onComplete)
    {
        FinishCutSceneAction = onComplete;
        StartCoroutine(StartCutScene());
    }

    public void SkipNowOne()
    {
        if (isTextTyping)
        {
            StopCoroutine(typingCoroutine);
            SceneText.text = SceneTextArray[NowSceneNum];
            isTextTyping = false;
        }
        else if(!isTextTyping)
        {
            Ftime = TypingAfterDelay;
        }


    }

    // 문장 출력 애니메이션 = = = = = = = = = =

    public void StartTyping(string text, float delay)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(Typing(text, delay));
    }

    IEnumerator Typing(string text, float delay)
    {

        isTextTyping = true;

        string cur = "";
        foreach (char c in text)
        {
            if (c >= 0xAC00 && c <= 0xD7A3)
            {
                int code = c - 0xAC00;
                int cho = code / (21 * 28);
                int jung = (code % (21 * 28)) / 28;
                int jong = code % 28;

                cur += "ㄱㄲㄴㄷㄸㄹㅁㅂㅃㅅㅆㅇㅈㅉㅊㅋㅌㅍㅎ"[cho];
                SceneText.text = cur;
                yield return new WaitForSeconds(delay);

                cur = cur.Substring(0, cur.Length - 1) +
                      (char)(0xAC00 + cho * 21 * 28 + jung * 28);
                SceneText.text = cur;
                yield return new WaitForSeconds(delay);

                if (jong != 0)
                {
                    cur = cur.Substring(0, cur.Length - 1) +
                          (char)(0xAC00 + cho * 21 * 28 + jung * 28 + jong);
                    SceneText.text = cur;
                    yield return new WaitForSeconds(delay);
                }
            }
            else
            {
                cur += c;
                SceneText.text = cur;
                yield return new WaitForSeconds(delay);
            }
        }

        isTextTyping = false;

    }
    // = = = = = = = = = = 문장 출력 애니메이션


    public void Update()
    {

        if(!cutScenePlaying)
        {
            return;
        }

        if(!isTextTyping)
        {

            Ftime += Time.deltaTime;

            if(Ftime >= TypingAfterDelay)
            {

                Ftime = 0f;
                NowSceneNum++;
                if (NowSceneNum >= CutSceneCount)
                {
                    StopCutScene();

                    return;
                }

                Debug.Log("Change CutScene : " + NowSceneNum.ToString());
                SceneImage.sprite = SceneImageArray[NowSceneNum];   // 이미지 변경

                StartTyping(SceneTextArray[NowSceneNum], TypingDelay);   // 텍스트 타이핑
            }

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

        NowSceneNum = -1;
        SceneImage.sprite = SceneImageArray[0];


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
        NowSceneNum = -1;
        isTextTyping = false;

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

        /*
        t = fadeTime;
        while (t > 0)
        {
            t -= Time.deltaTime;
            float alpha = t / fadeTime;
            SetShadowAlpha(alpha);
            yield return null;
        }
        SetShadowAlpha(0f);
        */
        
        UserDataManager.UserDataManagerInstance.firstPlay = false;

        FinishCutSceneAction?.Invoke();

        //gameObject.SetActive(false);
    }


    void SetShadowAlpha(float alpha)
    {
        Color c = ShadowImg.color;
        c.a = Mathf.Clamp01(alpha);
        //Debug.Log("Alpha :" + c.a.ToString());
        ShadowImg.color = c;
    }








}
