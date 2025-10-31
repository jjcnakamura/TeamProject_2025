using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_StatusChange : Enemy_Base
{
    [Header("Enemy_StatusChange")]

    [SerializeField,Label("�o�t��������ifalse���ƃf�o�t�j")] public bool buff;

    //Collider
    public CapsuleCollider col_AttackZone;
    public BoxCollider col_AttackZone_Wall;
    [System.NonSerialized] MeshRenderer mesh_AttackZone;

    //�X�e�[�^�X��ω������Ă���G�A���j�b�g�̃R���|�[�l���g
    List<Enemy_Base> buffEnemy = new List<Enemy_Base>();
    List<BattleUnit_Base> debuffUnit = new List<BattleUnit_Base>();

    //�O���ɂ���ǃ��j�b�g��Collider
    Collider wallUnitCol;

    //��Ԃ�\���t���O
    public bool isCollisionWallUnit, isDeadCheck_StatusChange;

    protected override void Start()
    {
        base.Start(); //���N���X��Start

        //Collider�̈ʒu�ƃT�C�Y�����߂�
        col_AttackZone.transform.localScale = new Vector3(distance, col_AttackZone.transform.localScale.y, distance);
        mesh_AttackZone = col_AttackZone.GetComponent<MeshRenderer>();
        mesh_AttackZone.enabled = false;

        col_AttackZone_Wall.center = new Vector3(0f, 0f, 1f);
        col_AttackZone_Wall.size = new Vector3(1f, col_Body.size.y, 1f);

        //���N���X�Ŏ��S�̔�������Ȃ�
        isDeadCheck = false;
        isDeadCheck_StatusChange = true;
    }

    protected override void Update()
    {
        base.Update(); //���N���X��Update

        WallUnitCollisionCheck();
        DeadCheck_StatusChange();
    }

    //�G�Ƀo�t��������
    public void Buff(Collider targetCol, bool flag)
    {
        if (!buff) return;

        Enemy_Base targetEnemy = targetCol.transform.parent.GetComponent<Enemy_Base>();
        if (targetEnemy == null) return;

        if (flag)
        {
            for (int i = buffEnemy.Count - 1; i >= 0; i--)
            {
                if (buffEnemy[i] == targetEnemy)
                {
                    return;
                }
            }

            targetEnemy.StatusChange(defaultValue, true);
            buffEnemy.Add(targetEnemy);
        }
        else
        {
            targetEnemy.StatusChange(defaultValue, false);

            for (int i = buffEnemy.Count - 1; i >= 0; i--)
            {
                if (buffEnemy[i] == targetEnemy)
                {
                    buffEnemy.RemoveAt(i);
                    break;
                }
            }
        }
    }
    //���j�b�g�Ƀf�o�t��������
    public void Debuff(Collider targetCol, bool flag)
    {
        if (buff) return;

        BattleUnit_Base targetUnit = targetCol.transform.parent.GetComponent<BattleUnit_Base>();
        if (targetUnit == null) return;

        if (flag)
        {
            for (int i = debuffUnit.Count - 1; i >= 0; i--)
            {
                if (debuffUnit[i] == targetUnit)
                {
                    return;
                }
            }

            targetUnit.StatusChange(-defaultValue, true);
            debuffUnit.Add(targetUnit);
        }
        else
        {
            targetUnit.StatusChange(-defaultValue, false);

            for (int i = debuffUnit.Count - 1; i >= 0; i--)
            {
                if (debuffUnit[i] == targetUnit)
                {
                    debuffUnit.RemoveAt(i);
                    break;
                }
            }
        }
    }
    //�ǃ��j�b�g�̏Փ˔���
    public void CollisionWallUnit(Collider col = null)
    {
        if (!isCollisionWallUnit)
        {
            if (wallUnitCol == null)
            {
                wallUnitCol = col;

                isCollisionWallUnit = true;
                isMove = false;
            }
        }
        else
        {
            if (col == wallUnitCol || col == null || wallUnitCol)
            {
                wallUnitCol = null;

                isCollisionWallUnit = false;
                isMove = true;
            }
        }
    }

    //�O���ɂ���ǃ��j�b�g�����S���������肷��
    void WallUnitCollisionCheck()
    {
        if (!isCollisionWallUnit) return;

        if (wallUnitCol == null)
        {
            isCollisionWallUnit = false;
            isMove = true;
        }
    }
    //���S���Ă���ꍇ�̓X�e�[�^�X�̕ω�������
    void DeadCheck_StatusChange()
    {
        if (!isDeadCheck_StatusChange) return;

        if (isDead)
        {
            if (buff)
            {
                //�o�t������
                for (int i = buffEnemy.Count - 1; i >= 0; i--)
                {
                    buffEnemy[i].StatusChange(defaultValue, false);
                }

            }
            else
            {
                //�f�o�t������
                for (int i = debuffUnit.Count - 1; i >= 0; i--)
                {
                    debuffUnit[i].StatusChange(-defaultValue, false);
                }
            }

            isDead = true;
            isDeadCheck = true;
            isDeadCheck_StatusChange = false;
        }
    }
}
