using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// イベントの呼び出し用
/// </summary>
public class EventWindowManager : Singleton<EventWindowManager>
{
    //各Canvas
    [SerializeField] GameObject[] canvas;

    [Space(10)]

    //各ウィンドウのコンポーネント
    public Window_Event window_Event;
    public Window_GetUnit window_GetUnit;
    public Window_Status window_Status;

    [Space(10)]

    public bool isEvent;

    void Awake()
    {
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

        //Canvasの表示
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(i <= 0);
        }

        //各ウィンドウを非表示に
        window_Event.gameObject.SetActive(false);
        window_GetUnit.gameObject.SetActive(false);
        window_Status.gameObject.SetActive(false);
    }

    /// <summary>
    /// イベントを呼び出す　引数でIDを指定
    /// </summary>
    public void CallEventAt(int id)
    {
        if (isEvent) return;

        canvas[0].SetActive(true);

        //イベントを開始
        window_Event.gameObject.SetActive(true);
        window_Event.StartEvent(id);

        isEvent = true;
    }

    /// <summary>
    /// イベントを終了してマップに戻る
    /// </summary>
    public void EndEvent()
    {
        if (!isEvent) return;

        //ステータスウィンドウを非表示に
        if (window_Status.gameObject.activeSelf)
        {
            window_Status.isViewStatus = false;
            window_Status.ViewUnits();
        }

        //各ウィンドウを非表示に
        window_Event.gameObject.SetActive(false);
        window_GetUnit.gameObject.SetActive(false);
        window_Status.gameObject.SetActive(false);

        canvas[0].SetActive(false);

        isEvent = false;
    }

    /// <summary>
    /// ステータスを表示する
    /// </summary>
    public void ViewStatus()
    {
        if (!window_Status.gameObject.activeSelf)
        {
            window_Status.gameObject.SetActive(true);
            window_Status.ViewUnits();
        }
        else
        {
            window_Status.ViewUnits();
        }
    }

    /// <summary>
    /// ステータスを上げるユニットの選択画面を表示する
    /// </summary>
    public void ViewStatus(EventsData.Choice content)
    {
        if (!window_Status.gameObject.activeSelf)
        {
            window_Status.gameObject.SetActive(true);
            window_Status.ViewUnits(content);
        }
        else
        {
            window_Status.ViewUnits();
        }
    }

    /// <summary>
    /// ポーズボタン
    /// </summary>
    public void Pause()
    {
        //ポーズ開始
        if (!canvas[1].activeSelf)
        {
            //ポーズ画面を表示
            canvas[1].SetActive(true);
        }
        //ポーズ終了
        else
        {
            //ポーズ画面を非表示
            canvas[1].SetActive(false);
        }
    }
}
