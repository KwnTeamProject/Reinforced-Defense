using UnityEngine;
using Defines;

public class BattleStartButtton : MonoBehaviour
{

    public void StartBattleScene(string SceneName)
    {
        SceneLoader.SceneLoaderInstance.ChangeScene(SceneName);
    }


}
