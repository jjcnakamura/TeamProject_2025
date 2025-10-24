using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit_PointUp : BattleUnit_Base
{
    [Header("BattleUnit_PointUp")]

    //タイマー
    float timer_Interval;

    protected override void FixedUpdate()
    {
        base.FixedUpdate(); //基底クラスのFixedUpdate

        if (!isBattle || !BattleManager.Instance.isMainGame) return; //戦闘中でない場合は戻る

        PointUp();
    }

    //ポイント上昇
    void PointUp()
    {
        if (timer_Interval < interval)
        {
            timer_Interval += Time.fixedDeltaTime;
        }
        else
        {
            //ポイントを上昇してインターバル開始
            BattleManager.Instance.PointChange(defaultValue);
            timer_Interval = 0;
        }
    }
}
