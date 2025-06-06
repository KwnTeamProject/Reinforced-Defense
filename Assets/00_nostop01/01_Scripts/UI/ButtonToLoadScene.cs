using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonToLoadScene : MonoBehaviour
{
    public Image fadeImage;           // ���� ���̵� �̹��� (UI Image)
    public float fadeDuration = 1f;   // ���̵� �ð�
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

        // �� ���� �� ���̵� ��
        StartCoroutine(FadeIn());
    }

    // ��ư���� �� �Լ��� ȣ���ϵ��� ����
    public void OnFadeButtonClicked(string sceneNameToLoad)
    {
        if (!isTransitioning)
        {
            StartCoroutine(FadeOutAndLoadSceneByName(sceneNameToLoad));
        }
    }

    // ȭ�� ������� ȿ��
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

    // ��ο����� ������ �� �̸����� ��ȯ
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
            Debug.LogWarning("�ε��� �� �̸��� ��� �ֽ��ϴ�.");
        }
    }

    // �̹��� ���� �� ����
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
