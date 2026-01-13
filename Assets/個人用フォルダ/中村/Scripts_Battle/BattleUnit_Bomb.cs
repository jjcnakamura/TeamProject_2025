using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit_Bomb : BattleUnit_Base
{
    [Header("[BattleUnit_Bomb]")]

    //攻撃の対象にしている敵
    Collider targetEnemyCol;
    Enemy_Base targetEnemy;

    //攻撃が当たった敵のリスト
    List<Collider> hitEnemy = new List<Collider>();

    //タイマー
    [System.NonSerialized] public float explosionTime = 0.75f; //爆発の持続時間
    float timer_Explosion;

    //状態を表すフラグ
    public bool isStart, isExplosion, isEnd;

    protected override void Update()
    {
        base.Update(); //基底クラスのUpdate

        if (!isBattle || !BattleManager.Instance.isMainGame) return; //戦闘中でない場合は戻る

        Place();
        Rotate();
        DeadCheck();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate(); //基底クラスのFixedUpdate

        if (!isBattle || !BattleManager.Instance.isMainGame) return; //戦闘中でない場合は戻る

        NowExplosion();
    }

    //配置された時の処理
    void Place()
    {
        if (!isStart && isBattle)
        {
            //col_AttackZone.enabled = false;

            isDeadCheck = false;
            isExplosion = false;
            isStart = true;
        }
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
                isAnimation = false;
            }
        }
    }

    //ターゲットしている敵の方を向く
    void Rotate()
    {
        if (isTarget)
        {
            if (targetEnemy != null)
            {
                //狙う敵の方向を向く
                Quaternion targetDir = Quaternion.LookRotation(targetEnemy.transform.position - transform.position);
                Quaternion lookDir = new Quaternion(transform.rotation.x, targetDir.y, transform.rotation.z, targetDir.w);
                DirectionChange(lookDir);
            }
            else
            {
                //敵が存在しない場合はターゲットを止める
                Target();
            } 
        }
    }

    //自身が死亡しているかチェックする
    void DeadCheck()
    {
        //死亡した瞬間に爆発する
        if (!isExplosion && isDead && !isAnimation && !isEnd)
        {
            //エフェクトとHPバーを非表示に
            buffObj.SetActive(false);
            debuffObj.SetActive(false);
            hpbarObj.SetActive(false);

            //アニメーション
            if (animator != null) animator.Play(anim_Name);
            isAnimation = true;

            //アニメーション終了後に爆発する
            Invoke("Explosion", anim_Time);
        }
    }
    //爆発
    void Explosion()
    {
        if (isExplosion || isEnd) return;

        if (se_Action != null && se_Action.Length > 0) SoundManager.Instance.PlaySE_OneShot_Game(se_Action[0]);

        model.SetActive(false);

        //エフェクトを生成
        GameObject effectInstance = Instantiate(effect);
        effectInstance.transform.position = transform.position;
        effectInstance.transform.localScale = new Vector3(distance, distance, distance);

        col_Body.enabled = false;
        col_AttackZone.enabled = true;

        isExplosion = true;
    }
    //爆発中の処理
    void NowExplosion()
    {
        if (!isExplosion) return;

        if (timer_Explosion < explosionTime)
        {
            timer_Explosion += Time.fixedDeltaTime;
        }
        else
        {
            col_AttackZone.enabled = false;

            isExplosion = false;
            isEnd = true;

            isDeadCheck = true;
        }
    }

    //敵が爆風に触れた場合に呼び出す
    public void Hit(Collider targetCol)
    {
        if (!isBattle || !BattleManager.Instance.isMainGame) return; //戦闘中でない場合は戻る

        if (!isExplosion || targetCol == null) return;

        //既に攻撃が当たった敵か判定する
        if (hitEnemy.Count > 0)
        {
            for (int i = 0; i < hitEnemy.Count; i++)
            {
                //既に攻撃が当たっていた場合は戻る
                if (hitEnemy[i] != null && targetCol == hitEnemy[i])
                    return;
            }
        }
        Enemy_Base enemy = targetCol.transform.parent.GetComponent<Enemy_Base>();
        if (enemy != null)
        {
            //敵にダメージを与える
            bool dead = enemy.Damage(value);
            //敵が死亡していない場合はリストに加える
            if (!dead) hitEnemy.Add(targetCol);
        }
    }
}
