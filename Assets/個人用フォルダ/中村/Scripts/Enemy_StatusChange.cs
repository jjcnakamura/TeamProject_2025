using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_StatusChange : Enemy_Base
{
    [Header("Enemy_StatusChange")]

    [SerializeField,Label("バフをかける（falseだとデバフ）")] public bool buff;

    //ステータスを変化させている敵、ユニットのコンポーネント
    List<Enemy_Base> buffEnemy = new List<Enemy_Base>();
    List<BattleUnit_Base> debuffUnit = new List<BattleUnit_Base>();

    //前方にいる壁ユニット
    Collider wallUnitCol;
    BattleUnit_Base wallUnit;
    int targetPosIndex;

    //状態を表すフラグ
    public bool isCollisionWallUnit, isDeadCheck_StatusChange;

    protected override void Start()
    {
        base.Start(); //基底クラスのStart

        //Colliderの位置とサイズを決める
        col_AttackZone.transform.localScale = new Vector3(distance, col_AttackZone.transform.localScale.y, distance);

        //基底クラスで死亡の判定をしない
        isDeadCheck = false;
        isDeadCheck_StatusChange = true;
    }

    protected override void Update()
    {
        base.Update(); //基底クラスのUpdate

        WallUnitCollisionCheck();
        DeadCheck_StatusChange();
    }

    //敵にバフをかける
    public void Buff(Collider targetCol, bool flag)
    {
        if (!buff) return;

        Enemy_Base targetEnemy = targetCol.transform.parent.GetComponent<Enemy_Base>();
        if (targetEnemy == null) return;

        if (flag)
        {
            for (int i = buffEnemy.Count - 1; i >= 0; i--)
            {
                if (buffEnemy[i] == targetEnemy)
                {
                    return;
                }
            }

            targetEnemy.StatusChange(defaultValue, true);
            buffEnemy.Add(targetEnemy);
        }
        else
        {
            targetEnemy.StatusChange(defaultValue, false);

            for (int i = buffEnemy.Count - 1; i >= 0; i--)
            {
                if (buffEnemy[i] == targetEnemy)
                {
                    buffEnemy.RemoveAt(i);
                    break;
                }
            }
        }
    }
    //ユニットにデバフをかける
    public void Debuff(Collider targetCol, bool flag)
    {
        if (buff) return;

        BattleUnit_Base targetUnit = targetCol.transform.parent.GetComponent<BattleUnit_Base>();
        if (targetUnit == null) return;

        if (flag)
        {
            for (int i = debuffUnit.Count - 1; i >= 0; i--)
            {
                if (debuffUnit[i] == targetUnit)
                {
                    return;
                }
            }

            targetUnit.StatusChange(-defaultValue, true);
            debuffUnit.Add(targetUnit);
        }
        else
        {
            targetUnit.StatusChange(-defaultValue, false);

            for (int i = debuffUnit.Count - 1; i >= 0; i--)
            {
                if (debuffUnit[i] == targetUnit)
                {
                    debuffUnit.RemoveAt(i);
                    break;
                }
            }
        }
    }
    //壁ユニットの衝突判定
    public void CollisionWallUnit(Collider col = null)
    {
        if (!isCollisionWallUnit)
        {
            if (wallUnitCol == null)
            {
                wallUnitCol = col;
                wallUnit = wallUnitCol.transform.parent.GetComponent<BattleUnit_Base>();

                if (wallUnit != null)
                {
                    //ユニットがターゲットされている数によって自身の位置を決める
                    targetPosIndex = wallUnit.beingTarget.Length;
                    for (int i = 0; i < wallUnit.beingTarget.Length; i++)
                    {
                        //ターゲット場所が空いている場合はその位置に
                        if (!wallUnit.beingTarget[i])
                        {
                            wallUnit.beingTarget[i] = true;
                            targetPosIndex = i;
                            break;
                        }
                    }
                    //targetPosIndexによって位置を決める
                    if (targetPosIndex <= 0)
                    {
                        targetPos = col_AttackZone_Wall.transform.position;
                    }
                    else if (targetPosIndex == 1)
                    {
                        targetPos = (transform.position + col_AttackZone_Wall.transform.position) / 2;
                    }
                    else
                    {
                        targetPos = transform.position;
                    }
                }
                else
                {
                    wallUnit = null;
                    return;
                }

                isCollisionWallUnit = true;
                isTarget = true;
                isMove = false;
            }
        }
        else
        {
            if (col == wallUnitCol || col == null || wallUnitCol)
            {
                wallUnitCol = null;
                wallUnit = null;

                if (wallUnit != null)
                {
                    //ユニットのターゲットフラグを外す
                    if (targetPosIndex < wallUnit.beingTarget.Length) wallUnit.beingTarget[targetPosIndex] = false;
                    targetPosIndex = 0;
                }

                isCollisionWallUnit = false;
                isTarget = false;
                isMove = true;
            }
        }
    }

    //前方にいる壁ユニットが存在するか判定する
    void WallUnitCollisionCheck()
    {
        if (!isCollisionWallUnit) return;

        if (wallUnit == null)
        {
            wallUnitCol = null;

            isCollisionWallUnit = false;
            isTarget = false;
            isMove = true;
        }
    }
    //死亡している場合はステータスの変化を解除
    void DeadCheck_StatusChange()
    {
        if (!isDeadCheck_StatusChange) return;

        if (isDead)
        {
            if (buff)
            {
                //バフを解除
                for (int i = buffEnemy.Count - 1; i >= 0; i--)
                {
                    buffEnemy[i].StatusChange(defaultValue, false);
                }
            }
            else
            {
                //デバフを解除
                for (int i = debuffUnit.Count - 1; i >= 0; i--)
                {
                    debuffUnit[i].StatusChange(-defaultValue, false);
                }
            }

            isDead = true;
            isDeadCheck = true;
            isDeadCheck_StatusChange = false;
        }
    }
}
