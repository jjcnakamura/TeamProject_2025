using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider_AttackZone : MonoBehaviour
{
    [SerializeField] Enemy_TargetAttack enemy_TargetAttack;
    [SerializeField] Enemy_StatusChange enemy_StatusChange;

    void OnTriggerStay(Collider other)
    {
        //射程に入ったユニットエリアのユニットをターゲットする敵の処理
        if (enemy_TargetAttack != null && other != enemy_TargetAttack.col_Body)
        {
            //ユニットが攻撃の射程内に入った場合
            if (other.transform.tag == "Unit" && enemy_TargetAttack.attackUnitZone && !enemy_TargetAttack.isTarget)
            {
                enemy_TargetAttack.Target(other);
            }
        }

        //周り敵にバフをかける敵の処理
        if (enemy_StatusChange != null && enemy_StatusChange.buff && other != enemy_StatusChange.col_Body)
        {
            //敵がバフの射程内に入った場合
            if (other.transform.tag == "Enemy")
            {
                enemy_StatusChange.Buff(other, true); ;
            }
        }

        //周りのユニットにデバフをかける敵の処理
        if (enemy_StatusChange != null && !enemy_StatusChange.buff && other != enemy_StatusChange.col_Body)
        {
            //ユニットがデバフの射程内に入った場合
            if (other.transform.tag == "Unit" || other.transform.tag == "Unit_Wall")
            {
                enemy_StatusChange.Debuff(other, true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        return; //攻撃範囲から離れた場合もターゲットを続けるため以下の処理はしない

        //射程に入ったユニットエリアのユニットをターゲットする敵の処理
        if (enemy_TargetAttack != null && other != enemy_TargetAttack.col_Body)
        {
            //ユニットが攻撃の射程外に出た場合
            if (other.transform.tag == "Unit" && enemy_TargetAttack.attackUnitZone && !enemy_TargetAttack.isTarget)
            {
                enemy_TargetAttack.Target(other);
            }
        }

        //周りの敵にバフをかける敵の処理
        if (enemy_StatusChange != null && enemy_StatusChange.buff && other != enemy_StatusChange.col_Body)
        {
            //敵がバフの射程外に出た場合
            if (other.transform.tag == "Enemy")
            {
                enemy_StatusChange.Buff(other, false); ;
            }
        }

        //周りのユニットにデバフをかける敵の処理
        if (enemy_StatusChange != null && !enemy_StatusChange.buff && other != enemy_StatusChange.col_Body)
        {
            //ユニットがデバフの射程外に出た場合
            if (other.transform.tag == "Unit" || other.transform.tag == "Unit_Wall")
            {
                enemy_StatusChange.Debuff(other, false);
            }
        }
    }
}
