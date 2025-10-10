using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �e���j�b�g�̏����p�����[�^�[
/// </summary>
public class UnitsData : Singleton<UnitsData>
{
    [System.NonSerialized] public Status[] unit = new Status[0];

    int index = -1;

    /// <summary>
    /// �����Ɋe���j�b�g�̏����X�e�[�^�X������
    /// </summary>
    void StatusInit()
    {
        //��DPS

        index++;
        Array.Resize(ref unit, unit.Length + 1);
        unit[index].name = "�������}��"; //�L������
        unit[index].role = 0;            //���[���@0��DPS�A1���^���N�A2���T�|�[�g
        unit[index].cost = 16;           //�ݒu���̃R�X�g
        unit[index].recast = 20;         //�Ĕz�u�܂ł̎���
        unit[index].hp = 10;             //�ϋv�l�i�ő�HP�j
        unit[index].value = 4;           //DPS�̏ꍇ�͍U���́A�T�|�[�g�̏ꍇ�͉񕜗ʁA�|�C���g�����ʂȂ�
        unit[index].interval = 2;        //�s�����x�i�U���A�񕜂�����Ԋu�j
        unit[index].distance = 10;       //�U���A�񕜂̎˒�
        unit[index].range = 0;           //�͈͍U���͈̔�
        unit[index].placeRoad = false;   //�G���ʂ铹�ɔz�u�ł��邩
        unit[index].lvUpStatus = "distance"; //���x���A�b�v���ɏオ��X�e�[�^�X�i�ϐ���������j

        //��DPS
        //���^���N

        index++;
        Array.Resize(ref unit, unit.Length + 1);
        unit[index].name = "�U���ǃ}��";
        unit[index].role = 1;
        unit[index].cost = 10;
        unit[index].recast = 20;
        unit[index].hp = 50;
        unit[index].value = 1;
        unit[index].interval = 1;
        unit[index].distance = 3;
        unit[index].range = 0;
        unit[index].placeRoad = true;
        unit[index].lvUpStatus = "hp";

        //���^���N
        //���T�|�[�g

        index++;
        Array.Resize(ref unit, unit.Length + 1);
        unit[index].name = "�|�C���g�����}��";
        unit[index].role = 2;
        unit[index].cost = 6;
        unit[index].recast = 15;
        unit[index].hp = 20;
        unit[index].value = 6;
        unit[index].interval = 10;
        unit[index].distance = 0;
        unit[index].range = 0;
        unit[index].placeRoad = false;
        unit[index].lvUpStatus = "";
    }

    void Awake()
    {
        //�V�[����J�ڂ��Ă��c��
        if (gameObject.transform.parent != null) gameObject.transform.parent = null;
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        //���j�b�g�̃X�e�[�^�X��������
        StatusInit();
    }

    //�X�e�[�^�X�̍\����
    public struct Status
    {
        public string name;
        public int role;
        public int cost;
        public int recast;
        public int hp;
        public int value;
        public float interval;
        public float distance;
        public float range;
        public bool placeRoad;
        public string lvUpStatus;
    }
}
