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

    //各ユニットのステータス画面用の変数
    [SerializeField] GameObject unitInfoParent;

    TextMeshProUGUI[] eventText;
    string defaultTitleText;
    int unitNum = -1;

    [System.NonSerialized] public EventsData.Choice eventContent;

    public bool isActive,isViewStatus, isEvent;

    void Awake()
    {
        defaultTitleText = text_Title.text;
    }

    /// <summary>
    /// ステータス画面を開く、閉じる時に呼び出す
    /// </summary>
    public void ViewUnits()
    {
        //ユニットのステータス画面を開いている場合は先にそちらを閉じる
        if (isViewStatus)
        {
            ViewStatus();
        }
        //ユニット一覧画面を開く
        else if (!isActive)
        {
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

            text_Title.text = defaultTitleText;

            isEvent = false;
            isActive = true;
        }
        //ユニット一覧画面を閉じる
        else if (!isEvent)
        {
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
    /// イベントの中でステータス画面を開く、閉じる時に呼び出す　引数でイベント用の変数を受け取る
    /// </summary>
    public void ViewUnits(EventsData.Choice arg_Content)
    {
        //ユニットのステータス画面を開いている場合は先にそちらを閉じる
        if (isViewStatus)
        {
            ViewStatus();
        }
        //ユニット一覧画面を開く
        else if (!isActive)
        {
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

            isEvent = true;
            isActive = true;
        }
        //ユニット一覧画面を閉じる
        else
        {
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
        if (!isEvent)
        {
            //ユニットのステータスを表示
            if (!isViewStatus && index >= 0)
            {
                //ユニットの情報画面を表示
                unitInfoParent.SetActive(true);

                //ボタンを非表示
                unitButtonParent.SetActive(false);
                //ユニット名を表示
                text_Title.text = ParameterManager.Instance.unitStatus[index].name;

                //ユニットの各情報を表示

            }
            //ユニットのステータスを非表示
            else
            {
                unitInfoParent.SetActive(false);
                unitButtonParent.SetActive(true);
                text_Title.text = defaultTitleText;
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
                    ParameterManager.Instance.unitStatus[index].cost + (int)eventContent.value, 1);
                    resultText1 = ParameterManager.Instance.unitStatus[index].name + "のコストが減少！";
                    resultText2 = preCost + " → " + ParameterManager.Instance.unitStatus[index].cost;
                    resultSprite = ParameterManager.Instance.unitStatus[index].sprite;
                    break;

                case EventsData.ContentType.再配置短縮:
                    int preRecast = ParameterManager.Instance.unitStatus[index].recast;
                    ParameterManager.Instance.unitStatus[index].recast = Mathf.Max(
                    ParameterManager.Instance.unitStatus[index].recast + (int)eventContent.value, 1);
                    resultText1 = ParameterManager.Instance.unitStatus[index].name + "の再配置短縮が短縮！";
                    resultText2 = preRecast + " → " + ParameterManager.Instance.unitStatus[index].recast;
                    resultSprite = ParameterManager.Instance.unitStatus[index].sprite;
                    break;
            }

            //リザルトを表示
            EventWindowManager.Instance.window_Event.Result(resultText1, resultText2, resultSprite);

            //ステータス画面を閉じる
            ViewUnits();
        }
    }

    //持っているユニットの数に合わせたボタンを生成する
    void GenerateUnitButton()
    {
        unitNum = ParameterManager.Instance.unitStatus.Length;
        eventText = new TextMeshProUGUI[unitNum];

        foreach (Transform n in unitButtonParent.transform) Destroy(n.gameObject); //全ての子オブジェクトを削除   

        for (int i = 0; i < unitNum; i++)
        {
            unitButtonParent.SetActive(true);

            //ボタンを生成
            Button button = Instantiate(unitButtonPredab);
            button.transform.SetParent(unitButtonParent.transform);
            button.transform.localScale = unitButtonParent.transform.localScale;

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
