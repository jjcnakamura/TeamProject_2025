using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider_AttackZone_Wall : MonoBehaviour
{
    public Enemy_TargetAttack enemy_TargetAttack { private get; set; }

    void OnTriggerStay(Collider other)
    {
        //�˒��ɓ������ǖ����j�b�g���^�[�Q�b�g����G�̏���
        if (enemy_TargetAttack != null)
        {
            //���j�b�g���U���̎˒����ɓ������ꍇ
            if (other.transform.tag == "Unit_Wall" && enemy_TargetAttack.attackWall && !enemy_TargetAttack.isTarget)
            {
                enemy_TargetAttack.Target(other);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //�˒��ɓ������ǖ����j�b�g���^�[�Q�b�g����G�̏���
        if (enemy_TargetAttack != null)
        {
            //���j�b�g���U���̎˒��O�ɏo���ꍇ
            if (other.transform.tag == "Unit_Wall" && enemy_TargetAttack.attackWall && enemy_TargetAttack.isTarget)
            {
                enemy_TargetAttack.Target(other);
            }
        }
    }
}
