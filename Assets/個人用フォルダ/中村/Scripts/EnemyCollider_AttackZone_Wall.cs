using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider_AttackZone_Wall : MonoBehaviour
{
    [SerializeField] Enemy_TargetAttack enemy_TargetAttack;
    [SerializeField] Enemy_StatusChange enemy_StatusChange;

    void OnTriggerStay(Collider other)
    {
        //射程に入った壁役ユニットをターゲットする敵の処理
        if (enemy_TargetAttack != null)
        {
            //ユニットが攻撃の射程内に入った場合
            if (other.transform.tag == "Unit_Wall" && enemy_TargetAttack.attackWall && !enemy_TargetAttack.isTarget)
            {
                enemy_TargetAttack.Target(other);
            }
        }

        //ステータスを変化させるユニットの処理
        if (enemy_StatusChange != null)
        {
            //壁ユニットが前方に来た場合
            if (other.transform.tag == "Unit_Wall" && !enemy_StatusChange.isCollisionWallUnit)
            {
                enemy_StatusChange.CollisionWallUnit(other);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        return; //攻撃範囲から離れた場合もターゲットを続けるため以下の処理はしない

        //射程に入った壁役ユニットをターゲットする敵の処理
        if (enemy_TargetAttack != null)
        {
            //ユニットが攻撃の射程外に出た場合
            if (other.transform.tag == "Unit_Wall" && enemy_TargetAttack.attackWall && enemy_TargetAttack.isTarget)
            {
                enemy_TargetAttack.Target(other);
            }
        }

        //ステータスを変化させるユニットの処理
        if (enemy_StatusChange != null)
        {
            //壁ユニットが前方に来た場合
            if (other.transform.tag == "Unit_Wall" && enemy_StatusChange.isCollisionWallUnit)
            {
                enemy_StatusChange.CollisionWallUnit(other);
            }
        }
    }
}
