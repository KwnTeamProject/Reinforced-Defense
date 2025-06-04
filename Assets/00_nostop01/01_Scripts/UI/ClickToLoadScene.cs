using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class ClickToLoadScene : MonoBehaviour
{
    public Image fadeImage;           // 전체 화면 검정 이미지 (UI Image)
    public float fadeDuration = 1f;   // 페이드 시간 (초)
    private bool isTransitioning = false; // 중복 입력 방지용

    void Start()
    {
        if (fadeImage == null)
            return;

        else if (fadeImage.gameObject.activeSelf == false)
            fadeImage.gameObject.SetActive(true);

        else
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
            StartCoroutine(FadeOutAndLoadNextScene());
        }
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
