using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCollider_AttackZone : MonoBehaviour
{
    [SerializeField] BattleUnit_TargetAttack unit_TargetAttack;
    [SerializeField] BattleUnit_LongAttack unit_LongAttack;

    void OnTriggerStay(Collider other)
    {
        //射程に入った敵をターゲットするユニットの処理
        if (unit_TargetAttack != null)
        {
            //敵が攻撃の射程内に入った場合
            if (other.transform.tag == "Enemy" && !unit_TargetAttack.isTarget)
            {
                unit_TargetAttack.Target(other);
            }
        }

        //射程に入った敵に遠距離攻撃をするユニットの処理
        if (unit_LongAttack != null)
        {
            //敵が攻撃の射程内に入った場合
            if (other.transform.tag == "Enemy" && !unit_LongAttack.isTarget)
            {
                unit_LongAttack.Target(other);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //射程に入った敵をターゲットするユニットの処理
        if (unit_TargetAttack != null)
        {
            //敵が攻撃の射程外に出た場合
            if (other.transform.tag == "Enemy" && unit_TargetAttack.isTarget)
            {
                unit_TargetAttack.Target(other);
            }
        }

        //射程に入った敵に遠距離攻撃をするユニットの処理
        if (unit_LongAttack != null)
        {
            //敵が攻撃の射程外に出た場合
            if (other.transform.tag == "Enemy" && unit_LongAttack.isTarget)
            {
                unit_LongAttack.Target(other);
            }
        }
    }
}
