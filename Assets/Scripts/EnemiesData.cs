using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemiesData : Singleton<EnemiesData>
{
    [System.NonSerialized] public Status[] enemy = new Status[0];

    int index = -1;

    /// <summary>
    /// ここに各敵の名前と説明を書く
    /// </summary>
    void EnemyInfoInit()
    {
        //↓DPS

        index++;
        Array.Resize(ref enemy, enemy.Length + 1);
        enemy[index].name = "壁殺すよ";             //敵の名前
        enemy[index].info = "タンクのみに攻撃する"; //行動の説明
        enemy[index].actId = 0;                     //行動のID



        //↑DPS
        //↓タンク



        //↑タンク
        //↓サポート
    }

    void Awake()
    {
        //シーンを遷移しても残る
        if (gameObject.transform.parent != null) gameObject.transform.parent = null;
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        //敵の情報を初期化
        EnemyInfoInit();
    }

    //ステータスの構造体
    public struct Status
    {
        public string name;
        public string info;
        public int actId;
    }
}
