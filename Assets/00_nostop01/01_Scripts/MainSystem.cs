using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class MainSystem : MonoBehaviour
{
    public static MainSystem mainSystemInstance;

    [Header("스테이지 정보")]
    public int stageInfo;
    public float maxTime = 360;
    public float remainingTime;
    public float stageDuration;
    public float byProductCount;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text productText;

    [Header("몬스터 정보")]
    public int currentEnemyCount;
    public int maxEnemyCount;
    [SerializeField] TMP_Text enemyText;

    [Header("잡다구리 변수")]
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
        remainingTime = maxTime;
        byProductCount = 10;
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
        GameLevelUp();
    }

    public void GameLevelUp()
    {
        if(remainingTime < 300)
        {
            EnemySpawner.Instance.guardSpawnTimer = 3;
            EnemySpawner.Instance.goblinSpawnTimer = 1.5f;
        }

        if(remainingTime < 240)
        {
            EnemySpawner.Instance.guardSpawnTimer = 2;
            EnemySpawner.Instance.goblinSpawnTimer = 1;
        }
    }

    public void PlusProduct(float productCount)
    {
        byProductCount += productCount;
        UpdateProductText();
    }

    public void MinusProduct(float productCount)
    {
        byProductCount -= productCount;
        UpdateProductText();
    }

    private void UpdateProductText()
    {
        if (productText == null) return;

        if (byProductCount <= 0)
            productText.text = ": 0";
        else
            productText.text = ": " + byProductCount.ToString();
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
            resultWindowComponent.PopupWindow(false, maxTime, 0, 0);
            isGameEnd = true;
        }
        else if(remainingTime <= 0)
        {
            resultWindow.SetActive(true);
            resultWindowComponent.PopupWindow(true, maxTime, 0, 0);
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
        Application.Quit(); // Unity 플레이어를 종료하는 원본 코드
#endif
    }
}
