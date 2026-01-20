using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ステータス画面
/// </summary>
public class Window_Status : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text_Title;
    [SerializeField] GameObject[] button_Back;

    [Space(10)]

    //各ユニットのボタン用変数
    [SerializeField] GameObject unitButtonParent;
    [SerializeField] Button unitButtonPredab;

    [Space(10)]

    [SerializeField] float unitImageOffsetY;

    [Space(10)]

    //各ユニットのステータス画面用の変数
    [SerializeField] GameObject unitInfoParent;
    [SerializeField] Image image_Unit;
    [SerializeField] Image image_UnitBack;
    [SerializeField] TextMeshProUGUI text_Name, text_Info, text_Lv, text_Exp, text_NextExp, text_Cost, text_Recast,
                                     text_Hp, text_Value, text_Interval, text_Distance, text_Range, text_TargetNum;

    TextMeshProUGUI[] eventText;
    string defaultTitleText;
    Vector2 unitImageDefaultPos;
    int unitNum = -1;

    [System.NonSerialized] public EventsData.Choice eventContent;

    //状態を表すフラグ
    public bool isActive,isViewStatus, isViewStatusId, isEvent;

    void Awake()
    {
        //オブジェクトの初期化
        foreach (Transform n in unitButtonParent.transform) Destroy(n.gameObject); 
        defaultTitleText = text_Title.text;

        //UIの初期位置を読み込み
        unitImageDefaultPos = image_UnitBack.rectTransform.position;
    }

    /// <summary>
    /// ステータス画面を開く、閉じる時に呼び出す　ユニットがいないなどで開けなかった場合はfalseを返す
    /// </summary>
    public void ViewUnits()
    {
        //ユニットのステータス画面を開いている場合は先にそちらを閉じる
        if (isViewStatusId)
        {
            ViewStatusId();
        }
        else if (isViewStatus)
        {
            ViewStatus();
        }
        //ユニット一覧画面を開く
        else if (!isActive)
        {
            SoundManager.Instance.PlaySE_Sys(1);

            //ユニットの情報画面を非表示
            unitInfoParent.SetActive(false);

            //所持ユニットの数が前と違う場合はボタンを生成
            if (unitNum != ParameterManager.Instance.unitStatus.Length) GenerateUnitButton();
            //各ユニットのボタンを表示
            unitButtonParent.SetActive(true);
            UnitButtonSetInteractable(true);
            //戻るボタンを表示
            for (int i = 0; i < button_Back.Length; i++)
            {
                button_Back[i].SetActive(true);
            }

            //イベント用のテキストを非アクティブに
            for (int i = 0; i < unitNum; i++)
            {
                eventText[i].gameObject.SetActive(false);
            }

            text_Title.gameObject.SetActive(true);
            text_Title.text = defaultTitleText;

            //フラグを設定
            isEvent = false;
            isActive = true;
        }
        //ユニット一覧画面を閉じる
        else if (!isEvent)
        {
            SoundManager.Instance.PlaySE_Sys(2);

            //フラグを設定
            isActive = false;

            gameObject.SetActive(false);
        }
        //イベント中の場合
        else
        {
            ViewUnits(eventContent);
        }
    }

    /// <summary>
    /// イベントの中でステータス画面を開く、閉じる時に呼び出す　引数でイベント用の変数を受け取る　ユニットがいないなどで開けなかった場合はfalseを返す
    /// </summary>
    public void ViewUnits(EventsData.Choice arg_Content)
    {
        //ユニットを持っていない場合は戻る
        if (ParameterManager.Instance.unitStatus.Length <= 0)
        {
            switch (arg_Content.type)
            {
                case EventsData.ContentType.コスト削減:
                    EventWindowManager.Instance.window_Event.Result(1, "ユニットを持っていない！", "");
                    break;

                case EventsData.ContentType.再配置短縮:
                    EventWindowManager.Instance.window_Event.Result(1, "ユニットを持っていない！", "");
                    break;
            }

            gameObject.SetActive(false);
            return;
        }

        //ユニットのステータス画面を開いている場合は先にそちらを閉じる
        if (isViewStatusId)
        {
            ViewStatusId();
        }
        else if (isViewStatus)
        {
            ViewStatus();
        }
        //ユニット一覧画面を開く
        else if (!isActive)
        {
            SoundManager.Instance.PlaySE_Sys(0);

            //ユニットの情報画面を非表示
            unitInfoParent.SetActive(false);

            //所持ユニットの数が前と違う場合はボタンを生成
            if (unitNum != ParameterManager.Instance.unitStatus.Length) GenerateUnitButton();
            //各ユニットのボタンを表示
            unitButtonParent.SetActive(true);
            UnitButtonSetInteractable(true);
            //戻るボタンを非表示
            for (int i = 0; i < button_Back.Length; i++)
            {
                button_Back[i].SetActive(false);
            }

            //イベントの内容を読み込み
            eventContent = arg_Content;
            //イベント名を表示
            text_Title.gameObject.SetActive(true);
            text_Title.text = eventContent.text;

            //ステータスの変動値をテキストで表示
            for (int i = 0; i < unitNum; i++)
            {
                float result = 0;

                switch (eventContent.type)
                {
                    case EventsData.ContentType.コスト削減:
                        if (ParameterManager.Instance.unitStatus[i].cost > 0)
                        {
                            result = Mathf.Max(ParameterManager.Instance.unitStatus[i].cost + eventContent.value, 1);
                            eventText[i].text = ParameterManager.Instance.unitStatus[i].cost + " → " + (int)result;
                        }
                        //コストが０のユニットは選べなくする
                        else
                        {
                            UnitButtonSetInteractable(false, i);
                            eventText[i].text = "";
                        }
                        break;

                    case EventsData.ContentType.再配置短縮:
                        if (ParameterManager.Instance.unitStatus[i].recast > 0)
                        {
                            result = Mathf.Max(ParameterManager.Instance.unitStatus[i].recast + eventContent.value, 1);
                            eventText[i].text = ParameterManager.Instance.unitStatus[i].recast + " → " + (int)result;
                        }
                        //リキャストが０のユニットは選べなくする
                        else
                        {
                            UnitButtonSetInteractable(false, i);
                            eventText[i].text = "";
                        }
                        break;
                }

                eventText[i].gameObject.SetActive(true);
            }

            //フラグを設定
            isEvent = true;
            isActive = true;
        }
        //ユニット一覧画面を閉じる
        else
        {
            SoundManager.Instance.PlaySE_Sys(2);

            //フラグを設定
            isEvent = false;
            isActive = false;

            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ステータスを見るユニットを選ぶ
    /// </summary>
    public void ViewStatus(int index = -1)
    {
        if (!isActive) return;

        if (!isEvent)
        {
            SoundManager.Instance.PlaySE_Sys(0);

            //ユニットのステータスを表示
            if (!isViewStatus && !isViewStatusId && index >= 0)
            {
                //ユニットの情報画面を表示
                unitInfoParent.SetActive(true);

                //ユニット一覧画面の要素を非表示
                unitButtonParent.SetActive(false);
                text_Title.gameObject.SetActive(false);

                //ユニットの各情報を表示
                image_Unit.sprite = ParameterManager.Instance.unitStatus[index].sprite;
                text_Name.text = ParameterManager.Instance.unitStatus[index].name;
                text_Info.text = UnitsData.Instance.unit[ParameterManager.Instance.unitStatus[index].id].info;
                text_Lv.text = "レベル  " + ParameterManager.Instance.unitStatus[index].lv.ToString();
                text_Exp.text = "経験値：" + ParameterManager.Instance.unitStatus[index].exp.ToString();
                //経験値の計算
                text_NextExp.text = "経験値残り：" + (UnitsData.Instance.levelUpExp[ParameterManager.Instance.unitStatus[index].lv] - ParameterManager.Instance.unitStatus[index].exp).ToString();
                text_Cost.text = "　コスト：" + ParameterManager.Instance.unitStatus[index].cost.ToString();
                text_Recast.text = "　再配置：" + ParameterManager.Instance.unitStatus[index].recast.ToString() + "秒";

                //ステータスの表示、非表示、ステータス名を指定する項目
                text_Hp.text = "　耐久値：" + ParameterManager.Instance.unitStatus[index].hp.ToString();
                text_Hp.gameObject.SetActive(UnitsData.Instance.unit[ParameterManager.Instance.unitStatus[index].id].viewStatus.hp);

                string valueName = UnitsData.Instance.unit[ParameterManager.Instance.unitStatus[index].id].valueName;
                text_Value.text = valueName + "：" + ParameterManager.Instance.unitStatus[index].value.ToString();
                if (valueName == "鈍化時間") text_Value.text += "秒";
                text_Value.gameObject.SetActive(UnitsData.Instance.unit[ParameterManager.Instance.unitStatus[index].id].viewStatus.value);

                text_Interval.text = "行動間隔：" + ParameterManager.Instance.unitStatus[index].interval.ToString() + "秒";
                text_Interval.gameObject.SetActive(UnitsData.Instance.unit[ParameterManager.Instance.unitStatus[index].id].viewStatus.interval);

                text_Distance.text = "　　射程：" + ParameterManager.Instance.unitStatus[index].distance.ToString();
                text_Distance.gameObject.SetActive(UnitsData.Instance.unit[ParameterManager.Instance.unitStatus[index].id].viewStatus.distance);

                text_Range.text = "　　範囲：" + ParameterManager.Instance.unitStatus[index].range.ToString();
                text_Range.gameObject.SetActive(UnitsData.Instance.unit[ParameterManager.Instance.unitStatus[index].id].viewStatus.range);

                text_TargetNum.text = "対象人数：" + ParameterManager.Instance.unitStatus[index].targetNum.ToString();
                text_TargetNum.gameObject.SetActive(UnitsData.Instance.unit[ParameterManager.Instance.unitStatus[index].id].viewStatus.targetNum);

                //キャラの背景を読み込み
                image_UnitBack.sprite = UnitsData.Instance.iconBackSprite[ParameterManager.Instance.unitStatus[index].role];

                //フラグを設定
                isViewStatus = true;
            }
            //ユニットのステータスを非表示
            else
            {
                SoundManager.Instance.PlaySE_Sys(2);

                //ユニット一覧画面の要素を再表示
                unitInfoParent.SetActive(false);
                unitButtonParent.SetActive(true);

                text_Title.gameObject.SetActive(true);
                text_Title.text = defaultTitleText;

                //キャラのアイコンの位置を戻す
                image_UnitBack.rectTransform.position = unitImageDefaultPos;

                //フラグを設定
                isViewStatus = false;
                isViewStatusId = false;
            }
        }
        //イベント中の場合
        else
        {
            string resultText1 = "";
            string resultText2 = "";
            Sprite resultSprite = null;

            //イベントの内容によるステータス変動
            switch (eventContent.type)
            {
                case EventsData.ContentType.コスト削減:
                    int preCost = ParameterManager.Instance.unitStatus[index].cost;
                    ParameterManager.Instance.unitStatus[index].cost = Mathf.Max(
                    ParameterManager.Instance.unitStatus[index].cost + (int)eventContent.value, 1); //最低１
                    resultText1 = ParameterManager.Instance.unitStatus[index].name + "の配置コストが減少！";
                    resultText2 = "配置コスト：" + preCost + " → " + ParameterManager.Instance.unitStatus[index].cost;
                    resultSprite = ParameterManager.Instance.unitStatus[index].sprite;
                    break;

                case EventsData.ContentType.再配置短縮:
                    int preRecast = ParameterManager.Instance.unitStatus[index].recast;
                    ParameterManager.Instance.unitStatus[index].recast = Mathf.Max(
                    ParameterManager.Instance.unitStatus[index].recast + (int)eventContent.value, 1); //最低１
                    resultText1 = ParameterManager.Instance.unitStatus[index].name + "の再配置までの時間が短縮！";
                    resultText2 = "再配置時間：" + preRecast + "秒 → " + ParameterManager.Instance.unitStatus[index].recast + "秒";
                    resultSprite = ParameterManager.Instance.unitStatus[index].sprite;
                    break;
            }

            //ユニット一覧画面の要素を再表示
            unitInfoParent.SetActive(false);
            unitButtonParent.SetActive(true);

            text_Title.gameObject.SetActive(true);
            text_Title.text = defaultTitleText;

            //キャラのアイコンの位置を戻す
            image_UnitBack.rectTransform.position = unitImageDefaultPos;

            //フラグを設定
            isViewStatus = false;
            isViewStatusId = false;

            //ステータス画面を閉じる
            ViewUnits();

            //リザルトを表示
            EventWindowManager.Instance.window_Event.Result(3, resultText1, resultText2, resultSprite);
        }
    }

    /// <summary>
    /// IDを指定して初期ステータスを見るユニットを選ぶ
    /// </summary>
    public void ViewStatusId(int id = -1)
    {
        if (!isActive) return;

        //ユニットのステータスを表示
        if (!isViewStatusId && !isViewStatus && id >= 0 && id < UnitsData.Instance.unit.Length && !isEvent)
        {
            SoundManager.Instance.PlaySE_Sys(0);

            //ユニットの情報画面を表示
            unitInfoParent.SetActive(true);

            //ユニット一覧画面の要素を非表示
            unitButtonParent.SetActive(false);
            text_Title.gameObject.SetActive(false);

            //ユニットの各情報を表示
            image_Unit.sprite = UnitsData.Instance.unit[id].sprite;
            text_Name.text = UnitsData.Instance.unit[id].name;
            text_Info.text = UnitsData.Instance.unit[id].info;

            //レベルと経験値は非表示
            text_Lv.gameObject.SetActive(false);
            text_Exp.gameObject.SetActive(false);
            text_NextExp.gameObject.SetActive(false);

            text_Cost.text = "　コスト：" + UnitsData.Instance.unit[id].cost.ToString();
            text_Recast.text = "　再配置：" + UnitsData.Instance.unit[id].recast.ToString() + "秒";

            //ステータスの表示、非表示、ステータス名を指定する項目
            text_Hp.text = "　耐久値：" + UnitsData.Instance.unit[id].hp.ToString();
            text_Hp.gameObject.SetActive(UnitsData.Instance.unit[id].viewStatus.hp);

            string valueName = UnitsData.Instance.unit[id].valueName;
            text_Value.text = valueName + "：" + UnitsData.Instance.unit[id].value.ToString();
            if (valueName == "鈍化時間") text_Value.text += "秒";
            text_Value.gameObject.SetActive(UnitsData.Instance.unit[id].viewStatus.value);

            text_Interval.text = "行動間隔：" + UnitsData.Instance.unit[id].interval.ToString() + "秒";
            text_Interval.gameObject.SetActive(UnitsData.Instance.unit[id].viewStatus.interval);

            text_Distance.text = "　　射程：" + UnitsData.Instance.unit[id].distance.ToString();
            text_Distance.gameObject.SetActive(UnitsData.Instance.unit[id].viewStatus.distance);

            text_Range.text = "　　範囲：" + UnitsData.Instance.unit[id].range.ToString();
            text_Range.gameObject.SetActive(UnitsData.Instance.unit[id].viewStatus.range);

            text_TargetNum.text = "対象人数：" + UnitsData.Instance.unit[id].targetNum.ToString();
            text_TargetNum.gameObject.SetActive(UnitsData.Instance.unit[id].viewStatus.targetNum);

            //キャラの背景を読み込み
            image_UnitBack.sprite = UnitsData.Instance.iconBackSprite[UnitsData.Instance.unit[id].role];

            //キャラのアイコンの位置を調整
            image_UnitBack.rectTransform.position = new Vector2(image_UnitBack.rectTransform.position.x, image_UnitBack.rectTransform.position.y - unitImageOffsetY);

            //フラグを設定
            isViewStatusId = true;
        }
        //ユニットのステータスを非表示
        else if (id < UnitsData.Instance.unit.Length)
        {
            //ユニット一覧画面の要素を再表示
            text_Lv.gameObject.SetActive(true);
            text_Exp.gameObject.SetActive(true);
            text_NextExp.gameObject.SetActive(true);

            unitInfoParent.SetActive(false);
            unitButtonParent.SetActive(true);

            text_Title.gameObject.SetActive(true);
            text_Title.text = defaultTitleText;

            //キャラのアイコンの位置を戻す
            image_UnitBack.rectTransform.position = unitImageDefaultPos;

            bool closeUnitsView = !isViewStatus;

            //フラグを設定
            isViewStatusId = false;
            isViewStatus = false;

            //ステータス画面を閉じる
            if (closeUnitsView) ViewUnits();
        }
    }

    //持っているユニットの数に合わせたボタンを生成する
    void GenerateUnitButton()
    {
        unitNum = ParameterManager.Instance.unitStatus.Length;
        eventText = new TextMeshProUGUI[unitNum];

        foreach (Transform n in unitButtonParent.transform) Destroy(n.gameObject); //全ての子オブジェクトを削除   

        unitButtonParent.SetActive(true);

        for (int i = 0; i < unitNum; i++)
        {
            //ボタンを生成
            Button button = Instantiate(unitButtonPredab);
            button.transform.SetParent(unitButtonParent.transform);
            button.transform.localScale = unitButtonParent.transform.localScale;

            //背景を読み込み
            button.GetComponent<Image>().sprite = UnitsData.Instance.iconBackSprite[ParameterManager.Instance.unitStatus[i].role];
            //画像を読み込み
            button.transform.GetChild(0).GetComponent<Image>().sprite = ParameterManager.Instance.unitStatus[i].sprite;
            //名前を読み込み
            button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ParameterManager.Instance.unitStatus[i].name;
            //イベント用のテキストを取得
            eventText[i] = button.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            //ボタンのOnClickを割り当て
            int _i = i;
            button.onClick.AddListener(() => ViewStatus(_i));
        }
    }

    //各ユニットのボタンがクリック可能かを切り替える
    //第一引数で真偽値 第二引数でユニットの番号を指定（指定しない場合は全てのユニットを参照する）
    void UnitButtonSetInteractable(bool active, int index = -1)
    {
        if (index >= 0)
        {
            unitButtonParent.transform.GetChild(index).GetComponent<Button>().interactable = active;
        }
        else
        {
            for (int i = 0; i < unitButtonParent.transform.childCount; i++)
            {
                unitButtonParent.transform.GetChild(i).GetComponent<Button>().interactable = active;
            }
        }
    }
}
