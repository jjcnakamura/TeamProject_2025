using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// イベントのウィンドウ
/// </summary>
public class Window_Event : MonoBehaviour
{
    //イベントウィンドウの内容
    [SerializeField] TextMeshProUGUI text_TextBox;
    [SerializeField] Image image;
    [SerializeField] GameObject choiceButtonParent;

    [Space(10)]

    [SerializeField] float unitImageOffset;

    [Space(10)]

    //リザルト用
    [SerializeField] TextMeshProUGUI text_ResultValue;
    [SerializeField] GameObject resultParent;

    Button[] button_Choice;
    Vector2 imageDefaultSize, imageUnitSize;
    float choiceButtonText1_PosY;

    int choice;

    EventsData.Content content;

    void Awake()
    {
        //選択肢用のボタンを取得
        button_Choice = new Button[choiceButtonParent.transform.childCount];
        for (int i = 0; i < button_Choice.Length; i++)
        {
            button_Choice[i] = choiceButtonParent.transform.GetChild(i).GetComponent<Button>();
        }

        //イベント用の画像サイズを取得
        imageDefaultSize = new Vector2(image.rectTransform.sizeDelta.x, image.rectTransform.sizeDelta.y);
        imageUnitSize = imageDefaultSize * unitImageOffset;
    }

    /// <summary>
    /// イベント開始時に呼び出される
    /// </summary>
    public void StartEvent(int id)
    {
        //イベントの情報を読み込み
        content = EventsData.Instance.eventData[id];

        //画像を読み込み
        if (content.sprite != null)
        {
            image.sprite = content.sprite;
            image.rectTransform.sizeDelta = imageDefaultSize;
            image.gameObject.SetActive(true);
        }
        else
        {
            image.rectTransform.sizeDelta = imageDefaultSize;
            image.gameObject.SetActive(false);
        }

        //イベントが無効の場合は戻る
        if (content.choice.Length > 1 && !EventWindowManager.Instance.EventActiveCheck(id))
        {
            //イベントが無効な場合のリザルト
            EventWindowManager.Instance.window_Event.Result("イベントに有効な選択肢がありません", "");
            return;
        }

        //テキストを読み込み
        text_TextBox.text = content.text;

        //リザルト用オブジェクトを非表示
        resultParent.SetActive(false);

        choiceButtonParent.SetActive(true);

        //ボタンの内容を読み込み
        for (int i = 0; i < button_Choice.Length; i++)
        {
            button_Choice[i].gameObject.SetActive(false);

            if (i < content.choice.Length)
            {
                //テキストを取得
                TextMeshProUGUI text_1 = button_Choice[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI text_2 = button_Choice[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();

                float nowValue = 0;
                float upValue = 0;
                bool active = false;

                //現在のステータスを読み込み
                switch (content.choice[i].type)
                {
                    case EventsData.ContentType.所持最大数増加:
                        nowValue = ParameterManager.Instance.maxUnitPossession;
                        upValue = Mathf.Min(nowValue + (int)content.choice[i].value, UnitsData.Instance.maxUnitPossession);
                        active = true;
                        break;

                    case EventsData.ContentType.同ユニット配置数増加:
                        nowValue = ParameterManager.Instance.sameUnitMaxInstallation;
                        upValue = Mathf.Min(nowValue + (int)content.choice[i].value, 100); //最大１００まで
                        active = true;
                        break;

                    case EventsData.ContentType.HP回復:
                        nowValue = ParameterManager.Instance.hp;
                        upValue = Mathf.Min(nowValue + (int)content.choice[i].value, ParameterManager.Instance.maxHp);
                        active = true;
                        break;
                }

                //テキストの１行目
                text_1.text = content.choice[i].text;
                text_1.gameObject.SetActive(true);

                //高さを取得
                if (choiceButtonText1_PosY <= 0) choiceButtonText1_PosY = text_1.transform.localPosition.y;

                //テキストの２行目
                if (active)
                {
                    text_2.text = nowValue + " → " + upValue;
                    text_2.gameObject.SetActive(true);
                    text_1.transform.localPosition = new Vector3(text_1.transform.localPosition.x, choiceButtonText1_PosY, text_1.transform.localPosition.z);
                }
                //２行目がない場合は１行目を中央に持ってくる
                else
                {
                    text_1.transform.localPosition = new Vector3(text_1.transform.localPosition.x, 0, text_1.transform.localPosition.z);
                    button_Choice[i].transform.GetChild(1).gameObject.SetActive(false);
                }

                //イベントが無効の場合はボタンを無効にする
                button_Choice[i].interactable = (content.choice.Length <= 1 || EventWindowManager.Instance.EventActiveCheckType(content.choice[i].type));

                button_Choice[i].gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// イベントの選択肢　引数で選択を指定
    /// </summary>
    public void Choice(int arg_Choice)
    {
        choice = arg_Choice;

        string resultText1 = "";
        string resultText2 = "";

        //選択したイベント内容のウィンドウを表示
        switch (content.choice[choice].type)
        {
            case EventsData.ContentType.なし:

                break;

            case EventsData.ContentType.コスト削減:
                EventWindowManager.Instance.ViewStatus(content.choice[choice]);
                break;

            case EventsData.ContentType.再配置短縮:
                EventWindowManager.Instance.ViewStatus(content.choice[choice]);
                break;

            case EventsData.ContentType.ユニット増加:
                EventWindowManager.Instance.GetUnit((int)content.choice[choice].value);
                break;

            case EventsData.ContentType.所持最大数増加:
                int prePossession = ParameterManager.Instance.maxUnitPossession;
                ParameterManager.Instance.maxUnitPossession = Mathf.Min(
                ParameterManager.Instance.maxUnitPossession + (int)content.choice[choice].value, UnitsData.Instance.maxUnitPossession);
                resultText1 = "ユニットの所持数が増加！";
                resultText2 = "最大所持数：" + prePossession + "体 → " + ParameterManager.Instance.maxUnitPossession + "体";
                Result(resultText1, resultText2);
                break;

            case EventsData.ContentType.同ユニット配置数増加:
                int preInstallation = ParameterManager.Instance.sameUnitMaxInstallation;
                ParameterManager.Instance.sameUnitMaxInstallation = Mathf.Min(
                ParameterManager.Instance.sameUnitMaxInstallation + (int)content.choice[choice].value, 100); //最大１００まで
                resultText1 = "ユニットの配置数が増加！";
                resultText2 = "同ユニット配置数：" + preInstallation + "体 → " + ParameterManager.Instance.sameUnitMaxInstallation + "体";
                Result(resultText1, resultText2);
                break;

            case EventsData.ContentType.HP回復:
                int preHp = ParameterManager.Instance.hp;
                ParameterManager.Instance.hp = Mathf.Min(
                ParameterManager.Instance.hp + (int)content.choice[choice].value, ParameterManager.Instance.maxHp);
                resultText1 = "HPが回復した！";
                resultText2 = "HP：" + preHp + " → " + ParameterManager.Instance.hp;
                Result(resultText1, resultText2);
                break;
        }
    }

    /// <summary>
    /// イベントの結果を表示
    /// </summary>
    public void Result(string text1, string text2, Sprite sprite = null)
    {
        choiceButtonParent.SetActive(false);
        resultParent.SetActive(true);

        //テキストを表示
        text_TextBox.text = text1;
        text_ResultValue.text = text2;

        //画像を表示
        if (sprite != null)
        {
            image.rectTransform.sizeDelta = imageDefaultSize;

            //ユニットを表示する場合は画像のサイズをユニット用のものにする
            switch (content.choice[choice].type)
            {
                case EventsData.ContentType.コスト削減:
                    image.rectTransform.sizeDelta = imageUnitSize;
                    break;
                case EventsData.ContentType.再配置短縮:
                    image.rectTransform.sizeDelta = imageUnitSize;
                    break;
                case EventsData.ContentType.ユニット増加:
                    image.rectTransform.sizeDelta = imageUnitSize;
                    break;
            }
            
            image.sprite = sprite;
        }
    }
}
