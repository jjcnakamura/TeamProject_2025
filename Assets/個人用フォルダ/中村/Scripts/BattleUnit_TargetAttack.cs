using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit_TargetAttack : BattleUnit_Base
{
    [Header("BattleUnit_TargetAttack")]

    //Collider
    public BoxCollider col_AttackZone;

    //攻撃の対象にしている敵
    Collider targetEnemyCol;
    Enemy_Base targetEnemy;

    //タイマー
    float timer_Interval;

    //状態を表すフラグ
    public bool isStart, isInterval;

    void Update()
    {
        //最初にステージ上に配置された場合にColliderのサイズを決める
        if (!isStart && isBattle)
        {
            col_AttackZone.size = new Vector3(distance, col_Body.size.y, distance);
            isStart = true;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate(); //基底クラスのFixedUpdate

        if (!isBattle || !BattleManager.Instance.isMainGame) return; //戦闘中でない場合は戻る

        Attack();
        Interval();
    }

    //敵をターゲット、ターゲット解除
    public void Target(Collider targetCol = null)
    {
        if (!isBattle || !BattleManager.Instance.isMainGame) return; //戦闘中でない場合は戻る

        if (!isTarget)
        {
            if (targetEnemyCol == null)
            {
                targetEnemyCol = targetCol;
                targetEnemy = targetCol.transform.parent.GetComponent<Enemy_Base>();

                isTarget = true;
                isRotation = true;
            }
        }
        else
        {
            if (targetCol == targetEnemyCol || targetCol == null)
            {
                targetEnemyCol = null;
                targetEnemy = null;

                isTarget = false;
                isRotation = false;
            }
        }
    }

    //攻撃
    void Attack()
    {
        if (!isTarget || targetEnemy == null) return;

        //狙う敵の方向を向く
        Quaternion targetDir = Quaternion.LookRotation(targetEnemy.transform.position - transform.position);
        Quaternion lookDir = new Quaternion(transform.rotation.x, targetDir.y, transform.rotation.z, targetDir.w);
        DirectionChange(lookDir);

        if (!isInterval)
        {
            //攻撃してインターバル開始
            bool dead = targetEnemy.Damage(value);
            timer_Interval = 0;
            isInterval = true;

            //敵がダメージで死亡した場合はターゲットを止める
            if (dead) Target();
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
