using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �퓬�V�[���̊Ǘ��p
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    void Awake()
    {
        //�f�o�b�O�p�@�����L����3�̂����[�h
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(1);
        ParameterManager.Instance.AddUnit(2);
    }
}
