using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_StatusChange : Enemy_Base
{
    [Header("Enemy_StatusChange")]

    [SerializeField,Label("�o�t��������ifalse���ƃf�o�t�j")] public bool buff;

    //Collider
    public BoxCollider col_AttackZone;

    //�X�e�[�^�X��ω������Ă���G�A���j�b�g�̃R���|�[�l���g
    List<Enemy_Base> buffEnemy = new List<Enemy_Base>();
    List<BattleUnit_Base> debuffUnit = new List<BattleUnit_Base>();

    public bool isDeadCheck_StatusChange;

    protected override void Start()
    {
        base.Start(); //���N���X��Start

        //Collider�̃T�C�Y�����߂�
        col_AttackZone.size = new Vector3(distance, col_Body.size.y, distance);

        //���N���X�Ŏ��S�̔�������Ȃ�
        isDeadCheck = false;
        isDeadCheck_StatusChange = true;
    }

    protected override void Update()
    {
        base.Update(); //���N���X��Update

        DeadCheck_StatusChange();
    }

    //�G�Ƀo�t��������
    public void Buff(Collider targetCol, bool flag)
    {
        Enemy_Base targetEnemy = targetCol.transform.parent.GetComponent<Enemy_Base>();
        if (targetEnemy == null) return;

        if (flag)
        {
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
        BattleUnit_Base targetUnit = targetCol.transform.parent.GetComponent<BattleUnit_Base>();
        if (targetUnit == null) return;

        if (flag)
        {
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

    //���S���Ă���ꍇ�̓X�e�[�^�X�̕ω�������
    void DeadCheck_StatusChange()
    {
        if (!isDeadCheck_StatusChange) return;

        if (isDead)
        {
            //�o�t������
            for (int i = debuffUnit.Count - 1; i >= 0; i--)
            {
                buffEnemy[i].StatusChange(defaultValue, false);
            }
            //�f�o�t������
            for (int i = debuffUnit.Count - 1; i >= 0; i--)
            {
                debuffUnit[i].StatusChange(-defaultValue, false);
            }

            isDead = true;
            isDeadCheck = true;
            isDeadCheck_StatusChange = false;
        }
    }
}
