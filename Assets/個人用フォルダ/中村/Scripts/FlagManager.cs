using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : Singleton<FlagManager>
{
    //マップシーン用
    [System.NonSerialized] public bool isMapScene;

    //戦闘シーン用
    [System.NonSerialized] public bool isBattleScene, isSpeedUp;

    void Awake()
    {
        //シーンを遷移しても残る
        if (gameObject.transform.parent != null) gameObject.transform.parent = null;
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
