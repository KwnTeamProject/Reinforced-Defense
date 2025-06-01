using UnityEditor;
using UnityEngine;

public class GameExit : MonoBehaviour
{
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // Unity �÷��̾ �����ϴ� ���� �ڵ�
#endif
    }
}
