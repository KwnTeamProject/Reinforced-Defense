using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainSystem : MonoBehaviour
{
    [Header("스테이지 정보")]
    [SerializeField] int stageInfo;
    [SerializeField] float remainingTime;
    [SerializeField] float stageDuration;
    [SerializeField] TMP_Text timerText;

    [Header("몬스터 정보")]
    [SerializeField] int currentEnemyCount;
    [SerializeField] int maxEnemyCount;
    [SerializeField] TMP_Text enemyText;

    [Header("잡다구리 변수")]
    public bool isStageStarted = false;
    public bool isPaused = false;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        TakePause();

        if (!isStageStarted || isPaused) return;

        remainingTime -= Time.deltaTime;
        UpdateTimerText();
        UpdateEnemyCountText();
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

    public void Pause()
    {
        isPaused = !isPaused;
    }

    public void TakePause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void UpdateTimerText()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        float secondsWithFraction = remainingTime % 60f;

        timerText.text = $"{minutes:D2}:{secondsWithFraction:00.00}";
    }
    private void UpdateEnemyCountText()
    {
        if (enemyText == null) return;

        enemyText.text = $"{currentEnemyCount}/{maxEnemyCount}";
    }

    public float GetElapsedTime()
    {
        return remainingTime;
    }
}
