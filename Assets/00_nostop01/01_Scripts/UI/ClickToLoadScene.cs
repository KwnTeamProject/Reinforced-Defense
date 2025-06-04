using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class ClickToLoadScene : MonoBehaviour
{
    public Image fadeImage;           // ��ü ȭ�� ���� �̹��� (UI Image)
    public float fadeDuration = 1f;   // ���̵� �ð� (��)
    private bool isTransitioning = false; // �ߺ� �Է� ������

    void Start()
    {
        if (fadeImage == null)
            return;

        else if (fadeImage.gameObject.activeSelf == false)
            fadeImage.gameObject.SetActive(true);

        else
            // �� ���� �� ���̵� ��
            StartCoroutine(FadeIn());
    }

    void Update()
    {
        if (isTransitioning) return;

        // ���콺 Ŭ�� �Ǵ� ��ġ ����
        if (Input.GetMouseButtonDown(0) ||
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            StartCoroutine(FadeOutAndLoadNextScene());
        }
    }

    // ���̵� �� (��ο� �� ����)
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

        // �� ��ȯ
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("�� �̻� ��ȯ�� ���� �����ϴ�.");
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
