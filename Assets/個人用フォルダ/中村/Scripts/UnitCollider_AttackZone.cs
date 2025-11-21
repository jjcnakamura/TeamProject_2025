using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCollider_AttackZone : MonoBehaviour
{
    [SerializeField] BattleUnit_TargetAttack unit_TargetAttack;
    [SerializeField] BattleUnit_LongAttack unit_LongAttack;
    [SerializeField] BattleUnit_Bomb unit_Bomb;
    [SerializeField] BattleUnit_Heal unit_Heal;
    [SerializeField] BattleUnit_SlowDebuff unit_SlowDebuff;

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

        //爆弾ユニットの処理
        if (unit_Bomb != null)
        {
            //敵が攻撃の射程内に入った場合
            if (other.transform.tag == "Enemy" && unit_Bomb.isExplosion)
            {
                unit_Bomb.Hit(other);
            }
        }

        //範囲内の味方を回復するユニットの処理
        if (unit_Heal != null && other != unit_Heal.col_Body)
        {
            //味方が回復の範囲内に入った場合
            if (other.transform.tag == "Unit" || other.transform.tag == "Unit_Wall")
            {
                unit_Heal.Target(other);
            }
        }

        //範囲内の敵に鈍化デバフを付与するユニットの処理
        if (unit_SlowDebuff != null)
        {
            //敵がデバフの範囲内に入った場合
            if (other.transform.tag == "Enemy")
            {
                
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
