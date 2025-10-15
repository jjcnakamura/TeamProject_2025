using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �퓬�V�[���̊Ǘ��p
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    public GameObject playerSide;

    [Space(10)]

    int maxPlayerHp;
    public int playerHp;
    int maxPoint;
    public int point;

    public float timer_EnemySpawn { get; private set; }

    public bool isMainGame, isClear, isGameOver;

    void Start()
    {
        isMainGame = true;

        //�f�o�b�O�p�@�����L����3�̂����[�h
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(1);
        ParameterManager.Instance.AddUnit(2);

        maxPlayerHp = ParameterManager.Instance.hp;
        playerHp = maxPlayerHp;
        maxPoint = ParameterManager.Instance.point;
        point = maxPoint;
    }

    void FixedUpdate()
    {
        timer_EnemySpawn += Time.fixedDeltaTime;
    }

    public void Damage()
    {
        if (!isMainGame) return;

        playerHp--;
        Debug.Log("�_���[�W");

        if (playerHp < 0)
        {
            isMainGame = false;
            isGameOver = true;

            Debug.Log("�Q�[���I�[�o�[");
        }
    }
}
