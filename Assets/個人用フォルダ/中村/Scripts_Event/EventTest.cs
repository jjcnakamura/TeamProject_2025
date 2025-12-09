using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

/// <summary>
/// イベントのテスト用
/// </summary>
public class EventTest : MonoBehaviour
{
    [SerializeField] GameObject canvas_Event;
    [SerializeField] GameObject eventTestButtonParent;
    [SerializeField] bool addUnit;
    
    Button eventTestButton;
    bool canvaNoActive = true;

    void Awake()
    {
        //イベントのCanvasがない場合は消える
        if (canvas_Event == null)
        {
            Destroy(gameObject);
            return;
        }
        //Canvasが非アクティブの場合は一度アクティブにしてから再度非アクティブにする
        else
        {
            canvas_Event.SetActive(true);
        }

        //デバッグ用　初期ステータスを設定
        if (ParameterManager.Instance.unitStatus.Length <= 0 && FindObjectOfType(System.Type.GetType("DebugScript")) == null)
        {
            ParameterManager.Instance.maxUnitPossession = 5;
            ParameterManager.Instance.maxInstallation = 10;
            ParameterManager.Instance.sameUnitMaxInstallation = 3;
            if (addUnit)
            {
                ParameterManager.Instance.AddUnit(0);
                ParameterManager.Instance.AddUnit(1);
                ParameterManager.Instance.AddUnit(3);
                ParameterManager.Instance.AddUnit(4);
                ParameterManager.Instance.AddUnit(6);
            } 
        }

        if (gameObject.transform.parent != null)
        {
            //自身の子オブジェクト状態を解除
            gameObject.transform.parent = null;

#if UNITY_EDITOR
            //Manager用の親オブジェクトがある場合はその子オブジェクトになる
            GameObject[] objs = FindObjectsOfType<GameObject>();
            var found = objs.FirstOrDefault(obj =>
            {
                string name = obj.name;
                if (name.Length < 7) return false;
                return name.Substring(0, 7) == "Manager";
            });
            if (found != null) gameObject.transform.parent = found.transform;
#endif

            transform.position = new Vector3();
            transform.rotation = new Quaternion();
            transform.localScale = new Vector3();
        }

        //イベント開始ボタンを生成
        if (eventTestButtonParent != null)
        {
            eventTestButton = eventTestButtonParent.transform.GetChild(0).GetComponent<Button>();

            if (eventTestButton != null)
            {
                eventTestButtonParent.SetActive(true);
                for (int i = 1; i < eventTestButtonParent.transform.childCount; i++) Destroy(eventTestButtonParent.transform.GetChild(i).gameObject);

                for (int i = 0; i < Mathf.Min(EventsData.Instance.eventData.Length, 9); i++)
                {
                    //ボタンを生成
                    Button button = Instantiate(eventTestButton);
                    button.transform.SetParent(eventTestButtonParent.transform);
                    button.transform.localScale = new Vector3(1, 1, 1);

                    //イベント名を読み込み
                    button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = EventsData.Instance.eventData[i].name;

                    //ボタンのOnClickを割り当て
                    int _i = i;
                    button.onClick.AddListener(() => EventWindowManager.Instance.CallEventAt(_i));
                }

                Destroy(eventTestButton.gameObject);
            }
        }
    }

    void Update()
    {
        //Canvasが非アクティブの場合は一度アクティブにしてから再度非アクティブにする
        if (canvaNoActive)
        {
            canvas_Event.SetActive(false);
            canvaNoActive = false;
        }

        //イベント呼び出し
        if (Input.GetKeyDown(KeyCode.Z) && EventsData.Instance.eventData.Length > 0)
        {
            EventWindowManager.Instance.EndEvent();
            EventWindowManager.Instance.CallEventAt(0);
        }
        if (Input.GetKeyDown(KeyCode.X) && EventsData.Instance.eventData.Length > 1)
        {
            EventWindowManager.Instance.EndEvent();
            EventWindowManager.Instance.CallEventAt(1);
        }
        if (Input.GetKeyDown(KeyCode.C) && EventsData.Instance.eventData.Length > 2)
        {
            EventWindowManager.Instance.EndEvent();
            EventWindowManager.Instance.CallEventAt(2);
        }
        if (Input.GetKeyDown(KeyCode.V) && EventsData.Instance.eventData.Length > 3)
        {
            EventWindowManager.Instance.EndEvent();
            EventWindowManager.Instance.CallEventAt(3);
        }
        if (Input.GetKeyDown(KeyCode.B) && EventsData.Instance.eventData.Length > 4)
        {
            EventWindowManager.Instance.EndEvent();
            EventWindowManager.Instance.CallEventAt(4);
        }
        if (Input.GetKeyDown(KeyCode.N) && EventsData.Instance.eventData.Length > 5)
        {
            EventWindowManager.Instance.EndEvent();
            EventWindowManager.Instance.CallEventAt(5);
        }
        if (Input.GetKeyDown(KeyCode.M) && EventsData.Instance.eventData.Length > 6)
        {
            EventWindowManager.Instance.EndEvent();
            EventWindowManager.Instance.CallEventAt(6);
        }

        //イベント終了
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EventWindowManager.Instance.EndEvent();
        }

        //リザルトを表示
        if (Input.GetKeyDown(KeyCode.W))
        {
            EventWindowManager.Instance.window_Event.Result(1, "", "");
        }

        //イベントが有効か判定
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool[] eventActive = EventWindowManager.Instance.EventActiveCheck();
            for (int i = 0; i < eventActive.Length; i++)
            {
                Debug.Log("イベント" + i + "は" + ((eventActive[i]) ? "「有効」" : "「無効」"));
            }
        }
    }
}
