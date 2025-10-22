using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �e�G�̖��O�Ɛ���
/// </summary>
public class EnemiesData : Singleton<EnemiesData>
{
    public Status[] enemy;

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
        public string name;
        public string info;
        public int actId;
    }
}
