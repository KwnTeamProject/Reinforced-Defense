using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class ClickToLoadScene : MonoBehaviour
{
    public Image fadeImage;           // 전체 화면 검정 이미지 (UI Image)
    public float fadeDuration = 1f;   // 페이드 시간 (초)
    private bool isTransitioning = false; // 중복 입력 방지용

    // by 이상협
    public StartCutSceneUI StartCutScene;

    bool playingCutScene = false;

    // = = = = =

    void Start()
    {
        if (fadeImage == null)
        {
            fadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
        }

        if (fadeImage.gameObject.activeSelf == false)
        {
            fadeImage.gameObject.SetActive(true);
        }

        // by 이상협
        if (StartCutScene == null)
        {
            StartCutScene = GameObject.Find("StartCutScene").GetComponent<StartCutSceneUI>();
        }
        // = = = = =
        
        // 씬 시작 시 페이드 인
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        if (isTransitioning) return;

        // 마우스 클릭 또는 터치 감지
        if (Input.GetMouseButtonDown(0) ||
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // by 이상협 : 컷씬 재생
            if (playingCutScene)
            {
                Debug.Log("Clicked on Playing CutScene");
                StartCutScene.SkipNowOne();
                return;
            }
            
            if (UserDataManager.UserDataManagerInstance.firstPlay)
            {
                Debug.Log("Play CutScene...");
                playingCutScene = true;
                StartCutScene.PlayCutScene(OnCutsceneComplete);
                return;
            }
            // = = = = = = = = = = =


            StartCoroutine(FadeOutAndLoadNextScene());
        }
    }
    private void OnCutsceneComplete()
    {
        playingCutScene = false;
        StartCoroutine(FadeOutAndLoadNextScene());
    }

    // 페이드 인 (어두운 → 밝은)
    IEnumerator FadeIn()
    {
        float t = fadeDuration;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float alpha = t / fadeDuration;
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(0f);
    }

    IEnumerator FadeOutAndLoadNextScene()
    {
        isTransitioning = true;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = t / fadeDuration;
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(1f);

        // 씬 전환
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("더 이상 전환할 씬이 없습니다.");
        }
    }

    void SetAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = Mathf.Clamp01(alpha);
            fadeImage.color = c;
        }
    }
}
