using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit_LongAttack : BattleUnit_Base
{
    [Header("[BattleUnit_LongAttack]")]

    //射出する弾
    [SerializeField] Bullet normalBullet;
    [SerializeField] Bullet_Explosion explosiveBullet;

    //攻撃の対象にしている敵
    Collider targetEnemyCol;
    Enemy_Base targetEnemy;

    //タイマー
    float timer_Interval;

    //状態を表すフラグ
    public bool isStart, isInterval;

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
        if (!isTarget) return;

        if (targetEnemy != null)
        {
            //狙う敵の方向を向く
            Quaternion targetDir = Quaternion.LookRotation(targetEnemy.transform.position - transform.position);
            Quaternion lookDir = new Quaternion(transform.rotation.x, targetDir.y, transform.rotation.z, targetDir.w);
            DirectionChange(lookDir);

            if (!isInterval)
            {
                //通常の弾を撃つ
                if (normalBullet != null)
                {

                }
                //広範囲に広がる弾を撃つ
                else if (explosiveBullet != null)
                {
                    Bullet_Explosion bullet = Instantiate(explosiveBullet);
                    bullet.transform.localScale = explosiveBullet.transform.localScale;
                    bullet.Shot(value, range, transform.position, targetEnemy.transform.position, effect);
                }
                else
                {
                    return;
                }

                //インターバル開始
                timer_Interval = 0;
                isInterval = true;
            }
        }
        else
        {
            //敵が存在しない場合はターゲットを止める
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
