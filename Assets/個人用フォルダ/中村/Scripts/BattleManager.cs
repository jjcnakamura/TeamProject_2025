using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦闘シーンの管理用
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

        //デバッグ用　初期キャラ3体をロード
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
        Debug.Log("ダメージ");

        if (playerHp < 0)
        {
            isMainGame = false;
            isGameOver = true;

            Debug.Log("ゲームオーバー");
        }
    }
}
