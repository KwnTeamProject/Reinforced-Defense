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

    private void Awake()
    {
        MainSystem.mainSystemInstance = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        GameEnd();
        TakePause();

        UpdateEnemyCountText();

        if (!isStageStarted || isPaused) return;

        remainingTime -= Time.deltaTime;
        UpdateTimerText();
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }
    }

    public void GameEnd()
    {
        if (currentEnemyCount == maxEnemyCount)
            Exit();
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
