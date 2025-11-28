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

    //リザルト用
    [SerializeField] TextMeshProUGUI text_ResultValue;
    [SerializeField] GameObject resultParent;

    string resultText_1, resultText_2;

    GameObject[] button_Choice;
    EventsData.Content content;

    void Awake()
    {
        //選択肢用のボタンを取得
        button_Choice = new GameObject[choiceButtonParent.transform.childCount];
        for (int i = 0; i < button_Choice.Length; i++)
        {
            button_Choice[i] = choiceButtonParent.transform.GetChild(i).gameObject;
        }
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
            image.gameObject.SetActive(true);
        }
        else
        {
            image.gameObject.SetActive(false);
        }

        //テキストを読み込み
        text_TextBox.text = content.text;

        //リザルト用オブジェクトを非表示
        resultParent.SetActive(false);

        choiceButtonParent.SetActive(true);

        //ボタンの内容を読み込み
        for (int i = 0; i < button_Choice.Length; i++)
        {
            if (i < content.choice.Length)
            {
                //テキストを取得
                TextMeshProUGUI text_1 = button_Choice[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI text_2 = button_Choice[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();

                float nowValue = 0;
                bool active = false;

                //現在のステータスを読み込み
                switch (content.choice[i].type)
                {
                    case EventsData.ContentType.所持最大数増加:
                        nowValue = ParameterManager.Instance.maxUnitPossession;
                        active = true;
                        break;

                    case EventsData.ContentType.同ユニット配置数増加:
                        nowValue = ParameterManager.Instance.sameUnitMaxInstallation;
                        active = true;
                        break;

                    case EventsData.ContentType.HP回復:
                        nowValue = ParameterManager.Instance.hp;
                        active = true;
                        break;
                }

                //テキストの１行目
                text_1.text = content.choice[i].text;
                text_1.gameObject.SetActive(true);

                //テキストの２行目
                if (active)
                {
                    text_2.text = nowValue + " → " + Mathf.Max(nowValue + content.choice[i].value, 0);
                    text_2.gameObject.SetActive(true);
                }
                //２行目がない場合は１行目を中央に持ってくる
                else
                {
                    text_1.transform.localPosition = new Vector3(text_1.transform.localPosition.x, 0, text_1.transform.localPosition.z);
                    button_Choice[i].transform.GetChild(1).gameObject.SetActive(false);
                }

                button_Choice[i].SetActive(true);
            }
            else
            {
                button_Choice[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// イベントの選択肢　引数で選択を指定
    /// </summary>
    public void Choice(int choice)
    {
        //選択したイベント内容のウィンドウを表示
        switch (content.choice[choice].type)
        {
            case EventsData.ContentType.なし:

                break;

            case EventsData.ContentType.コスト削減:
                resultText_1 = "コストが減少！";
                resultText_2 = "6 → 3";
                Result();
                break;

            case EventsData.ContentType.再配置短縮:
                resultText_1 = "再配置時間が減少！";
                resultText_2 = "10 → 5";
                Result();
                break;

            case EventsData.ContentType.ユニット増加:

                break;

            case EventsData.ContentType.所持最大数増加:

                break;

            case EventsData.ContentType.同ユニット配置数増加:

                break;

            case EventsData.ContentType.HP回復:

                break;
        }
    }

    /// <summary>
    /// イベントの結果を表示
    /// </summary>
    public void Result()
    {
        choiceButtonParent.SetActive(false);
        resultParent.SetActive(true);

        //テキストを表示
        text_TextBox.text = resultText_1;
        text_ResultValue.text = resultText_2;
    }
}
