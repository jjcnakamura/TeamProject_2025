using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_StatusChange : Enemy_Base
{
    [Header("Enemy_StatusChange")]

    [SerializeField,Label("バフをかける（falseだとデバフ）")] public bool buff;

    //Collider
    public CapsuleCollider col_AttackZone;
    public BoxCollider col_AttackZone_Wall;
    [System.NonSerialized] MeshRenderer mesh_AttackZone;

    //ステータスを変化させている敵、ユニットのコンポーネント
    List<Enemy_Base> buffEnemy = new List<Enemy_Base>();
    List<BattleUnit_Base> debuffUnit = new List<BattleUnit_Base>();

    //前方にいる壁ユニットのCollider
    Collider wallUnitCol;

    //状態を表すフラグ
    public bool isCollisionWallUnit, isDeadCheck_StatusChange;

    protected override void Start()
    {
        base.Start(); //基底クラスのStart

        //Colliderの位置とサイズを決める
        col_AttackZone.transform.localScale = new Vector3(distance, col_AttackZone.transform.localScale.y, distance);
        mesh_AttackZone = col_AttackZone.GetComponent<MeshRenderer>();
        mesh_AttackZone.enabled = false;

        col_AttackZone_Wall.center = new Vector3(0f, 0f, 1f);
        col_AttackZone_Wall.size = new Vector3(1f, col_Body.size.y, 1f);

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

                isCollisionWallUnit = true;
                isMove = false;
            }
        }
        else
        {
            if (col == wallUnitCol || col == null || wallUnitCol)
            {
                wallUnitCol = null;

                isCollisionWallUnit = false;
                isMove = true;
            }
        }
    }

    //前方にいる壁ユニットが死亡したか判定する
    void WallUnitCollisionCheck()
    {
        if (!isCollisionWallUnit) return;

        if (wallUnitCol == null)
        {
            isCollisionWallUnit = false;
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
