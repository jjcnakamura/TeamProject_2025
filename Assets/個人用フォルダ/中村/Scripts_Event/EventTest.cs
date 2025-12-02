using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベントのテスト用
/// </summary>
public class EventTest : MonoBehaviour
{
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
            ParameterManager.Instance.AddUnit(3);
            ParameterManager.Instance.AddUnit(4);
            ParameterManager.Instance.AddUnit(6);
        }
    }

    void Update()
    {
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
            EventWindowManager.Instance.window_Event.Result("", "");
        }
    }
}
