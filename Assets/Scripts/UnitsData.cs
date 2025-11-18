using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 各ユニットの初期パラメーター
/// </summary>
public class UnitsData : Singleton<UnitsData>
{
    public Status[] unit;
    public enum LvUpStatus
    {
        none,
        hp,
        value,
        interval,
        distance,
        range,
        targetNum
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
    }

    //ステータスの構造体
    [System.Serializable]
    public struct Status
    {
        public GameObject prefab;     //キャラのPrefab
        public Sprite sprite;         //キャラの画像
        public string name;           //キャラ名
        public int role;              //ロール　0がDPS、1がタンク、2がサポート
        public int cost;              //設置時のコスト
        public int upCost;            //同じユニットを複数置く場合のコスト増加量
        public int recast;            //再配置までの時間
        public int hp;                //耐久値（最大HP）
        public int value;             //DPSの場合は攻撃力、サポートの場合は回復量、ポイント増加量など
        public float interval;        //行動速度（攻撃、回復をする間隔）
        public float distance;        //攻撃、回復の射程
        public float range;           //範囲攻撃の範囲
        public int targetNum;         //攻撃、回復の対象に出来る数
        public LvUpStatus lVUPStatus; //レベルアップ時に上がるステータス
        public bool place_UnitZone;   //ユニット配置場所に配置できるか
        public bool place_Floor;      //敵が通る道に配置できるか
    }
}
