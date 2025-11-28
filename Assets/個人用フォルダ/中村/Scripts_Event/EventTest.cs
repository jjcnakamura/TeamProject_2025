using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベントのテスト用
/// </summary>
public class EventTest : MonoBehaviour
{
    void Update()
    {
        //イベント呼び出し
        if (Input.GetKeyDown(KeyCode.Alpha1) && EventsData.Instance.eventData.Length > 0)
        {
            EventWindowManager.Instance.CallEventAt(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && EventsData.Instance.eventData.Length > 1)
        {
            EventWindowManager.Instance.CallEventAt(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && EventsData.Instance.eventData.Length > 2)
        {
            EventWindowManager.Instance.CallEventAt(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && EventsData.Instance.eventData.Length > 3)
        {
            EventWindowManager.Instance.CallEventAt(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) && EventsData.Instance.eventData.Length > 4)
        {
            EventWindowManager.Instance.CallEventAt(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && EventsData.Instance.eventData.Length > 5)
        {
            EventWindowManager.Instance.CallEventAt(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) && EventsData.Instance.eventData.Length > 6)
        {
            EventWindowManager.Instance.CallEventAt(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8) && EventsData.Instance.eventData.Length > 7)
        {
            EventWindowManager.Instance.CallEventAt(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9) && EventsData.Instance.eventData.Length > 8)
        {
            EventWindowManager.Instance.CallEventAt(8);
        }

        //イベント終了
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EventWindowManager.Instance.EndEvent();
        }

        //リザルトを表示
        if (Input.GetKeyDown(KeyCode.W))
        {
            EventWindowManager.Instance.window_Event.Result();
        }
    }
}
