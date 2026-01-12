using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit_TargetAttack : BattleUnit_Base
{
    [Header("[BattleUnit_TargetAttack]")]

    //攻撃の対象にしている敵
    Collider targetEnemyCol;
    Enemy_Base targetEnemy;

    //タイマー
    float timer_Interval;

    //状態を表すフラグ
    public bool isInterval;

    protected override void Update()
    {
        base.Update(); //基底クラスのUpdate

        if (!isBattle || !BattleManager.Instance.isMainGame) return; //戦闘中でない場合は戻る

        Defense();
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
        if (!isTarget) return;

        if (targetEnemy != null)
        {
            //狙う敵の方向を向く
            Quaternion targetDir = Quaternion.LookRotation(targetEnemy.transform.position - transform.position);
            Quaternion lookDir = new Quaternion(transform.rotation.x, targetDir.y, transform.rotation.z, targetDir.w);
            DirectionChange(lookDir);

            //攻撃しない場合は戻る
            if (noAction) return;

            if (!isInterval && !isAnimation)
            {
                //アニメーション
                if (animator != null) animator.Play(anim_Name);
                isAnimation = true;

                //アニメーション終了後にダメージを与える
                Invoke("ToDamage", anim_Time);
            }
        }
        else
        {
            //敵が存在しない場合はターゲットを止める
            Target();
        }
    }
    //攻撃でダメージを与える
    void ToDamage()
    {
        if (!isTarget) return;

        if (targetEnemy != null)
        {
            //SE
            if (se_Action != null && se_Action.Length > 0) SoundManager.Instance.PlaySE_OneShot_Game(se_Action[0]);

            //攻撃
            bool dead = targetEnemy.Damage(value);
            //エフェクトを敵の位置に生成
            Instantiate(effect).transform.position = targetEnemy.transform.position;
            //インターバル開始
            timer_Interval = 0;
            isInterval = true;

            //敵がダメージで死亡した場合はターゲットを止める
            if (dead) Target();
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

                isAnimation = false;
                isInterval = false;
            }
        }
    }
    //攻撃しない場合は盾を構えるアニメーションを再生する
    void Defense()
    {
        if (!noAction || isAnimation) return;

        if (isBattle)
        {
            //アニメーション
            if (animator != null) animator.SetTrigger(anim_Name);
            isAnimation = true;
        }
    }
}
