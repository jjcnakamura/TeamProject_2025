using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleManagerのギブアップ関連の関数呼び出し用
/// </summary>
public class Giveup : MonoBehaviour
{
    /// <summary>
    /// あきらめるボタン
    /// </summary>
    public void GiveupWindow()
    {
        BattleManager.Instance.Giveup();
    }

    /// <summary>
    /// タイトルに戻る(データは保存されない)
    /// </summary>
    public void BackTitle()
    {
        BattleManager.Instance.BackTitle();
    }
}
