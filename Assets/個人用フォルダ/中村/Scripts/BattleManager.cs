using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦闘シーンの管理用
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    //各Canvas
    [SerializeField] GameObject canvasParent;
    GameObject[] canvas;

    //プレイヤーのパラメーター用変数
    public GameObject playerSide;
    int maxPlayerHp;
    public int playerHp;
    int maxPoint;
    public int point;

    //ユニットのパラメーター用変数
    BattleUnit_Base[] battleUnitStatus; //配置されている各ユニットのステータス
    GameObject[] battleUnitPrefab;                              //ボタンから生成されるユニットのPrefab
    [Space(10)] [SerializeField] GameObject unitPullZone;       //ユニットを持ってくるボタンの集まり
    [SerializeField] PullUnit unitPullButton;                   //ユニットを持ってくるボタン
    Vector3 pullUnitSizeOffset = new Vector3(0.4f, 0.4f, 0.4f); //ユニットを持った場合にかけるサイズ補正
    GameObject dragUnit;                        //現在ドラッグしているユニット
    int dragUnitIndex;                          //ドラッグしているユニットの要素番号
    [SerializeField] GameObject unitZoneParent; //ユニットの配置場所の親オブジェクト
    [SerializeField] GameObject floorParent;    //敵が通る道の親オブジェクト
    UnitZone[] unitZone;                        //ユニットの配置場所
    //現在ドラッグしているユニットがどこに配置できるか
    public bool place_UnitZone { get; private set; }
    public bool place_Floor { get; private set; }

    //敵出現の時間をカウントするタイマー
    public float timer_EnemySpawn { get; private set; }

    //ゲームの状態を表すフラグ
    public bool isMainGame, isClear, isGameOver, isUnitDrag, isOnMouseUnitZone;

    void Start()
    {
        //Canvasの表示
        canvas = new GameObject[canvasParent.transform.childCount];
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i] = canvasParent.transform.GetChild(i).gameObject;
            canvas[i].SetActive(i == 0);
        }

        //フラグを設定
        isMainGame = true;

        //デバッグ用　キャラをロード
        ParameterManager.Instance.maxUnitPossession = 5;
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(0);

        //プレイヤーの初期パラメーターを設定
        maxPlayerHp = ParameterManager.Instance.hp;
        playerHp = maxPlayerHp;
        maxPoint = ParameterManager.Instance.point;
        point = maxPoint;

        //使用するユニットのPrefabを読み込み
        battleUnitPrefab = new GameObject[ParameterManager.Instance.unitStatus.Length];
        GameObject[] loadUnits = Resources.LoadAll<GameObject>("Units");
        for (int i = 0; i < battleUnitPrefab.Length; i++)
        {
            battleUnitPrefab[i] = loadUnits[ParameterManager.Instance.unitStatus[i].id];
        }

        //ユニットを持ってくるボタンをUI上に配置
        foreach (Transform n in unitPullZone.transform) Destroy(n.gameObject); //全ての子オブジェクトを削除
        for (int i = 0; i < battleUnitPrefab.Length; i++)
        {
            //インスタンスを生成
            PullUnit instance = Instantiate(unitPullButton);
            instance.transform.SetParent(unitPullZone.transform);

            //サイズが崩れないように調整
            RectTransform rect = instance.GetComponent<RectTransform>();
            rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y, 0);
            instance.transform.rotation = new Quaternion();
            instance.transform.localScale = new Vector3(1, 1, 1);

            //キャラを割り当て
            instance.index = i;
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
    }

    void Update()
    {
        //ユニットドラッグ中の処理
        if (isUnitDrag)
        {
            dragUnit.transform.position = MouseManager.Instance.worldPos;

            if (Input.GetKeyUp(KeyCode.Mouse0) && !isOnMouseUnitZone) LetgoUnit();
        }
    }

    void FixedUpdate()
    {
        timer_EnemySpawn += Time.fixedDeltaTime;
    }

    //ユニットを選ぶ
    public void PullUnit(int unitIndex)
    {
        isUnitDrag = true;

        dragUnitIndex = unitIndex;
        dragUnit = Instantiate(battleUnitPrefab[unitIndex]);
        dragUnit.transform.localScale -= pullUnitSizeOffset;
        dragUnit.transform.rotation = new Quaternion(0, 180f, 0, 0);

        //どこに配置出来るか（仮）
        place_UnitZone = true;
        place_Floor = false;
    }
    //ドラッグしているユニットを離す
    public void LetgoUnit()
    {
        Destroy(dragUnit);
        isUnitDrag = false;

        place_UnitZone = false;
        place_Floor = false;
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
        battleUnitStatus[zoneIndex].cost = ParameterManager.Instance.unitStatus[unitIndex].cost;
        battleUnitStatus[zoneIndex].recast = ParameterManager.Instance.unitStatus[unitIndex].recast;
        battleUnitStatus[zoneIndex].maxHp = ParameterManager.Instance.unitStatus[unitIndex].hp;
        battleUnitStatus[zoneIndex].hp = ParameterManager.Instance.unitStatus[unitIndex].hp;
        battleUnitStatus[zoneIndex].value = ParameterManager.Instance.unitStatus[unitIndex].value;
        battleUnitStatus[zoneIndex].interval = ParameterManager.Instance.unitStatus[unitIndex].interval;
        battleUnitStatus[zoneIndex].distance = ParameterManager.Instance.unitStatus[unitIndex].distance;
        battleUnitStatus[zoneIndex].range = ParameterManager.Instance.unitStatus[unitIndex].range;

        battleUnitStatus[zoneIndex].isBattle = true;

        place_UnitZone = false;
        place_Floor = false;
        isUnitDrag = false;
    }
    //配置されているユニットを削除
    public void OutUnit(int zoneIndex)
    {
        if (!unitZone[zoneIndex].placed) return;

        battleUnitStatus[zoneIndex].Out();
        battleUnitStatus[zoneIndex] = null;
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
