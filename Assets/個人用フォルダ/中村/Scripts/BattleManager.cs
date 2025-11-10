using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject playerSide;                         //プレイヤーの陣地（タワーに当たる場所）
    [SerializeField] TextMeshProUGUI text_Hp;             //プレイヤー（タワー）のHP用テキスト
    [SerializeField] TextMeshProUGUI text_UnitNum;        //配置しているユニット数用テキスト
    [SerializeField] TextMeshProUGUI text_SameMaxUnitNum; //同じユニットの最大配置数用テキスト
    [SerializeField] TextMeshProUGUI text_Point;          //ポイント用テキスト
    public           TextMeshProUGUI text_EnemyNum;       //現在の敵の数用テキスト
    int maxPlayerHp;
    public int playerHp;
    public int battleUnitNum;
    int maxInstallation;
    int sameUnitMaxInstallation;
    public int point;

    [Space(10)]

    public int nowEnemyNum; //現在の敵の数

    int pointUpVal = 1;     //時間で増加するポイント数
    float pointUpTime = 1f; //ポイントの時間増加にかかる秒数
    float timer_PointUp;    //ポイントの時間増加用タイマー

    [Space(10)]

    //ユニットのパラメーター用変数
    [SerializeField] GameObject unitPullButtonParent;               //ユニットを持ってくるボタンの親オブジェクト
    [SerializeField] PullUnit unitPullButtonPrefab;                 //ユニットを持ってくるボタン
    [SerializeField] PullUnit[] unitPullButton;                     //ユニットを持ってくるボタン
    GameObject[] battleUnitPrefab;                                  //ボタンから生成されるユニットのPrefab
    float defaltCameraPosY = 18.5f;                                 //カメラの高さの初期値
    float pullUnitSizeOffset = 0.4f;                                //ユニットを持った場合にかけるサイズ補正
    public BattleUnit_Base dragUnit { get; private set; }           //現在ドラッグしているユニット
    int dragUnitIndex;                                              //ドラッグしているユニットの要素番号
    int[] unitInstallationCount;                                    //ユニットの配置数カウント
    public bool[] unitMaxInstallation { get; private set; }         //ユニットが最大配置数に達しているか

    [SerializeField] GameObject unitZoneParent;                     //ユニットの配置場所の親オブジェクト
    [SerializeField] GameObject floorParent;                        //敵が通る道の親オブジェクト
    UnitZone[] unitZone;                                            //ユニットの配置場所
    public bool place_UnitZone { get; private set; }                //現在ドラッグしているユニットがどこに配置できるか
    public bool place_Floor { get; private set; }                   //現在ドラッグしているユニットがどこに配置できるか
    public BattleUnit_Base[] battleUnitStatus { get; private set; } //配置されている各ユニットのステータス
    [System.NonSerialized] public int[] unitCost;                   //各ユニットのコスト
    [System.NonSerialized] public float[] unitRecast;               //各ユニットのリキャスト

    [Space(10)]

    //ユニットと敵のHP用変数
    public GameObject hpbarParent;
    public GameObject hpbarPrefab;

    [Space(10)]

    //敵のルート表示用変数
    public GameObject enemyRouteArrow;
    public float preEnemySpawnTime = 5;
    public float enemyRouteActiveTime = 4;

    [Space(10)]

    //時間の進む速度に関する変数
    [SerializeField] float dragTimeScale = 0.4f;    //ユニットドラッグ中の時間が進む速度
    [SerializeField] float speedUpTimeScale = 1.5f; //時間加速中の時間が進む速度
    [SerializeField] GameObject speedUpImage;       //時間加速中に表示されるオブジェクト
    float preTimeScale;                             //前の時間が進む速度

    //敵出現のdisplay時間をカウントするタイマー
    public float timer_EnemySpawn { get; private set; }

    //ゲームの状態を表すフラグ
    public bool isMainGame, isClear, isGameOver, isPause, isSpeedUp, isMaxInstallation, isUnitDrag, isUnitPlace, isOnMouseUnitZone;

    void Awake()
    {
        //デバッグ用　初期ステータスを設定
        if (ParameterManager.Instance.unitStatus.Length <= 0 && FindObjectOfType(System.Type.GetType("DebugScript")) == null)
        {
            ParameterManager.Instance.maxUnitPossession = 5;
            ParameterManager.Instance.maxInstallation = 10;
            ParameterManager.Instance.sameUnitMaxInstallation = 3;
            ParameterManager.Instance.AddUnit(0);
            ParameterManager.Instance.AddUnit(1);
            ParameterManager.Instance.AddUnit(2);
            ParameterManager.Instance.AddUnit(3);
            ParameterManager.Instance.AddUnit(4);
        }
    }

    public void Start()
    {
        //ユニット設置関連の初期パラメーターを設定
        maxInstallation = ParameterManager.Instance.maxInstallation;
        text_UnitNum.text = battleUnitNum.ToString() + " / " + maxInstallation.ToString();
        sameUnitMaxInstallation = ParameterManager.Instance.sameUnitMaxInstallation;
        text_SameMaxUnitNum.text = "同ユニット\n配置可数 : " + sameUnitMaxInstallation;

        //使用するユニットのPrefabを読み込み
        battleUnitPrefab = new GameObject[ParameterManager.Instance.unitStatus.Length];
        for (int i = 0; i < battleUnitPrefab.Length; i++)
        {
            battleUnitPrefab[i] = ParameterManager.Instance.unitStatus[i].prefab;
        }

        //ユニットを持ってくるボタンをUI上に配置
        foreach (Transform n in unitPullButtonParent.transform) Destroy(n.gameObject); //全ての子オブジェクトを削除
        unitPullButton = new PullUnit[battleUnitPrefab.Length];
        unitCost = new int[battleUnitPrefab.Length];
        unitRecast = new float[battleUnitPrefab.Length];
        for (int i = 0; i < battleUnitPrefab.Length; i++)
        {
            //インスタンスを生成
            unitPullButton[i] = Instantiate(unitPullButtonPrefab);
            unitPullButton[i].transform.SetParent(unitPullButtonParent.transform);

            //サイズが崩れないように調整
            RectTransform rect = unitPullButton[i].GetComponent<RectTransform>();
            rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y, 0);
            unitPullButton[i].transform.rotation = new Quaternion();
            unitPullButton[i].transform.localScale = new Vector3(1, 1, 1);

            //キャラID、コスト、リキャストを割り当て
            unitPullButton[i].index = i;
            unitPullButton[i].text_Cost.text = ParameterManager.Instance.unitStatus[i].cost.ToString();
            unitCost[i] = ParameterManager.Instance.unitStatus[i].cost;
            unitRecast[i] = ParameterManager.Instance.unitStatus[i].recast;
        }

        //戦闘シーン開始時のみ実行する
        if (!isMainGame && !isClear && !isGameOver)
        {
            //FPSを固定
            Application.targetFrameRate = 60;

            //プレイヤーの初期パラメーターを設定
            maxPlayerHp = ParameterManager.Instance.hp;
            playerHp = maxPlayerHp;
            text_Hp.text = playerHp.ToString();
            point = ParameterManager.Instance.point;
            text_Point.text = point.ToString();

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
            unitInstallationCount = new int[unitZone.Length];
            unitMaxInstallation = new bool[unitZone.Length];

            //Canvasの表示
            canvas = new GameObject[canvasParent.transform.childCount];
            for (int i = 0; i < canvas.Length; i++)
            {
                canvas[i] = canvasParent.transform.GetChild(i).gameObject;
                canvas[i].SetActive(i <= 1);
            }
            canvasParent.SetActive(true);

            //リトライ前に速度を上げていた場合は開始時から速度を上げる
            if (FlagManager.Instance.isSpeedUp && !isSpeedUp)
            {
                SpeedUp();
            }
            else
            {
                speedUpImage.SetActive(false);
            }

            //フラグを設定
            isMainGame = true;
        }
    }

    void Update()
    {
        if (!isMainGame) return; //メインゲーム中でなければ戻る

        DragUnit();   //ユニットドラッグ中の処理
        ClearCheck(); //敵を全て倒したらクリアにする
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
        dragUnit = Instantiate(battleUnitPrefab[unitIndex]).GetComponent<BattleUnit_Base>();

        //カメラからの距離によってサイズを調整する
        float lerp = Mathf.Lerp(1f, 0.9125f, (Camera.main.transform.position.y - defaltCameraPosY) / (37f - defaltCameraPosY));
        float sizeOffset = Mathf.Min(Camera.main.transform.position.y * (pullUnitSizeOffset / defaltCameraPosY * lerp), 1);
        dragUnit.transform.localScale -= new Vector3(sizeOffset, sizeOffset, sizeOffset);
        dragUnit.transform.rotation = new Quaternion(0, 180f, 0, 0);

        //Colliderのサイズを決定、攻撃範囲を表示
        if (dragUnit.col_AttackZone != null && dragUnit.mesh_AttackZone != null)
        {
            dragUnit.col_AttackZone.transform.localScale = new Vector3(ParameterManager.Instance.unitStatus[unitIndex].distance,
                                                                       dragUnit.col_AttackZone.transform.localScale.y,
                                                                       ParameterManager.Instance.unitStatus[unitIndex].distance);
            dragUnit.mesh_AttackZone.enabled = true;
        }

        //どこに配置出来るか
        place_UnitZone = ParameterManager.Instance.unitStatus[unitIndex].place_UnitZone;
        place_Floor = ParameterManager.Instance.unitStatus[unitIndex].place_Floor;

        //時間を遅くする
        preTimeScale = Time.timeScale;
        Time.timeScale = dragTimeScale;
    }
    //ドラッグしているユニットを離す
    public void LetgoUnit()
    {
        if (dragUnit != null) Destroy(dragUnit.gameObject);

        //時間の速さを戻す
        Time.timeScale = preTimeScale;

        place_UnitZone = false;
        place_Floor = false;
        isUnitDrag = false;
    }
    //ユニットを配置する
    public void PlaceUnit(int zoneIndex)
    {
        int unitIndex = dragUnitIndex;

        dragUnit.transform.position = unitZone[zoneIndex].unitPoint;
        dragUnit.transform.localScale = battleUnitPrefab[unitIndex].transform.localScale;

        //ステータスを読み込み
        battleUnitStatus[zoneIndex] = dragUnit;
        battleUnitStatus[zoneIndex].unitIndex = unitIndex;
        battleUnitStatus[zoneIndex].zoneIndex = zoneIndex;
        battleUnitStatus[zoneIndex].role = ParameterManager.Instance.unitStatus[unitIndex].role;
        battleUnitStatus[zoneIndex].maxHp = ParameterManager.Instance.unitStatus[unitIndex].hp;
        battleUnitStatus[zoneIndex].hp = ParameterManager.Instance.unitStatus[unitIndex].hp;
        battleUnitStatus[zoneIndex].defaultValue = ParameterManager.Instance.unitStatus[unitIndex].value;
        battleUnitStatus[zoneIndex].value = ParameterManager.Instance.unitStatus[unitIndex].value;
        battleUnitStatus[zoneIndex].interval = ParameterManager.Instance.unitStatus[unitIndex].interval;
        battleUnitStatus[zoneIndex].distance = ParameterManager.Instance.unitStatus[unitIndex].distance;
        battleUnitStatus[zoneIndex].range = ParameterManager.Instance.unitStatus[unitIndex].range;

        battleUnitStatus[zoneIndex].isBattle = true;

        //HPバーを生成
        Hpbar battleUnitHpbar = Instantiate(hpbarPrefab).GetComponent<Hpbar>();
        battleUnitHpbar.transform.SetParent(hpbarParent.transform);
        battleUnitHpbar.transform.localScale = new Vector3(1f, 1f, 1f);
        battleUnitHpbar.transform.localRotation = new Quaternion();
        battleUnitHpbar.targetUnit = battleUnitStatus[zoneIndex];
        battleUnitStatus[zoneIndex].hpbarObj = battleUnitHpbar.gameObject;

        //攻撃範囲を非表示
        if (dragUnit.col_AttackZone != null && dragUnit.mesh_AttackZone != null)
        {
            battleUnitStatus[zoneIndex].mesh_AttackZone.enabled = false;
        }

        //コスト分のポイントを減らして再配置のコストを増やしてUIに反映
        PointChange(-unitCost[unitIndex]);
        unitCost[unitIndex] += ParameterManager.Instance.unitStatus[unitIndex].upCost;
        unitPullButton[unitIndex].text_Cost.text = unitCost[unitIndex].ToString();

        //時間の速さを戻す
        Time.timeScale = preTimeScale;

        //現在のユニット配置数を増やしてUIに反映
        battleUnitNum++;
        text_UnitNum.text = battleUnitNum.ToString() + " / " + maxInstallation.ToString();

        //ユニットの配置数をカウント、最大の場合はこれ以上配置できなくする
        isMaxInstallation = (battleUnitNum >= maxInstallation) ? true : false;
        unitInstallationCount[unitIndex] = Mathf.Min(unitInstallationCount[unitIndex] + 1, sameUnitMaxInstallation);
        if (unitInstallationCount[unitIndex] >= sameUnitMaxInstallation) unitMaxInstallation[unitIndex] = true;

        dragUnit = null;

        place_UnitZone = false;
        place_Floor = false;
        isUnitDrag = false;

        isUnitPlace = true;
    }

    //ユニットドラッグ中の処理
    void DragUnit()
    {
        if (isUnitDrag)
        {
            //ユニットをマウスに追従させる
            dragUnit.transform.position = MouseManager.Instance.worldPos;
            if (Input.GetKeyUp(KeyCode.Mouse0) && !isOnMouseUnitZone) LetgoUnit();
        }
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
        //ユニットをドラッグしていたら離す
        LetgoUnit();

        isSpeedUp = false;
        isMainGame = false;
        isClear = true;

        //時間の速さを等速に
        Time.timeScale = 1f;
        isSpeedUp = false;
        FlagManager.Instance.isSpeedUp = false;

        //ステージクリア画面を表示
        canvas[2].SetActive(true);
    }
    //ゲームオーバー
    void GameOver()
    {
        //ユニットをドラッグしていたら離す
        LetgoUnit();

        isSpeedUp = false;
        isMainGame = false;
        isGameOver = true;

        //時間の速さを等速に
        Time.timeScale = 1f;
        isSpeedUp = false;
        FlagManager.Instance.isSpeedUp = false;

        //ゲームオーバー画面を表示
        canvas[3].SetActive(true);
    }

    //配置されているユニットを削除
    public void OutUnit(int zoneIndex)
    {
        if (!unitZone[zoneIndex].placed) return;

        //同ユニットの配置コストを減らしてUIに反映
        unitCost[battleUnitStatus[zoneIndex].unitIndex] -= ParameterManager.Instance.unitStatus[battleUnitStatus[zoneIndex].unitIndex].upCost;
        unitPullButton[battleUnitStatus[zoneIndex].unitIndex].text_Cost.text = unitCost[battleUnitStatus[zoneIndex].unitIndex].ToString();

        //現在のユニット配置数を減らしてUIに反映
        battleUnitNum--;
        text_UnitNum.text = battleUnitNum.ToString() + " / " + maxInstallation.ToString();

        //ユニットの配置数をカウント
        isMaxInstallation = (battleUnitNum >= maxInstallation) ? true : false;
        unitInstallationCount[battleUnitStatus[zoneIndex].unitIndex] = Mathf.Max(unitInstallationCount[0] - 1, 0);
        unitMaxInstallation[battleUnitStatus[zoneIndex].unitIndex] = false;

        //ユニットを削除
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
            GameOver();
        }
    }

    /// <summary>
    /// 時間の進みを速くするボタン
    /// </summary>
    public void SpeedUp()
    {
        //時間加速開始
        if (!isSpeedUp)
        {
            //時間を速くする
            Time.timeScale = speedUpTimeScale;

            speedUpImage.SetActive(true);

            isSpeedUp = true;
            FlagManager.Instance.isSpeedUp = true;
        }
        //時間加速終了
        else
        {
            //時間の速さを等速に
            Time.timeScale = 1f;

            speedUpImage.SetActive(false);

            isSpeedUp = false;
            FlagManager.Instance.isSpeedUp = false;
        }
    }

    /// <summary>
    /// ポーズボタン
    /// </summary>
    public void Pause()
    {
        //ポーズ開始
        if (!isPause)
        {
            isPause = true;
            isMainGame = false;

            //時間を止める
            preTimeScale = Time.timeScale;
            Time.timeScale = 0f;

            //ポーズ画面を表示
            canvas[4].SetActive(true);
        }
        //ポーズ終了
        else
        {
            //時間の速さを戻す
            Time.timeScale = preTimeScale;

            //ポーズ画面を非表示
            canvas[4].SetActive(false);

            isPause = false;
            isMainGame = true;
        }
    }

    /// <summary>
    /// ステージをリトライするボタン
    /// </summary>
    public void Retry()
    {
        FadeManager.Instance.LoadSceneIndex(SceneManager.GetActiveScene().buildIndex, 0.5f);
    }
}
