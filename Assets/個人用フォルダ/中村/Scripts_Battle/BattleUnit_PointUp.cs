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

        PointUpCount();
    }

    //ポイント上昇の時間をカウント
    void PointUpCount()
    {
        if (timer_Interval < interval)
        {
            timer_Interval += Time.fixedDeltaTime;
        }
        else if (!isAnimation)
        {
            //アニメーション
            isAnimation = true;
            if (animator != null) animator.SetTrigger(anim_Name);

            //アニメーション終了後にポイントを上昇させる
            Invoke("PointUp", anim_Time);
        }
    }
    //ポイント上昇
    void PointUp()
    {
        if (se_Action != null && se_Action.Length > 0) SoundManager.Instance.PlaySE_OneShot_Game(se_Action[0]);

        //ポイントを上昇
        BattleManager.Instance.PointChange(defaultValue);
        //エフェクトを自身の位置に生成
        Instantiate(effect).transform.position = footPos.transform.position;
        //インターバル開始
        timer_Interval = 0;

        isAnimation = false;
    }
}
