using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�Ə������j�b�g�̃p�����[�^�[���Ǘ�����
/// </summary>
public class PlayerManager : Singleton<PlayerManager>
{
    //�v���C���[�̃X�e�[�^�X
    public int point;                   //�����|�C���g��
    public int maxInstallation;         //���j�b�g�ő�z�u��
    public int sameUnitMaxInstallation; //�������j�b�g�̍ő�z�u��
    public int maxPossession;           //�ő僆�j�b�g������

    //�������Ă��郆�j�b�g�̃X�e�[�^�X
    public UnitStatus[] unitStatus;

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
}
