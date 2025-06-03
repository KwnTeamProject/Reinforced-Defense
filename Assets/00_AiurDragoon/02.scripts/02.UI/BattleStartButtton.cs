using UnityEngine;
using Defines;

public class BattleStartButtton : MonoBehaviour
{

    public void StartBattleScene()
    {
        SceneLoader.SceneLoaderInstance.ChangeScene("StartBattleScene");
    }


}
