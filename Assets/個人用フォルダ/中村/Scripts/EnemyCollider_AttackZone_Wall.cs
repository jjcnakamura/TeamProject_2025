using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider_AttackZone_Wall : MonoBehaviour
{
    [SerializeField] Enemy_TargetAttack enemy_TargetAttack;
    [SerializeField] Enemy_StatusChange enemy_StatusChange;

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

        //�X�e�[�^�X��ω������郆�j�b�g�̏���
        if (enemy_StatusChange != null)
        {
            //�ǃ��j�b�g���O���ɗ����ꍇ
            if (other.transform.tag == "Unit_Wall" && !enemy_StatusChange.isCollisionWallUnit)
            {
                enemy_StatusChange.CollisionWallUnit(other);
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

        //�X�e�[�^�X��ω������郆�j�b�g�̏���
        if (enemy_StatusChange != null)
        {
            //�ǃ��j�b�g���O���ɗ����ꍇ
            if (other.transform.tag == "Unit_Wall" && enemy_StatusChange.isCollisionWallUnit)
            {
                enemy_StatusChange.CollisionWallUnit(other);
            }
        }
    }
}
