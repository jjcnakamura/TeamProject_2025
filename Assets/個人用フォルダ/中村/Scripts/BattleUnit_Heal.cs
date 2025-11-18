using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit_Heal : BattleUnit_Base
{
    [Header("[BattleUnit_Heal]")]

    //射出する回復弾
    [SerializeField] Bullet_Heal healBullet;

    //回復の対象にしているユニット
    List<Collider> targetUnitCol = new List<Collider>();
    List<BattleUnit_Base> targetUnit = new List<BattleUnit_Base>();

    //タイマー
    float timer_Interval;

    //状態を表すフラグ
    public bool isInterval;

    protected override void FixedUpdate()
    {
        base.FixedUpdate(); //基底クラスのFixedUpdate

        if (!isBattle || !BattleManager.Instance.isMainGame) return; //戦闘中でない場合は戻る

        Heal();
        Interval();
    }

    //ユニットをターゲット、ターゲット解除
    public void Target(Collider targetCol = null)
    {
        if (!isBattle || !BattleManager.Instance.isMainGame) return; //戦闘中でない場合は戻る

        targetUnit.Add(targetCol.transform.parent.GetComponent<BattleUnit_Base>());
        isTarget = true;

        if (targetCol == null)
        {
            targetUnitCol = null;
            targetUnit = null;

            isTarget = false;
        }
    }

    //回復
    void Heal()
    {
        if (!isTarget) return;

        if (targetUnit[0] != null)
        {
            if (!isInterval)
            {
                //回復弾を出す
                if (healBullet != null)
                {
                    Bullet_Heal bullet = Instantiate(healBullet);
                    bullet.transform.localScale = healBullet.transform.localScale;
                    bullet.Shot(value, transform.position, targetUnit[0].transform.position, targetUnit[0], effect);
                }

                //インターバル開始
                timer_Interval = 0;
                isInterval = true;
            }
        }
    }
    //回復のインターバル
    void Interval()
    {
        if (isInterval)
        {
            if (timer_Interval < interval)
            {
                timer_Interval += Time.fixedDeltaTime;
            }
            else
            {
                timer_Interval = 0;
                isInterval = false;
            }
        }
    }
}
