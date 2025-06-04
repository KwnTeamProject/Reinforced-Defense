using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonToLoadScene : MonoBehaviour
{
    public Image fadeImage;           // 검정 페이드 이미지 (UI Image)
    public float fadeDuration = 1f;   // 페이드 시간
    private bool isTransitioning = false;

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

        // 씬 시작 시 페이드 인
        StartCoroutine(FadeIn());
    }

    // 버튼에서 이 함수를 호출하도록 연결
    public void OnFadeButtonClicked(string sceneNameToLoad)
    {
        if (!isTransitioning)
        {
            StartCoroutine(FadeOutAndLoadSceneByName(sceneNameToLoad));
        }
    }

    // 화면 밝아지는 효과
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

    // 어두워지며 지정된 씬 이름으로 전환
    IEnumerator FadeOutAndLoadSceneByName(string sceneName)
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

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("로드할 씬 이름이 비어 있습니다.");
        }
    }

    // 이미지 알파 값 조절
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
