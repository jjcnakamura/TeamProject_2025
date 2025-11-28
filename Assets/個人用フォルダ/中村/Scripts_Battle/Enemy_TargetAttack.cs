using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TargetAttack : Enemy_Base
{
    [Header("[Enemy_TargetAttack]")]

    [SerializeField,Label("壁役を狙う")] public bool attackWall;
    [SerializeField,Label("ユニットエリアを狙う")] public bool attackUnitZone;

    //攻撃の対象にしているユニット
    Collider targetUnitCol;
    BattleUnit_Base targetUnit;
    int targetPosIndex;

    //タイマー
    float timer_Interval;

    [Space(10)]

    //状態を表すフラグ
    public bool isInterval;

    protected override void Start()
    {
        base.Start(); //基底クラスのStart

        //Colliderの位置とサイズを決める
        col_AttackZone.transform.localScale = new Vector3(distance, col_AttackZone.transform.localScale.y, distance);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate(); //基底クラスのFixedUpdate

        if (!BattleManager.Instance.isMainGame) return; //メインゲーム中でなければ戻る

        Attack();
        Interval();
    }

    //ユニットをターゲット、ターゲット解除
    public void Target(Collider targetCol = null)
    {
        if (!isTarget)
        {
            if (targetUnitCol == null)
            {
                targetUnitCol = targetCol;
                targetUnit = targetCol.transform.parent.GetComponent<BattleUnit_Base>();

                if  (targetUnit != null)
                {
                    targetUnit.beingTargetNum++;

                    //ユニットがターゲットされている数を取得
                    targetPosIndex = targetUnit.beingTarget.Length;
                    for (int i = 0; i < targetUnit.beingTarget.Length; i++)
                    {
                        if (!targetUnit.beingTarget[i])
                        {
                            targetUnit.beingTarget[i] = true;
                            targetPosIndex = i;
                            break;
                        }
                    }
                    //ターゲット中のみ敵同士の押し出し判定をオンにする
                    if (targetPosIndex < targetUnit.beingTarget.Length)
                    {
                        rig.drag = Mathf.Max(maxDrag * ((float)(targetUnit.beingTarget.Length - targetPosIndex) / (float)targetUnit.beingTarget.Length), minDrag);
                        rig.angularDrag = rig.drag;
                        col_Parent.enabled = true;
                    }
                    //押し出し判定がオンにならない敵は待ち時間を決める
                    else
                    {
                        moveWaitTime = moveWaitTimeRadix * (targetUnit.beingTargetNum - targetUnit.beingTarget.Length);
                    }
                }
                else
                {
                    targetUnitCol = null;
                    return;
                }

                //狙うユニットの方向を向く
                Quaternion targetDir = Quaternion.LookRotation(targetUnit.transform.position - transform.position);
                Quaternion lookDir = new Quaternion(transform.rotation.x, targetDir.y, transform.rotation.z, targetDir.w);
                DirectionChange(lookDir);

                isTarget = true;
                isMove = false;
            }
        }
        else
        {
            if (targetCol == targetUnitCol || targetCol == null)
            {
                if (targetUnit != null)
                {
                    //ユニットのターゲットフラグを外す
                    if (targetPosIndex < targetUnit.beingTarget.Length) targetUnit.beingTarget[targetPosIndex] = false;
                    targetUnit.beingTargetNum--;
                    targetPosIndex = 0;
                }

                //移動待ち時間開始
                if (moveWaitTime > 0) isWait = true;

                //敵同士の押し出し判定をオフにする
                rig.drag = minDrag;
                rig.angularDrag = minDrag;
                col_Parent.enabled = false;

                targetUnitCol = null;
                targetUnit = null;

                //狙うユニットの方向を外す
                Quaternion targetDir = Quaternion.LookRotation(spawnPoint.routePoint[routeIndex].pos[currentRoute] - transform.position);
                Quaternion lookDir = new Quaternion(transform.rotation.x, targetDir.y, transform.rotation.z, targetDir.w);
                DirectionChange(lookDir);

                isTarget = false;
                isMove = true;
            }
        }
    }
    //攻撃
    void Attack()
    {
        if (!isTarget) return;

        if (targetUnit != null)
        {
            if (!isInterval)
            {
                //攻撃
                bool dead = targetUnit.Damage(value);
                //エフェクトを攻撃ユニットの位置に生成
                Instantiate(effect).transform.position = targetUnit.transform.position;
                //インターバル開始
                timer_Interval = 0;
                isInterval = true;

                //ユニットがダメージで死亡した場合はターゲットを止める
                if (dead) Target();
            }
        }
        else
        {
            //ユニットが存在しない場合はターゲットを止める
            Target();
        }
    }
    //攻撃のインターバル
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
