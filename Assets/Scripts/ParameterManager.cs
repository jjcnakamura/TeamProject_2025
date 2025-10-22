using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// プレイヤーと所持ユニットのパラメーターを管理する
/// </summary>
public class ParameterManager : Singleton<ParameterManager>
{
    //プレイヤーのステータスと初期値
    public int hp = 10;                     //プレイヤー（タワー）のHP

    public int maxUnitPossession = 3;       //最大ユニット所持数

    public int point = 6;                   //初期ポイント数
    public int maxInstallation = 4;         //ユニット最大配置数
    public int sameUnitMaxInstallation = 1; //同じユニットの最大配置数

    [Space(10)]

    //所持しているユニットごとのステータス
    public UnitStatus[] unitStatus;
    public int[] battleUnitId;      //戦闘で使用するユニットの番号

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

        //ユニットと敵のデータ読み込み用オブジェクトを生成
        //GameObject loadData = new GameObject();
        //loadData.name = "LoadData";
        //loadData.AddComponent<UnitsData>();
        //loadData.AddComponent<EnemiesData>();
        //loadData.transform.SetParent(transform);
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
        unitStatus[index].prefab = UnitsData.Instance.unit[id].prefab;     //キャラのPrefab
        unitStatus[index].sprite = UnitsData.Instance.unit[id].sprite;     //キャラの画像

        unitStatus[index].id = id;                                         //どのユニットかを示すID
        unitStatus[index].role = UnitsData.Instance.unit[id].role;         //ロール　0がDPS、1がタンク、2がサポート

        unitStatus[index].lv = 1;                                          //レベル
        unitStatus[index].exp = 0;                                         //所持経験値

        unitStatus[index].cost = UnitsData.Instance.unit[id].cost;         //設置時のコスト
        unitStatus[index].recast = UnitsData.Instance.unit[id].recast;     //再配置までの時間

        unitStatus[index].hp = UnitsData.Instance.unit[id].hp;             //耐久値（最大HP）
        unitStatus[index].value = UnitsData.Instance.unit[id].value;       //DPSの場合は攻撃力、サポートの場合は回復量、ポイント増加量など
        unitStatus[index].interval = UnitsData.Instance.unit[id].interval; //行動速度（攻撃、回復をする間隔）
        unitStatus[index].distance = UnitsData.Instance.unit[id].distance; //攻撃、回復の射程
        unitStatus[index].range = UnitsData.Instance.unit[id].range;       //範囲攻撃の範囲

        unitStatus[index].place_UnitZone = UnitsData.Instance.unit[id].place_UnitZone; //ユニットの配置場所に置けるか
        unitStatus[index].place_Floor = UnitsData.Instance.unit[id].place_Floor;       //敵の通り道に置けるか
    }

    /// <summary>
    /// ゲーム内で増減する各ユニットのパラメータ
    /// </summary>
    [System.Serializable]
    public struct UnitStatus
    {
        public GameObject prefab;   //キャラのPrefab
        public Sprite sprite;       //キャラの画像

        public int id;              //どのユニットかを示すID
        public int role;            //ロール　0がDPS、1がタンク、2がサポート

        public int lv;              //レベル
        public int exp;             //所持経験値

        public int cost;            //設置時のコスト
        public int recast;          //再配置までの時間

        public int hp;              //耐久値（最大HP）
        public int value;           //DPSの場合は攻撃力、サポートの場合は回復量、ポイント増加量など
        public float interval;      //行動速度（攻撃、回復をする間隔）
        public float distance;      //攻撃、回復の射程
        public float range;         //範囲攻撃の範囲

        public bool place_UnitZone; //ユニットの配置場所に置けるか
        public bool place_Floor;    //敵の通り道に置けるか
    }
}
