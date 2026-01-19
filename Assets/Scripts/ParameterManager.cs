using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// プレイヤーと所持ユニットのパラメーターを管理する
/// </summary>
public class ParameterManager : Singleton<ParameterManager>
{
    //フロア(エリア)数
    public int floorNum;

    [Space(10)]

    //プレイヤーのステータスと初期値
    public int hp = 10;                     //プレイヤー（タワー）のHP
    public int maxHp = 100;                 //プレイヤー（タワー）の最大HP

    public int maxUnitPossession = 5;       //最大ユニット所持数

    public int point = 6;                   //初期ポイント数
    public int maxInstallation = 10;        //ユニット最大配置数
    public int sameUnitMaxInstallation = 1; //同じユニットの最大配置数

    [Space(10)]

    //所持しているユニットごとのステータス
    public UnitStatus[] unitStatus;
    public int[] battleUnitId;      //戦闘で使用するユニットの番号

    //ステージクリア後に使用する変数
    public int getExp;              //獲得した経験値

    //マップシーン用のフラグ
    [System.NonSerialized] public bool isBattleClear;

    //戦闘シーン用のフラグ
    [System.NonSerialized] public bool isSpeedUp;

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

    /// <summary>
    /// 全てのステータスを初期化する
    /// </summary>
    public void StatusInit()
    {
        //フロア(エリア)数
        floorNum = 0;

        //プレイヤーのステータス
        hp = 10;                     //プレイヤー（タワー）のHP
        maxHp = 100;                 //プレイヤー（タワー）の最大HP

        maxUnitPossession = 5;       //最大ユニット所持数

        point = 6;                   //初期ポイント数
        maxInstallation = 10;        //ユニット最大配置数
        sameUnitMaxInstallation = 1; //同じユニットの最大配置数

        //ユニットのステータス
        Array.Resize(ref unitStatus, 0);
        Array.Resize(ref battleUnitId, 0);

        //獲得した経験値
        getExp = 0;
    }

    /// <summary>
    /// ユニットを新しく入手する　引数でユニットのIDを指定
    /// </summary>
    public void AddUnit(int id)
    {
        //最大ユニット所持数に達している場合は増やせない
        if (unitStatus.Length >= maxUnitPossession) return;

        int index = unitStatus.Length;
        Array.Resize(ref unitStatus, index + 1);
        unitStatus[index] = new UnitStatus();

        //IDに対応したユニットのステータスを読み込み
        unitStatus[index].name = UnitsData.Instance.unit[id].name;                    //ユニット名
        unitStatus[index].prefab = UnitsData.Instance.unit[id].prefab;                //ユニットのPrefab
        unitStatus[index].sprite = UnitsData.Instance.unit[id].sprite;                //ユニットの画像
        unitStatus[index].id = id;                                                    //どのユニットかを示すID
        unitStatus[index].se_Place = UnitsData.Instance.unit[id].se_Place;            //設置時のSE
        unitStatus[index].se_Action = UnitsData.Instance.unit[id].se_Action;          //戦闘中の行動時のSE
        unitStatus[index].anim_Name = UnitsData.Instance.unit[id].anim_Name;          //戦闘中の行動時のアニメーション名
        unitStatus[index].anim_Time = UnitsData.Instance.unit[id].anim_Time;          //アニメーションから実際の行動が起こるまでの時間

        unitStatus[index].lv = 1;                                                      //レベル
        unitStatus[index].exp = 0;                                                     //所持経験値

        unitStatus[index].role = UnitsData.Instance.unit[id].role;                     //ロール　0がDPS、1がタンク、2がサポート
        unitStatus[index].cost = UnitsData.Instance.unit[id].cost;                     //設置時のコスト
        unitStatus[index].upCost = UnitsData.Instance.unit[id].upCost;                 //同じユニットを複数置く場合のコスト増加量
        unitStatus[index].recast = UnitsData.Instance.unit[id].recast;                 //再配置までの時間
        unitStatus[index].hp = UnitsData.Instance.unit[id].hp;                         //耐久値（最大HP）
        unitStatus[index].value = UnitsData.Instance.unit[id].value;                   //DPSの場合は攻撃力、サポートの場合は回復量、ポイント増加量など
        unitStatus[index].interval = UnitsData.Instance.unit[id].interval;             //行動速度（攻撃、回復をする間隔）
        unitStatus[index].distance = UnitsData.Instance.unit[id].distance;             //攻撃、回復の射程
        unitStatus[index].range = UnitsData.Instance.unit[id].range;                   //範囲攻撃の範囲
        unitStatus[index].targetNum = UnitsData.Instance.unit[id].targetNum;           //攻撃、回復の対象に出来る数
        unitStatus[index].lvUpStatus = UnitsData.Instance.unit[id].lVUPStatus;         //レベルアップ時に上がるステータス

        unitStatus[index].place_UnitZone = UnitsData.Instance.unit[id].place_UnitZone; //ユニットの配置場所に置けるか
        unitStatus[index].place_Floor = UnitsData.Instance.unit[id].place_Floor;       //敵の通り道に置けるか
    }

    /// <summary>
    /// ユニットのレベルアップ　引数でユニットの要素番号を指定
    /// </summary>
    public void LevelUp(int unitIndex)
    {
        //全キャラ共通で成長するステータス
        unitStatus[unitIndex].lv += 1;
        unitStatus[unitIndex].hp += 3;
        unitStatus[unitIndex].value += 1;

        //キャラごとに違う成長ステータス
        if (unitStatus[unitIndex].lvUpStatus != UnitsData.LvUpStatus.none)
        {
            //耐久値（最大HP）
            if (unitStatus[unitIndex].lvUpStatus == UnitsData.LvUpStatus.hp)
            {
                unitStatus[unitIndex].hp += 4;
            }
            //DPSの場合は攻撃力、サポートの場合は回復量、ポイント増加量など
            else if (unitStatus[unitIndex].lvUpStatus == UnitsData.LvUpStatus.value)
            {
                unitStatus[unitIndex].value += 2;
            }
            //行動速度（攻撃、回復をする間隔）
            else if (unitStatus[unitIndex].lvUpStatus == UnitsData.LvUpStatus.interval)
            {
                unitStatus[unitIndex].interval = Mathf.Max(unitStatus[unitIndex].interval - 0.25f, 0);
            }
            //攻撃、回復の射程
            else if (unitStatus[unitIndex].lvUpStatus == UnitsData.LvUpStatus.distance)
            {
                unitStatus[unitIndex].distance += 0.5f;
            }
            //範囲攻撃の範囲
            else if (unitStatus[unitIndex].lvUpStatus == UnitsData.LvUpStatus.range)
            {
                unitStatus[unitIndex].range += 0.25f;
            }
            //攻撃、回復の対象に出来る数
            else if (unitStatus[unitIndex].lvUpStatus == UnitsData.LvUpStatus.targetNum)
            {
                unitStatus[unitIndex].targetNum += 1;
            }
        }
    }

    /// <summary>
    /// ゲーム内で増減する各ユニットのパラメータ
    /// </summary>
    [System.Serializable]
    public struct UnitStatus
    {
        public GameObject prefab;   //ユニットのPrefab
        public Sprite sprite;       //ユニットの画像
        public string name;         //ユニット名
        public int id;              //どのユニットかを示すID
        public int se_Place;        //設置時のSE
        public int[] se_Action;     //戦闘中の行動時のSE
        public string anim_Name;    //戦闘中の行動時のアニメーション名
        public float anim_Time;     //アニメーションから実際の行動が起こるまでの時間

        public int lv;              //レベル
        public int exp;             //所持経験値

        public int role;            //ロール　0がDPS、1がタンク、2がサポート
        public int cost;            //設置時のコスト
        public int upCost;          //同じユニットを複数置く場合のコスト増加量
        public int recast;          //再配置までの時間
        public int hp;              //耐久値（最大HP）
        public int value;           //DPSの場合は攻撃力、サポートの場合は回復量、ポイント増加量など
        public float interval;      //行動速度（攻撃、回復をする間隔）
        public float distance;      //攻撃、回復の射程
        public float range;         //範囲攻撃の範囲
        public int targetNum;       //攻撃、回復の対象に出来る数

        public UnitsData.LvUpStatus lvUpStatus; //レベルアップ時に上がるステータス

        public bool place_UnitZone;             //ユニットの配置場所に置けるか
        public bool place_Floor;                //敵の通り道に置けるか
    }
}
