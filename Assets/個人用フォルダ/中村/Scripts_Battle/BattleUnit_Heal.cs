using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleUnit_Heal : BattleUnit_Base
{
    [Header("[BattleUnit_Heal]")]

    //射出する回復弾
    [SerializeField] Bullet_Heal healBullet;

    //回復の対象にしているユニット
    List<Collider> targetUnitCol = new List<Collider>();
    List<BattleUnit_Base> targetUnit = new List<BattleUnit_Base>();

    //回復対象のユニット抽選用の変数
    List<BattleUnit_Base> healTarget = new List<BattleUnit_Base>();
    int healTargetIndex;

    //連続で回復する際の間隔
    float healInterval = 0.1f;

    //タイマー
    float timer_Interval;

    //状態を表すフラグ
    public bool isStart, isHeal, isInterval;

    protected override void Update()
    {
        base.Update(); //基底クラスのUpdate

        if (!isBattle || !BattleManager.Instance.isMainGame) return; //戦闘中でない場合は戻る

        Place();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate(); //基底クラスのFixedUpdate

        if (!isBattle || !BattleManager.Instance.isMainGame) return; //戦闘中でない場合は戻る

        StartHeal();
        Interval();
    }

    //配置された時の処理
    void Place()
    {
        if (!isStart && isBattle)
        {
            Target(col_Body);

            isInterval = true;
            isStart = true;
        }
    }

    //ユニットをターゲット、ターゲット解除
    public void Target(Collider targetCol = null)
    {
        if (!isBattle || !BattleManager.Instance.isMainGame) return; //戦闘中でない場合は戻る

        if (isHeal) return; //回復中の場合は戻る

        if (targetCol != null)
        {
            for (int i = 0; i < targetUnitCol.Count; i++)
            {
                //ターゲット中のユニットの場合は戻る
                if (targetUnitCol != null && targetCol == targetUnitCol[i])
                {
                    return;
                }
            }

            targetUnitCol.Add(targetCol);
            targetUnit.Add(targetCol.transform.parent.GetComponent<BattleUnit_Base>());

            isTarget = true;
        }
        else
        {
            targetUnitCol.Clear();
            targetUnit.Clear();

            isTarget = false;
        }
    }

    //回復開始
    void StartHeal()
    {
        if (!isTarget || isHeal || isInterval) return;

        //回復の対象になるユニットを選択する
        healTargetIndex = 0;
        //nullになっているターゲットを除外する
        targetUnit.RemoveAll(i => i == null);
        if (targetUnit.Count > 0)
        {
            //回復の対象になるユニットのHPをと最大HPに対する割合を保持
            int[] hp = new int[targetUnit.Count];
            float[] hpRatio = new float[targetUnit.Count];
            for (int i = 0; i < targetUnit.Count; i++)
            {
                hp[i] = targetUnit[i].hp;
                hpRatio[i] = hp[i] / (float)targetUnit[i].maxHp;
            }
            //HPが少ない順に選ぶ
            for (int i = 0; i < targetNum; i++)
            {
                int index = 0;
                for (int j = 1; j < hp.Length; j++)
                {
                    //HPの割合を比較
                    if (hpRatio[j] <= hpRatio[index])
                    {
                        bool choice = false;

                        if (hpRatio[j] != hpRatio[index])
                        {
                            choice = true;
                        }
                        else
                        {
                            //比較したHPが同数の場合はHP実数値が低い方を選ぶ
                            if (hp[j] < hp[index])
                            {
                                choice = true;
                            }
                            //HP実数値も同数の場合はランダムに片方を選ぶ
                            else if (UnityEngine.Random.Range(0, 2) == 0)
                            {
                                choice = true;
                            }
                        }
                        if (choice)
                        {
                            index = j;
                            hpRatio[j] = hpRatio[j];
                        }
                    }
                }

                //回復の対象のリストに加える
                if (targetUnit[index] != null && hp[index] < targetUnit[index].maxHp)
                healTarget.Add(targetUnit[index]);
                //回復するHPを足して保持中のHP値を更新
                hp[index] += defaultValue;
                hpRatio[index] = hp[index] / (float)targetUnit[index].maxHp;
            }
        }

        //アニメーションを再生して回復弾を出す
        isHeal = true;

        //回復対象がいない場合はアニメーションを再生しない
        if (healTarget.Count > 0)
        {
            if(animator != null) animator.Play(anim_Name);
            isAnimation = true;
        }
        
        Invoke("HealBullet", anim_Time);
    }
    //回復
    void HealBullet()
    {
        if (!isHeal) return;

        //回復弾を出す
        if (healTarget.Count > 0 && healTarget[healTargetIndex] != null && healBullet != null)
        {
            if (se_Action != null && se_Action.Length > 1) SoundManager.Instance.PlaySE_OneShot_Game(se_Action[0]);

            int seIndex = (se_Action != null && se_Action.Length > 1) ? se_Action[1] : -1;
            seIndex = (seIndex < 0 && se_Action != null && se_Action.Length > 0) ? se_Action[0] : seIndex;
            Bullet_Heal bullet = Instantiate(healBullet);
            bullet.transform.localScale = healBullet.transform.localScale;
            bullet.Shot(defaultValue, transform.position, healTarget[healTargetIndex].footPos.transform.position, healTarget[healTargetIndex], seIndex, effect);

            isAnimation = false;
        }
    }
    //回復のインターバル
    void Interval()
    {
        if (isHeal && !isAnimation)
        {
            if (timer_Interval < healInterval)
            {
                timer_Interval += Time.fixedDeltaTime;
            }
            else
            {
                timer_Interval = 0;
                healTargetIndex++;

                //弾数が残っている場合は回復弾を出す
                if (healTargetIndex < healTarget.Count)
                {
                    HealBullet();
                }
                else
                {
                    healTargetIndex = 0;

                    isHeal = false;

                    //インターバル開始
                    isInterval = true;

                    healTarget.Clear();
                    Target();
                    Target(col_Body);
                }
            }
        }
        else if (isInterval)
        {
            if (timer_Interval < interval)
            {
                timer_Interval += Time.fixedDeltaTime;
            }
            else
            {
                timer_Interval = 0;
                isAnimation = false;
                isInterval = false;
            }
        }
    }
}
