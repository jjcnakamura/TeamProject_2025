using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦闘シーンの管理用
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    void Awake()
    {
        //デバッグ用　初期キャラ3体をロード
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(1);
        ParameterManager.Instance.AddUnit(2);
    }
}
