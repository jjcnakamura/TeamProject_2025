using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーと所持ユニットのパラメーターを管理する
/// </summary>
public class PlayerManager : Singleton<PlayerManager>
{
    //プレイヤーのステータス
    public int point;                   //初期ポイント数
    public int maxInstallation;         //ユニット最大配置数
    public int sameUnitMaxInstallation; //同じユニットの最大配置数
    public int maxPossession;           //最大ユニット所持数

    //所持しているユニットのステータス
    public UnitStatus[] unitStatus;

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
