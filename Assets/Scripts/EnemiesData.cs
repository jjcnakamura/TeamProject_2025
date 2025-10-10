using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemiesData : Singleton<EnemiesData>
{
    [System.NonSerialized] public Status[] enemy = new Status[0];

    int index = -1;

    /// <summary>
    /// �����Ɋe�G�̖��O�Ɛ���������
    /// </summary>
    void EnemyInfoInit()
    {
        //��DPS

        index++;
        Array.Resize(ref enemy, enemy.Length + 1);
        enemy[index].name = "�ǎE����";             //�G�̖��O
        enemy[index].info = "�^���N�݂̂ɍU������"; //�s���̐���
        enemy[index].actId = 0;                     //�s����ID



        //��DPS
        //���^���N



        //���^���N
        //���T�|�[�g
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

        //�G�̏���������
        EnemyInfoInit();
    }

    //�X�e�[�^�X�̍\����
    public struct Status
    {
        public string name;
        public string info;
        public int actId;
    }
}
