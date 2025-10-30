using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TargetAttack : Enemy_Base
{
    [Header("Enemy_TargetAttack")]

    //Collider
    public BoxCollider col_AttackZone;
    public BoxCollider col_AttackZone_Wall;

    [Space(10)]

    [SerializeField,Label("�ǖ���_��")] public bool attackWall;
    [SerializeField,Label("���j�b�g�G���A��_��")] public bool attackUnitZone;

    //�U���̑Ώۂɂ��Ă��郆�j�b�g
    Collider targetUnitCol;
    BattleUnit_Base targetUnit;

    //�^�C�}�[
    float timer_Interval;

    [Space(10)]

    //��Ԃ�\���t���O
    public bool isInterval;

    protected override void Start()
    {
        base.Start(); //���N���X��Start

        //Collider�̈ʒu�ƃT�C�Y�����߂�
        col_AttackZone.size = new Vector3(distance, col_Body.size.y, distance);
        col_AttackZone_Wall.center = new Vector3(0f, 0f, 1f);
        col_AttackZone_Wall.size = new Vector3(1f, col_AttackZone.size.y, 1f);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate(); //���N���X��FixedUpdate

        if (!BattleManager.Instance.isMainGame) return; //���C���Q�[�����łȂ���Ζ߂�

        Attack();
        Interval();
    }

    //���j�b�g���^�[�Q�b�g�A�^�[�Q�b�g����
    public void Target(Collider targetCol = null)
    {
        if (!isTarget)
        {
            if (targetUnitCol == null)
            {
                targetUnitCol = targetCol;
                targetUnit = targetCol.transform.parent.GetComponent<BattleUnit_Base>();

                //�_�����j�b�g�̕���������
                Quaternion targetDir = Quaternion.LookRotation(targetUnit.transform.position - transform.position);
                Quaternion lookDir = new Quaternion(transform.rotation.x, targetDir.y, transform.rotation.z, targetDir.w);
                DirectionChange(lookDir);

                isTarget = true;
                isMove = false;
            }
        }
        else
        {
            if (targetCol == targetUnitCol || targetCol == null)
            {
                targetUnitCol = null;
                targetUnit = null;

                //�_�����j�b�g�̕������O��
                Quaternion targetDir = Quaternion.LookRotation(spawnPoint.routePoint[routeIndex].pos[currentRoute] - transform.position);
                Quaternion lookDir = new Quaternion(transform.rotation.x, targetDir.y, transform.rotation.z, targetDir.w);
                DirectionChange(lookDir);

                isTarget = false;
                isMove = true;
            }
        }
    }
    //�U��
    void Attack()
    {
        if (!isTarget) return;

        if (targetUnit != null)
        {
            if (!isInterval)
            {
                //�U��
                bool dead = targetUnit.Damage(value);
                //�G�t�F�N�g���U�����j�b�g�̈ʒu�ɐ���
                Instantiate(effect).transform.position = targetUnit.transform.position;
                //�C���^�[�o���J�n
                timer_Interval = 0;
                isInterval = true;

                //���j�b�g���_���[�W�Ŏ��S�����ꍇ�̓^�[�Q�b�g���~�߂�
                if (dead) Target();
            }
        }
        else
        {
            //���j�b�g�����݂��Ȃ��ꍇ�̓^�[�Q�b�g���~�߂�
            Target();
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
