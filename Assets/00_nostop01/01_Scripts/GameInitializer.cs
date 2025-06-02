using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60; // 여기서 프레임 제한 설정
    }
}
