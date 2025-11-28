using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベントの呼び出し用
/// </summary>
public class EventWindowManager : Singleton<EventWindowManager>
{
    [SerializeField] GameObject canvas;

    [Space(10)]

    //各ウィンドウのコンポーネント
    public Window_Event window_Event;
    public Window_GetUnit window_GetUnit;
    public Window_Status window_Status;

    [Space(10)]

    public bool isEvent;

    void Awake()
    {
        //自身の子オブジェクト状態を解除
        if (gameObject.transform.parent != null)
        {
            gameObject.transform.parent = null;
            transform.position = new Vector3();
            transform.rotation = new Quaternion();
            transform.localScale = new Vector3();
        }
    }

    /// <summary>
    /// イベントを呼び出す　引数でIDを指定
    /// </summary>
    public void CallEventAt(int id)
    {
        if (isEvent) return;

        canvas.SetActive(true);
        window_Event.StartEvent(id);

        isEvent = true;
    }

    /// <summary>
    /// ステータスを表示する
    /// </summary>
    public void ViewStatus()
    {
        if (!Instance.window_Status.gameObject.activeSelf)
        {
            Instance.window_Status.gameObject.SetActive(true);
        }
        else
        {
            Instance.window_Status.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// イベントを終了してマップに戻る
    /// </summary>
    public void EndEvent()
    {
        if (!isEvent) return;

        canvas.SetActive(false);

        isEvent = false;
    }
}
