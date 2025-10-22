using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �e���j�b�g�̏����p�����[�^�[
/// </summary>
public class UnitsData : Singleton<UnitsData>
{
    public Status[] unit;

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
    }

    //�X�e�[�^�X�̍\����
    [System.Serializable]
    public struct Status
    {
        public GameObject prefab;   //�L������Prefab
        public Sprite sprite;       //�L�����̉摜
        public string name;         //�L������
        public int role;            //���[���@0��DPS�A1���^���N�A2���T�|�[�g
        public int cost;            //�ݒu���̃R�X�g
        public int recast;          //�Ĕz�u�܂ł̎���
        public int hp;              //�ϋv�l�i�ő�HP�j
        public int value;           //DPS�̏ꍇ�͍U���́A�T�|�[�g�̏ꍇ�͉񕜗ʁA�|�C���g�����ʂȂ�
        public float interval;      //�s�����x�i�U���A�񕜂�����Ԋu�j
        public float distance;      //�U���A�񕜂̎˒�
        public float range;         //�͈͍U���͈̔�
        public bool place_UnitZone; //���j�b�g�z�u�ꏊ�ɔz�u�ł��邩
        public bool place_Floor;    //�G���ʂ铹�ɔz�u�ł��邩
        public string lvUpStatus;   //���x���A�b�v���ɏオ��X�e�[�^�X�i�ϐ���������j
    }
}
