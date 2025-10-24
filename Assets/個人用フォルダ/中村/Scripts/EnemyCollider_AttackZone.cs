using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider_AttackZone : MonoBehaviour
{
    [SerializeField] Enemy_TargetAttack enemy_TargetAttack;
    [SerializeField] Enemy_StatusChange enemy_StatusChange;

    void OnTriggerStay(Collider other)
    {
        //�˒��ɓ��������j�b�g�G���A�̃��j�b�g���^�[�Q�b�g����G�̏���
        if (enemy_TargetAttack != null && other != enemy_TargetAttack.col_Body)
        {
            //���j�b�g���U���̎˒����ɓ������ꍇ
            if (other.transform.tag == "Unit" && enemy_TargetAttack.attackUnitZone && !enemy_TargetAttack.isTarget)
            {
                enemy_TargetAttack.Target(other);
            }
        }

        //����G�Ƀo�t��������G�̏���
        if (enemy_StatusChange != null && other != enemy_StatusChange.col_Body)
        {
            //�G���o�t�̎˒����ɓ������ꍇ
            if (other.transform.tag == "Enemy")
            {
                enemy_StatusChange.Buff(other, true); ;
            }
        }

        //����̃��j�b�g�Ƀf�o�t��������G�̏���
        if (enemy_StatusChange != null && other != enemy_StatusChange.col_Body)
        {
            //���j�b�g���f�o�t�̎˒����ɓ������ꍇ
            if (other.transform.tag == "Unit" || other.transform.tag == "Unit_Wall")
            {
                enemy_StatusChange.Debuff(other, true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //�˒��ɓ��������j�b�g�G���A�̃��j�b�g���^�[�Q�b�g����G�̏���
        if (enemy_TargetAttack != null && other != enemy_TargetAttack.col_Body)
        {
            //���j�b�g���U���̎˒��O�ɏo���ꍇ
            if (other.transform.tag == "Unit" && enemy_TargetAttack.attackUnitZone && !enemy_TargetAttack.isTarget)
            {
                enemy_TargetAttack.Target(other);
            }
        }

        //����̓G�Ƀo�t��������G�̏���
        if (enemy_StatusChange != null && other != enemy_StatusChange.col_Body)
        {
            //�G���o�t�̎˒��O�ɏo���ꍇ
            if (other.transform.tag == "Enemy")
            {
                enemy_StatusChange.Buff(other, false); ;
            }
        }

        //����̃��j�b�g�Ƀf�o�t��������G�̏���
        if (enemy_StatusChange != null && other != enemy_StatusChange.col_Body)
        {
            //���j�b�g���f�o�t�̎˒��O�ɏo���ꍇ
            if (other.transform.tag == "Unit" || other.transform.tag == "Unit_Wall")
            {
                enemy_StatusChange.Debuff(other, false);
            }
        }
    }
}
