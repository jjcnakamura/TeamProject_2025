using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 各ユニットの初期パラメーター
/// </summary>
public class UnitsData : Singleton<UnitsData>
{
    public Sprite[] iconBackSprite;    //ユニットのアイコンの背景

    [Space(10)]

    public int[] levelUpExp;           //各レベルごとの必要経験値(レベル1なら配列の1番目)

    [Space(10)]

    public int maxUnitPossession = 5;  //ユニットの所持数の最大

    [Space(10)]

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
        public GameObject prefab;          //キャラのPrefab
        public GameObject model;           //キャラのモデル
        public Sprite sprite;              //キャラの画像
        public string name;                //キャラ名
        public int se_Place;               //設置時のSE
        public int[] se_Action;            //戦闘中の行動時のSE
        public string anim_Name;           //戦闘中の行動時のアニメーション名
        public float anim_Time;             //アニメーションから実際の行動が起こるまでの時間

        [Space(5)]

        public int role;                   //ロール　0がDPS、1がタンク、2がサポート
        public int cost;                   //設置時のコスト
        public int upCost;                 //同じユニットを複数置く場合のコスト増加量
        public int recast;                 //再配置までの時間
        public int hp;                     //耐久値（最大HP）
        public int value;                  //DPSの場合は攻撃力、サポートの場合は回復量、ポイント増加量など
        public float interval;             //行動速度（攻撃、回復をする間隔）
        public float distance;             //攻撃、回復の射程
        public float range;                //範囲攻撃の範囲
        public int targetNum;              //攻撃、回復の対象に出来る数
        public LvUpStatus lVUPStatus;      //レベルアップ時に上がるステータス

        [Space(5)]

        public bool place_UnitZone;        //ユニット配置場所に配置できるか
        public bool place_Floor;           //敵が通る道に配置できるか

        [Space(5)]

        [Multiline(3)] public string info; //キャラの説明文
        public string valueName;           //Valueのステータス画面での名前
        public ViewStatus viewStatus;      //ステータス画面で表示するステータス
    }

    //ステータス画面で表示するステータス用の構造体
    [System.Serializable]
    public struct ViewStatus
    {
        public bool hp;
        public bool value;
        public bool interval;
        public bool distance;
        public bool range;
        public bool targetNum;
    }
}
