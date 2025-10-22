using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �v���C���[�Ə������j�b�g�̃p�����[�^�[���Ǘ�����
/// </summary>
public class ParameterManager : Singleton<ParameterManager>
{
    //�v���C���[�̃X�e�[�^�X�Ə����l
    public int hp = 10;                     //�v���C���[�i�^���[�j��HP

    public int maxUnitPossession = 3;       //�ő僆�j�b�g������

    public int point = 6;                   //�����|�C���g��
    public int maxInstallation = 4;         //���j�b�g�ő�z�u��
    public int sameUnitMaxInstallation = 1; //�������j�b�g�̍ő�z�u��

    [Space(10)]

    //�������Ă��郆�j�b�g���Ƃ̃X�e�[�^�X
    public UnitStatus[] unitStatus;
    public int[] battleUnitId;      //�퓬�Ŏg�p���郆�j�b�g�̔ԍ�

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

        //���j�b�g�ƓG�̃f�[�^�ǂݍ��ݗp�I�u�W�F�N�g�𐶐�
        //GameObject loadData = new GameObject();
        //loadData.name = "LoadData";
        //loadData.AddComponent<UnitsData>();
        //loadData.AddComponent<EnemiesData>();
        //loadData.transform.SetParent(transform);
    }

    /// <summary>
    /// ���j�b�g��V�������肷��@�����Ń��j�b�g��ID���w��
    /// </summary>
    public void AddUnit(int id)
    {
        //�ő僆�j�b�g�������ɒB���Ă���ꍇ�͑��₹�Ȃ�
        if (unitStatus.Length >= maxUnitPossession) return;

        int index = unitStatus.Length;
        Array.Resize(ref unitStatus, index + 1);
        unitStatus[index] = new UnitStatus();

        //ID�ɑΉ��������j�b�g�̃X�e�[�^�X��ǂݍ���
        unitStatus[index].prefab = UnitsData.Instance.unit[id].prefab;     //�L������Prefab
        unitStatus[index].sprite = UnitsData.Instance.unit[id].sprite;     //�L�����̉摜

        unitStatus[index].id = id;                                         //�ǂ̃��j�b�g��������ID
        unitStatus[index].role = UnitsData.Instance.unit[id].role;         //���[���@0��DPS�A1���^���N�A2���T�|�[�g

        unitStatus[index].lv = 1;                                          //���x��
        unitStatus[index].exp = 0;                                         //�����o���l

        unitStatus[index].cost = UnitsData.Instance.unit[id].cost;         //�ݒu���̃R�X�g
        unitStatus[index].recast = UnitsData.Instance.unit[id].recast;     //�Ĕz�u�܂ł̎���

        unitStatus[index].hp = UnitsData.Instance.unit[id].hp;             //�ϋv�l�i�ő�HP�j
        unitStatus[index].value = UnitsData.Instance.unit[id].value;       //DPS�̏ꍇ�͍U���́A�T�|�[�g�̏ꍇ�͉񕜗ʁA�|�C���g�����ʂȂ�
        unitStatus[index].interval = UnitsData.Instance.unit[id].interval; //�s�����x�i�U���A�񕜂�����Ԋu�j
        unitStatus[index].distance = UnitsData.Instance.unit[id].distance; //�U���A�񕜂̎˒�
        unitStatus[index].range = UnitsData.Instance.unit[id].range;       //�͈͍U���͈̔�

        unitStatus[index].place_UnitZone = UnitsData.Instance.unit[id].place_UnitZone; //���j�b�g�̔z�u�ꏊ�ɒu���邩
        unitStatus[index].place_Floor = UnitsData.Instance.unit[id].place_Floor;       //�G�̒ʂ蓹�ɒu���邩
    }

    /// <summary>
    /// �Q�[�����ő�������e���j�b�g�̃p�����[�^
    /// </summary>
    [System.Serializable]
    public struct UnitStatus
    {
        public GameObject prefab;   //�L������Prefab
        public Sprite sprite;       //�L�����̉摜

        public int id;              //�ǂ̃��j�b�g��������ID
        public int role;            //���[���@0��DPS�A1���^���N�A2���T�|�[�g

        public int lv;              //���x��
        public int exp;             //�����o���l

        public int cost;            //�ݒu���̃R�X�g
        public int recast;          //�Ĕz�u�܂ł̎���

        public int hp;              //�ϋv�l�i�ő�HP�j
        public int value;           //DPS�̏ꍇ�͍U���́A�T�|�[�g�̏ꍇ�͉񕜗ʁA�|�C���g�����ʂȂ�
        public float interval;      //�s�����x�i�U���A�񕜂�����Ԋu�j
        public float distance;      //�U���A�񕜂̎˒�
        public float range;         //�͈͍U���͈̔�

        public bool place_UnitZone; //���j�b�g�̔z�u�ꏊ�ɒu���邩
        public bool place_Floor;    //�G�̒ʂ蓹�ɒu���邩
    }
}
