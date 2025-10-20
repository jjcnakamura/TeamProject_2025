using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦闘シーンの管理用
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    public GameObject playerSide;

    //プレイヤーのパラメーター用変数
    int maxPlayerHp;
    [Space(10)]  public int playerHp;
    int maxPoint;
    public int point;

    //ユニットのパラメーター用変数
    BattleUnit_Base[] battleUnitStatus;         //配置されている各ユニットのステータス
    [SerializeField] GameObject testUnit;       //ユニットの配置テスト用
    [SerializeField] GameObject unitPullZone;   //ユニットを持ってくるボタンの集まり
    [SerializeField] PullUnit unitPullButton;   //ユニットを持ってくるボタン
    Vector3 pullUnitSizeOffset = new Vector3(0.4f, 0.4f, 0.4f); //ユニットを持った場合にかけるサイズ補正
    GameObject dragUnit;                        //現在ドラッグしているユニット
    int dragUnitIndex;                          //ドラッグしているユニットの要素番号
    [SerializeField] GameObject unitZoneParent; //ユニットの配置場所の親オブジェクト
    UnitZone[] unitZone;                        //ユニットの配置場所

    public float timer_EnemySpawn { get; private set; }

    public bool isMainGame, isClear, isGameOver, isUnitDrag, isOnMouseUnitZone;

    void Start()
    {
        //フラグを設定
        isMainGame = true;

        //デバッグ用　初期キャラ3体をロード
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(1);
        ParameterManager.Instance.AddUnit(2);

        //プレイヤーの初期パラメーターを設定
        maxPlayerHp = ParameterManager.Instance.hp;
        playerHp = maxPlayerHp;
        maxPoint = ParameterManager.Instance.point;
        point = maxPoint;

        //ユニットを持ってくるボタンをUI上に配置
        foreach (Transform n in unitPullZone.transform) Destroy(n.gameObject); //全ての子オブジェクトを削除
        for (int i = 0; i < ParameterManager.Instance.unitStatus.Length; i++)
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
        unitZone = new UnitZone[unitZoneParent.transform.childCount];
        for (int i = 0; i < unitZone.Length; i++)
        {
            unitZone[i] = unitZoneParent.transform.GetChild(i).GetComponent<UnitZone>();
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
        dragUnit = Instantiate(testUnit);
        dragUnit.transform.localScale -= pullUnitSizeOffset;
        dragUnit.transform.rotation = new Quaternion(0, 180f, 0, 0);
    }
    //ドラッグしているユニットを離す
    public void LetgoUnit()
    {
        Destroy(dragUnit);
        isUnitDrag = false;
    }
    //ユニットを配置する
    public void PlaceUnit(int zoneIndex)
    {
        dragUnit.transform.localScale += pullUnitSizeOffset;
        dragUnit.transform.position = unitZone[zoneIndex].unitPoint;

        battleUnitStatus[zoneIndex] = dragUnit.GetComponent<BattleUnit_Base>();

        isUnitDrag = false;
    }
    //配置されているユニットを削除
    public void OutUnit(int zoneIndex)
    {
        if (!unitZone[zoneIndex].placed) return;

        battleUnitStatus[zoneIndex].Out();
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
