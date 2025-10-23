using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider_AttackZone : MonoBehaviour
{
    [SerializeField] Enemy_TargetAttack enemy_TargetAttack;

    void OnTriggerStay(Collider other)
    {
        //�˒��ɓ��������j�b�g�G���A�̃��j�b�g���^�[�Q�b�g����G�̏���
        if (enemy_TargetAttack != null)
        {
            //���j�b�g���U���̎˒����ɓ������ꍇ
            if (other.transform.tag == "Unit" && enemy_TargetAttack.attackUnitZone && !enemy_TargetAttack.isTarget)
            {
                enemy_TargetAttack.Target(other);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //�˒��ɓ��������j�b�g�G���A�̃��j�b�g���^�[�Q�b�g����G�̏���
        if (enemy_TargetAttack != null)
        {
            //���j�b�g���U���̎˒��O�ɏo���ꍇ
            if (other.transform.tag == "Unit" && enemy_TargetAttack.attackUnitZone && !enemy_TargetAttack.isTarget)
            {
                enemy_TargetAttack.Target(other);
            }
        }
    }
}
