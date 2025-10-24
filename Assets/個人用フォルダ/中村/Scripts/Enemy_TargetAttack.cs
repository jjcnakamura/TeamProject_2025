using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TargetAttack : Enemy_Base
{
    [Header("Enemy_TargetAttack")]

    //Collider
    public BoxCollider col_AttackZone;

    [Space(10)]

    [SerializeField,Label("壁役を狙う")] public bool attackWall;
    [SerializeField,Label("ユニットエリアを狙う")] public bool attackUnitZone;

    //攻撃の対象にしているユニット
    Collider targetUnitCol;
    BattleUnit_Base targetUnit;

    //タイマー
    float timer_Interval;

    [Space(10)]

    //状態を表すフラグ
    public bool isInterval;

    protected override void Start()
    {
        base.Start(); //基底クラスのStart

        //Colliderのサイズを決める
        col_AttackZone.size = new Vector3(distance, col_Body.size.y, distance);

        //壁役ユニットを狙う用のColliderを生成する
        if (attackWall) GenerateAttackWallCollider();
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
                //攻撃してインターバル開始
                bool dead = targetUnit.Damage(value);
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

    //壁役ユニットを狙う用のColliderを生成する
    void GenerateAttackWallCollider()
    {
        GameObject atkWallObj = new GameObject();
        atkWallObj.name = "Col_AttackZone_Wall";

        atkWallObj.transform.SetParent(transform);
        atkWallObj.transform.position = col_AttackZone.transform.position;
        atkWallObj.transform.rotation = new Quaternion();
        atkWallObj.transform.localScale = new Vector3(1f, 1f, 1f);

        BoxCollider atkWallCol = atkWallObj.AddComponent<BoxCollider>();
        atkWallCol.isTrigger = true;
        atkWallCol.center = new Vector3(0f, 0f, 1f);
        atkWallCol.size = new Vector3(1f, col_AttackZone.size.y, 1f);

        EnemyCollider_AttackZone_Wall atkWallColComponent = atkWallObj.AddComponent<EnemyCollider_AttackZone_Wall>();
        atkWallColComponent.enemy_TargetAttack = this;
    }
}
