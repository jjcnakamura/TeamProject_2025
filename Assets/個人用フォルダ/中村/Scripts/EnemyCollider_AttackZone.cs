using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider_AttackZone : MonoBehaviour
{
    [SerializeField] Enemy_TargetAttack enemy_TargetAttack;

    void OnTriggerStay(Collider other)
    {
        //射程に入ったユニットエリアのユニットをターゲットする敵の処理
        if (enemy_TargetAttack != null)
        {
            //ユニットが攻撃の射程内に入った場合
            if (other.transform.tag == "Unit" && enemy_TargetAttack.attackUnitZone && !enemy_TargetAttack.isTarget)
            {
                enemy_TargetAttack.Target(other);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //射程に入ったユニットエリアのユニットをターゲットする敵の処理
        if (enemy_TargetAttack != null)
        {
            //ユニットが攻撃の射程外に出た場合
            if (other.transform.tag == "Unit" && enemy_TargetAttack.attackUnitZone && !enemy_TargetAttack.isTarget)
            {
                enemy_TargetAttack.Target(other);
            }
        }
    }
}
