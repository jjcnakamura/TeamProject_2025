using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit_TargetAttack : BattleUnit_Base
{
    [Header("BattleUnit_TargetAttack")]

    //Collider
    public BoxCollider col_AttackZone;

    //�U���̑Ώۂɂ��Ă���G
    Collider targetEnemyCol;
    Enemy_Base targetEnemy;

    //�^�C�}�[
    float timer_Interval;

    //��Ԃ�\���t���O
    public bool isStart, isInterval;

    void Update()
    {
        //�ŏ��ɃX�e�[�W��ɔz�u���ꂽ�ꍇ��Collider�̃T�C�Y�����߂�
        if (!isStart && isBattle)
        {
            col_AttackZone.size = new Vector3(distance, col_Body.size.y, distance);
            isStart = true;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate(); //���N���X��FixedUpdate

        if (!isBattle || !BattleManager.Instance.isMainGame) return; //�퓬���łȂ��ꍇ�͖߂�

        Attack();
        Interval();
    }

    //�G���^�[�Q�b�g�A�^�[�Q�b�g����
    public void Target(Collider targetCol = null)
    {
        if (!isBattle || !BattleManager.Instance.isMainGame) return; //�퓬���łȂ��ꍇ�͖߂�

        if (!isTarget)
        {
            if (targetEnemyCol == null)
            {
                targetEnemyCol = targetCol;
                targetEnemy = targetCol.transform.parent.GetComponent<Enemy_Base>();

                isTarget = true;
                isRotation = true;
            }
        }
        else
        {
            if (targetCol == targetEnemyCol || targetCol == null)
            {
                targetEnemyCol = null;
                targetEnemy = null;

                isTarget = false;
                isRotation = false;
            }
        }
    }

    //�U��
    void Attack()
    {
        if (!isTarget || targetEnemy == null) return;

        //�_���G�̕���������
        Quaternion targetDir = Quaternion.LookRotation(targetEnemy.transform.position - transform.position);
        Quaternion lookDir = new Quaternion(transform.rotation.x, targetDir.y, transform.rotation.z, targetDir.w);
        DirectionChange(lookDir);

        if (!isInterval)
        {
            //�U�����ăC���^�[�o���J�n
            bool dead = targetEnemy.Damage(value);
            timer_Interval = 0;
            isInterval = true;

            //�G���_���[�W�Ŏ��S�����ꍇ�̓^�[�Q�b�g���~�߂�
            if (dead) Target();
        }
    }
    //�U���̃C���^�[�o��
    void Interval()
    {
        if (isInterval)
        {
            if (timer_Interval < interval)
            {
                timer_Interval += Time.fixedDeltaTime;
            }
            else
            {
                timer_Interval = 0;
                isInterval = false;
            }
        }
    }
}
