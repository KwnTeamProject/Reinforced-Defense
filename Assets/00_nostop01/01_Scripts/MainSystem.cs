using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainSystem : MonoBehaviour
{
    [Header("스테이지 정보")]
    [SerializeField] int stageInfo;
    [SerializeField] float elapsedTime;
    [SerializeField] TMP_Text timerText;

    [Header("웨이브 정보")]
    [SerializeField] int enemyCount;
    [SerializeField] int waveCount;
    [SerializeField] int maxWaveCount;

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

        elapsedTime += Time.deltaTime;
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
        elapsedTime = 0;
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

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        float secondsWithFraction = elapsedTime % 60f;

        timerText.text = $"{minutes:D2}:{secondsWithFraction:00.00}";
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
