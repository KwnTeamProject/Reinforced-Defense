using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class MainSystem : MonoBehaviour
{
    public static MainSystem mainSystemInstance;

    [Header("�������� ����")]
    public int stageInfo;
    public float remainingTime;
    public float stageDuration;
    [SerializeField] TMP_Text timerText;

    [Header("���� ����")]
    public int currentEnemyCount;
    public int maxEnemyCount;
    [SerializeField] TMP_Text enemyText;

    [Header("��ٱ��� ����")]
    public bool isStageStarted = false;
    public bool isPaused = false;
    public bool isGameEnd = false;

    public AudioSource BGM;
    public GameObject resultWindow;
    private ResultWindow resultWindowComponent;

    private void Awake()
    {
        MainSystem.mainSystemInstance = this;
    }

    private void Start()
    {
        resultWindowComponent = resultWindow.GetComponent<ResultWindow>();
    }

    private void Update()
    {
        GameEnd();
        BGMController();

        UpdateEnemyCountText();

        if (!isStageStarted || isPaused || isGameEnd) return;

        remainingTime -= Time.deltaTime;
        UpdateTimerText();
    }

    public void BGMController()
    {
        if(isGameEnd)
        {
            BGM.Pause();
        }
    }

    public void StartStage()
    {
        if (isStageStarted) return;

        isStageStarted = true;
        isPaused = false;
        TimerReset();
    }

    void TimerReset()
    {
        remainingTime = stageDuration;
    }

    public void TakePause()
    {
        isPaused = !isPaused;
    }

    public void GameEnd()
    {
        if (currentEnemyCount == maxEnemyCount)
        {
            resultWindow.SetActive(true);
            resultWindowComponent.PopupWindow(false, remainingTime, 0, 0);
            isGameEnd = true;
        }
        else if(remainingTime <= 0)
        {
            resultWindow.SetActive(true);
            resultWindowComponent.PopupWindow(true, remainingTime, 0, 0);
            isGameEnd = true;
        }
    }

    private void UpdateTimerText()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        float secondsWithFraction = remainingTime % 60f;

        timerText.text = $"{minutes:D2}:{secondsWithFraction:00.00}";
    }

    public void UpdateEnemyCountText()
    {
        if (enemyText == null) return;

        enemyText.text = $"{currentEnemyCount}/{maxEnemyCount}";
    }

    public float GetElapsedTime()
    {
        return remainingTime;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // Unity �÷��̾ �����ϴ� ���� �ڵ�
#endif
    }
}
