using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 戦闘シーンの管理用
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    //各Canvas
    [SerializeField] GameObject canvasParent;
    GameObject[] canvas;

    [Space(10)]

    //プレイヤーのパラメーター用変数
    public GameObject playerSide;                //プレイヤーの陣地（タワーに当たる場所）
    [SerializeField] TextMeshProUGUI text_Hp;    //プレイヤー（タワー）のHP用テキスト
    [SerializeField] TextMeshProUGUI text_Point; //ポイント用テキスト
    public TextMeshProUGUI text_EnemyNum;        //現在の敵の数用テキスト
    int maxPlayerHp;
    public int playerHp;
    int maxPoint;
    public int point;

    [Space(10)]

    public int nowEnemyNum; //現在の敵の数

    int pointUpVal = 1;     //時間で増加するポイント数
    float pointUpTime = 1f; //ポイントの時間増加にかかる秒数
    float timer_PointUp;    //ポイントの時間増加用タイマー

    [Space(10)]

    //ユニットのパラメーター用変数
    [SerializeField] GameObject unitPullButtonParent;           //ユニットを持ってくるボタンの親オブジェクト
    [SerializeField] PullUnit unitPullButton;                   //ユニットを持ってくるボタン
    GameObject[] battleUnitPrefab;                              //ボタンから生成されるユニットのPrefab
    Vector3 pullUnitSizeOffset = new Vector3(0.4f, 0.4f, 0.4f); //ユニットを持った場合にかけるサイズ補正
    GameObject dragUnit;                              //現在ドラッグしているユニット
    int dragUnitIndex;                                //ドラッグしているユニットの要素番号
    [SerializeField] GameObject unitZoneParent;       //ユニットの配置場所の親オブジェクト
    [SerializeField] GameObject floorParent;          //敵が通る道の親オブジェクト
    UnitZone[] unitZone;                              //ユニットの配置場所
    public bool place_UnitZone { get; private set; }  //現在ドラッグしているユニットがどこに配置できるか
    public bool place_Floor { get; private set; }     //現在ドラッグしているユニットがどこに配置できるか

    BattleUnit_Base[] battleUnitStatus;               //配置されている各ユニットのステータス
    [System.NonSerialized] public int[] unitCost;     //各ユニットのコスト
    [System.NonSerialized] public float[] unitRecast; //各ユニットのリキャスト

    //敵出現の時間をカウントするタイマー
    public float timer_EnemySpawn { get; private set; }

    //ゲームの状態を表すフラグ
    public bool isMainGame, isClear, isGameOver, isUnitDrag, isUnitPlace, isOnMouseUnitZone;

    void Start()
    {
        //デバッグ用　キャラをロード
        ParameterManager.Instance.maxUnitPossession = 5;
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(1);
        ParameterManager.Instance.AddUnit(2);
        ParameterManager.Instance.AddUnit(3);
        ParameterManager.Instance.AddUnit(4);

        //プレイヤーの初期パラメーターを設定
        maxPlayerHp = ParameterManager.Instance.hp;
        playerHp = maxPlayerHp;
        text_Hp.text = playerHp.ToString();
        maxPoint = ParameterManager.Instance.point;
        point = maxPoint;
        text_Point.text = point.ToString();

        text_EnemyNum.text = nowEnemyNum.ToString();

        //使用するユニットのPrefabを読み込み
        battleUnitPrefab = new GameObject[ParameterManager.Instance.unitStatus.Length];
        for (int i = 0; i < battleUnitPrefab.Length; i++)
        {
            battleUnitPrefab[i] = ParameterManager.Instance.unitStatus[i].prefab;
        }

        //ユニットを持ってくるボタンをUI上に配置
        foreach (Transform n in unitPullButtonParent.transform) Destroy(n.gameObject); //全ての子オブジェクトを削除
        unitCost = new int[battleUnitPrefab.Length];
        unitRecast = new float[battleUnitPrefab.Length];
        for (int i = 0; i < battleUnitPrefab.Length; i++)
        {
            //インスタンスを生成
            PullUnit pullUnit = Instantiate(unitPullButton);
            pullUnit.transform.SetParent(unitPullButtonParent.transform);

            //サイズが崩れないように調整
            RectTransform rect = pullUnit.GetComponent<RectTransform>();
            rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y, 0);
            pullUnit.transform.rotation = new Quaternion();
            pullUnit.transform.localScale = new Vector3(1, 1, 1);

            //キャラID、コスト、リキャストを割り当て
            pullUnit.index = i;
            pullUnit.text_Cost.text = ParameterManager.Instance.unitStatus[i].cost.ToString();
            unitCost[i] = ParameterManager.Instance.unitStatus[i].cost;
            unitRecast[i] = ParameterManager.Instance.unitStatus[i].recast;
        }

        //ユニットの配置場所を取得
        int unitZoneNum = unitZoneParent.transform.childCount;
        unitZone = new UnitZone[unitZoneNum + floorParent.transform.childCount];
        for (int i = 0; i < unitZoneNum; i++)
        {
            unitZone[i] = unitZoneParent.transform.GetChild(i).GetComponent<UnitZone>();
            unitZone[i].index = i;
            unitZone[i].unitZone = true;
        }
        for (int i = unitZoneNum; i < unitZone.Length; i++)
        {
            unitZone[i] = floorParent.transform.GetChild(i - unitZoneNum).GetComponent<UnitZone>();
            unitZone[i].index = i; 
        }
        //ユニットの配置場所の数に合わせて配置可能ユニット数を決定
        battleUnitStatus = new BattleUnit_Base[unitZone.Length];

        //Canvasの表示
        canvas = new GameObject[canvasParent.transform.childCount];
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i] = canvasParent.transform.GetChild(i).gameObject;
            canvas[i].SetActive(i == 0);
        }

        //フラグを設定
        isMainGame = true;
    }

    void Update()
    {
        if (!isMainGame) return; //メインゲーム中でなければ戻る

        ClearCheck(); //敵を全て倒したらクリアにする

        //ユニットドラッグ中の処理
        if (isUnitDrag)
        {
            dragUnit.transform.position = MouseManager.Instance.worldPos;
            if (Input.GetKeyUp(KeyCode.Mouse0) && !isOnMouseUnitZone) LetgoUnit();
        }
    }

    void FixedUpdate()
    {
        if (!isMainGame) return; //メインゲーム中でなければ戻る

        PointUp();                               //ポイントの時間増加
        timer_EnemySpawn += Time.fixedDeltaTime; //敵の出現時間カウント
    }

    //ユニットを選ぶ
    public void PullUnit(int unitIndex)
    {
        isUnitDrag = true;

        dragUnitIndex = unitIndex;
        dragUnit = Instantiate(battleUnitPrefab[unitIndex]);
        dragUnit.transform.localScale -= pullUnitSizeOffset;
        dragUnit.transform.rotation = new Quaternion(0, 180f, 0, 0);

        //どこに配置出来るか
        place_UnitZone = ParameterManager.Instance.unitStatus[unitIndex].place_UnitZone;
        place_Floor = ParameterManager.Instance.unitStatus[unitIndex].place_Floor;
    }
    //ドラッグしているユニットを離す
    public void LetgoUnit()
    {
        Destroy(dragUnit);
        
        place_UnitZone = false;
        place_Floor = false;
        isUnitDrag = false;
    }
    //ユニットを配置する
    public void PlaceUnit(int zoneIndex)
    {
        dragUnit.transform.localScale += pullUnitSizeOffset;
        dragUnit.transform.position = unitZone[zoneIndex].unitPoint;

        int unitIndex = dragUnitIndex;

        //ステータスを読み込み
        battleUnitStatus[zoneIndex] = dragUnit.GetComponent<BattleUnit_Base>();
        battleUnitStatus[zoneIndex].zoneIndex = zoneIndex;
        battleUnitStatus[zoneIndex].role = ParameterManager.Instance.unitStatus[unitIndex].role;
        battleUnitStatus[zoneIndex].maxHp = ParameterManager.Instance.unitStatus[unitIndex].hp;
        battleUnitStatus[zoneIndex].hp = ParameterManager.Instance.unitStatus[unitIndex].hp;
        battleUnitStatus[zoneIndex].value = ParameterManager.Instance.unitStatus[unitIndex].value;
        battleUnitStatus[zoneIndex].interval = ParameterManager.Instance.unitStatus[unitIndex].interval;
        battleUnitStatus[zoneIndex].distance = ParameterManager.Instance.unitStatus[unitIndex].distance;
        battleUnitStatus[zoneIndex].range = ParameterManager.Instance.unitStatus[unitIndex].range;

        battleUnitStatus[zoneIndex].isBattle = true;

        //コスト分のポイントを減らす
        PointChange(-ParameterManager.Instance.unitStatus[unitIndex].cost);

        place_UnitZone = false;
        place_Floor = false;
        isUnitDrag = false;

        isUnitPlace = true;
    }
    //ポイントの時間増加
    void PointUp()
    {
        if (timer_PointUp < pointUpTime)
        {
            timer_PointUp += Time.fixedDeltaTime;
        }
        else
        {
            timer_PointUp = 0;
            PointChange(pointUpVal);
        }
    }
    //敵を全て倒したか判定
    void ClearCheck()
    {
        if (isClear) return;

        //敵の数が0になったらクリア
        if (nowEnemyNum <= 0) Clear();
    }
    //ステージクリア
    void Clear()
    {
        isMainGame = false;
        isClear = true;

        //ステージクリア画面を表示
        canvas[1].SetActive(true);
    }
    //ゲームオーバー
    void GameOver()
    {
        isMainGame = false;
        isGameOver = true;

        //ゲームオーバー画面を表示
        canvas[2].SetActive(true);
    }

    //配置されているユニットを削除
    public void OutUnit(int zoneIndex)
    {
        if (!unitZone[zoneIndex].placed) return;

        battleUnitStatus[zoneIndex].Out();
        battleUnitStatus[zoneIndex] = null;

        unitZone[zoneIndex].placed = false;
    }
    //ポイントを増減する
    public void PointChange(int val)
    {
        point = Mathf.Min(Mathf.Max(point + val, 0), 999); //最大値は999
        text_Point.text = point.ToString();
    }
    //プレイヤー（タワー）へのダメージ
    public void Damage()
    {
        if (!isMainGame) return;

        playerHp = Mathf.Max(playerHp - 1, 0);
        text_Hp.text = playerHp.ToString();

        //HPが0になったらゲームオーバー
        if (playerHp <= 0)
        {
            isMainGame = false;
            isGameOver = true;

            GameOver();
        }
    }
}
