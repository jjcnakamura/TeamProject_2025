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
        if (enemy_TargetAttack != null)
        {
            //ユニットが攻撃の射程内に入った場合
            if (other.transform.tag == "Unit" && enemy_TargetAttack.attackUnitZone && !enemy_TargetAttack.isTarget)
            {
                enemy_TargetAttack.Target(other);
            }
        }

        //周り敵のにバフをかける敵の処理
        if (enemy_StatusChange != null)
        {

        }

        //周りのユニットにデバフをかける敵の処理
        if (enemy_StatusChange != null)
        {

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

        //周りの敵にバフをかける敵の処理
        if (enemy_StatusChange != null)
        {

        }

        //周りのユニットにデバフをかける敵の処理
        if (enemy_StatusChange != null)
        {

        }
    }
}
