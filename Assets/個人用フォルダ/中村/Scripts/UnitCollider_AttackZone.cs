using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCollider_AttackZone : MonoBehaviour
{
    [SerializeField] BattleUnit_TargetAttack unit_TargetAttack;

    void OnTriggerStay(Collider other)
    {
        //�˒��ɓ������G���^�[�Q�b�g���郆�j�b�g�̏���
        if (unit_TargetAttack != null)
        {
            //�G���U���̎˒����ɓ������ꍇ
            if (other.transform.tag == "Enemy" && !unit_TargetAttack.isTarget)
            {
                unit_TargetAttack.Target(other);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //�˒��ɓ������G���^�[�Q�b�g���郆�j�b�g�̏���
        if (unit_TargetAttack != null)
        {
            //�G���U���̎˒��O�ɏo���ꍇ
            if (other.transform.tag == "Enemy" && unit_TargetAttack.isTarget)
            {
                unit_TargetAttack.Target(other);
            }
        }    
    }
}
