using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class ClickToLoadScene : MonoBehaviour
{
    public Image fadeImage;           // ��ü ȭ�� ���� �̹��� (UI Image)
    public float fadeDuration = 1f;   // ���̵� �ð� (��)
    private bool isTransitioning = false; // �ߺ� �Է� ������

    // by �̻���
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

        // by �̻���
        if (StartCutScene == null)
        {
            StartCutScene = GameObject.Find("StartCutScene").GetComponent<StartCutSceneUI>();
        }
        // = = = = =
        
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
            // by �̻��� : �ƾ� ���
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
