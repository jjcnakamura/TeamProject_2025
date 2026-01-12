using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit_LongAttack : BattleUnit_Base
{
    [Header("[BattleUnit_LongAttack]")]

    //射出する弾
    [SerializeField] Bullet normalBullet;
    [SerializeField] Bullet_Explosion explosiveBullet;
    [SerializeField] Bullet_SpeedDebuff speedDebuffBullet;

    //攻撃の対象にしている敵
    Collider targetEnemyCol;
    Enemy_Base targetEnemy;

    //タイマー
    float timer_Interval;

    [Space(10)]

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

            if (!isInterval &&!isAnimation)
            {
                //アニメーション
                if (animator != null) animator.Play(anim_Name);
                isAnimation = true;

                //アニメーション終了後に弾を撃つ
                Invoke("Shot", anim_Time);
            }
        }
        else
        {
            //敵が存在しない場合はターゲットを止める
            Target();
        }
    }
    //弾を撃つ
    void Shot()
    {
        if (!isTarget) return;

        if (targetEnemy != null)
        {
            //通常の弾を撃つ
            if (normalBullet != null)
            {

            }
            //広範囲に広がる弾を撃つ
            else if (explosiveBullet != null)
            {
                int seIndex = (se_Action != null && se_Action.Length > 0) ? se_Action[0] : -1;
                Bullet_Explosion bullet = Instantiate(explosiveBullet);
                bullet.transform.localScale = explosiveBullet.transform.localScale;
                bullet.Shot(value, range, transform.position, targetEnemy.transform.position, seIndex, effect);
            }
            //速度鈍化デバフの弾を撃つ
            else if (speedDebuffBullet != null)
            {
                int seIndex = (se_Action != null && se_Action.Length > 0) ? se_Action[0] : -1;
                Bullet_SpeedDebuff bullet = Instantiate(speedDebuffBullet);
                bullet.transform.localScale = speedDebuffBullet.transform.localScale;
                bullet.Shot(0.5f, defaultValue, range, transform.position, targetEnemy.transform.position, seIndex, effect);
            }
            else
            {
                return;
            }

            //インターバル開始
            timer_Interval = 0;
            isAnimation = false;
            isInterval = true;
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
