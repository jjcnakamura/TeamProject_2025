using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 各ユニットの初期パラメーター
/// </summary>
public class UnitsData : Singleton<UnitsData>
{
    [System.NonSerialized] public Status[] unit = new Status[0];

    int index = -1;

    /// <summary>
    /// ここに各ユニットの初期ステータスを書く
    /// </summary>
    void StatusInit()
    {
        //↓DPS

        index++;
        Array.Resize(ref unit, unit.Length + 1);
        unit[index].name = "遠距離マン"; //キャラ名
        unit[index].role = 0;            //ロール　0がDPS、1がタンク、2がサポート
        unit[index].cost = 16;           //設置時のコスト
        unit[index].recast = 20;         //再配置までの時間
        unit[index].hp = 10;             //耐久値（最大HP）
        unit[index].value = 4;           //DPSの場合は攻撃力、サポートの場合は回復量、ポイント増加量など
        unit[index].interval = 2;        //行動速度（攻撃、回復をする間隔）
        unit[index].distance = 10;       //攻撃、回復の射程
        unit[index].range = 0;           //範囲攻撃の範囲
        unit[index].placeRoad = false;   //敵が通る道に配置できるか
        unit[index].lvUpStatus = "distance"; //レベルアップ時に上がるステータス（変数名を入れる）

        //↑DPS
        //↓タンク

        index++;
        Array.Resize(ref unit, unit.Length + 1);
        unit[index].name = "攻撃壁マン";
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

        //↑タンク
        //↓サポート

        index++;
        Array.Resize(ref unit, unit.Length + 1);
        unit[index].name = "ポイント増加マン";
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
        //シーンを遷移しても残る
        if (gameObject.transform.parent != null) gameObject.transform.parent = null;
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        //ユニットのステータスを初期化
        StatusInit();
    }

    //ステータスの構造体
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
