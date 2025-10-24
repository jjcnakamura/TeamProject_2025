using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TargetAttack : Enemy_Base
{
    [Header("Enemy_TargetAttack")]

    //Collider
    public BoxCollider col_AttackZone;

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

        //Collider�̃T�C�Y�����߂�
        col_AttackZone.size = new Vector3(distance, col_Body.size.y, distance);

        //�ǖ����j�b�g��_���p��Collider�𐶐�����
        if (attackWall) GenerateAttackWallCollider();
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
                //�U�����ăC���^�[�o���J�n
                bool dead = targetUnit.Damage(value);
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

    //�ǖ����j�b�g��_���p��Collider�𐶐�����
    void GenerateAttackWallCollider()
    {
        GameObject atkWallObj = new GameObject();
        atkWallObj.name = "Col_AttackZone_Wall";

        atkWallObj.transform.SetParent(transform);
        atkWallObj.transform.position = col_AttackZone.transform.position;
        atkWallObj.transform.rotation = new Quaternion();
        atkWallObj.transform.localScale = new Vector3(1f, 1f, 1f);

        BoxCollider atkWallCol = atkWallObj.AddComponent<BoxCollider>();
        atkWallCol.isTrigger = true;
        atkWallCol.center = new Vector3(0f, 0f, 1f);
        atkWallCol.size = new Vector3(1f, col_AttackZone.size.y, 1f);

        EnemyCollider_AttackZone_Wall atkWallColComponent = atkWallObj.AddComponent<EnemyCollider_AttackZone_Wall>();
        atkWallColComponent.enemy_TargetAttack = this;
    }
}
