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
            canvas[i].SetActive(false);
        }

        //各ウィンドウを非表示に
        window_Event.gameObject.SetActive(false);
        window_GetUnit.gameObject.SetActive(false);
        window_Status.gameObject.SetActive(false);
    }

    /// <summary>
    /// イベントが有効か無効かをbool型の配列で返す　例：イベントID１が有効の場合は配列の要素１がtrueになる
    /// 引数でIDを指定するとそのイベントのみを判定する
    /// </summary>
    public bool[] EventActiveCheck()
    {
        bool[] result = new bool[EventsData.Instance.eventData.Length];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = EventActiveCheck(i);
        }

        return result;
    }

    /// <summary>
    /// 指定したIDのイベントが有効か無効かをboolで返す
    /// </summary>
    public bool EventActiveCheck(int id)
    {
        id = Mathf.Min(Mathf.Max(id, 0), EventsData.Instance.eventData.Length - 1);

        bool result = true;
        int inativeNum = 0;

        for (int i = 0; i < EventsData.Instance.eventData[id].choice.Length; i++)
        {
            //イベントの選択肢の内容から判定する
            if (!EventActiveCheckType(EventsData.Instance.eventData[id].choice[i].type)) inativeNum++;

            //選択肢の内容全てが行えない場合はイベントを無効と判定する
            if (inativeNum >= EventsData.Instance.eventData[id].choice.Length)
            {
                result = false;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 指定したContentTypeのイベントが有効か無効かをboolで返す
    /// </summary>
    public bool EventActiveCheckType(EventsData.ContentType type)
    {
        bool result = true;

        switch (type)
        {
            case EventsData.ContentType.コスト削減:

                //コストが最低値(１)のユニットしかいない場合は無効になる
                int lowestCostUnitNum = 0;
                for (int j = 0; j < ParameterManager.Instance.unitStatus.Length; j++)
                {
                    if (ParameterManager.Instance.unitStatus[j].cost <= 1)
                    {
                        lowestCostUnitNum++;
                        if (lowestCostUnitNum >= ParameterManager.Instance.unitStatus.Length)
                        {
                            result = false;
                            break;
                        }
                    }
                }

                break;

            case EventsData.ContentType.再配置短縮:

                //再配置時間が最低値(１)のユニットしかいない場合は無効になる
                int lowestRecastUnitNum = 0;
                for (int j = 0; j < ParameterManager.Instance.unitStatus.Length; j++)
                {
                    if (ParameterManager.Instance.unitStatus[j].recast <= 1)
                    {
                        lowestRecastUnitNum++;
                        if (lowestRecastUnitNum >= ParameterManager.Instance.unitStatus.Length)
                        {
                            result = false;
                            break;
                        }
                    }
                }

                break;

            case EventsData.ContentType.ユニット増加:

                //ユニットの所持数が最大の場合は無効
                if (ParameterManager.Instance.unitStatus.Length >= UnitsData.Instance.maxUnitPossession) result = false;

                break;

            case EventsData.ContentType.所持最大数増加:

                //所持可能数が最大の場合は無効
                if (ParameterManager.Instance.maxUnitPossession >= UnitsData.Instance.maxUnitPossession) result = false;

                break;

            case EventsData.ContentType.同ユニット配置数増加:

                //同ユニット配置可能数が最大(１００)の場合は無効
                if (ParameterManager.Instance.sameUnitMaxInstallation >= 100) result = false;

                break;

            case EventsData.ContentType.HP回復:

                break;
        }

        return result;
    }

    /// <summary>
    /// 戦闘のIDを抽選してint型の配列で返す　第一引数で戦闘イベントの数　第二引数で戦闘のプール番号を指定
    /// </summary>
    public int[] BattleRandomChoice(int num, int poolIndex)
    {
        int[] result = new int[num];

        //プール番号に対応した戦闘IDをリストに格納
        List<int> pool = new List<int>(EventsData.Instance.battlIdPool[poolIndex].id);
        List<int> tmpPool = pool;

        //IDを抽選して配列に格納
        for (int i = 0; i < result.Length; i++)
        {
            int index = UnityEngine.Random.Range(0, tmpPool.Count);
            int randomId = tmpPool[index];
            result[i] = randomId;
            tmpPool.RemoveAt(index);

            //リストが空になったら再度IDをリストに格納
            if (tmpPool.Count <= 0) tmpPool = pool;
        }

        return result;
    }

    /// <summary>
    /// イベントのIDを抽選してint型の配列で返す　引数でイベント数を指定
    /// </summary>
    public int[] EventRandomChoice(int num)
    {
        int[] result = new int[num];
        List<int> pool = new List<int>();

        //有効なイベントのIDをリストに格納
        for (int i = 0; i < EventsData.Instance.eventData.Length; i++)
        {
            if (EventActiveCheck(i)) pool.Add(i);
        }

        List<int> tmpPool = pool;

        //IDを抽選して配列に格納
        for (int i = 0; i < result.Length; i++)
        {
            int index = UnityEngine.Random.Range(0, tmpPool.Count);
            int randomId = tmpPool[index];
            result[i] = randomId;
            tmpPool.RemoveAt(index);

            //リストが空になったら再度IDをリストに格納
            if (tmpPool.Count <= 0) tmpPool = pool;
        }

        return result;
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

        SoundManager.Instance.PlaySE_Sys(1);

        //ステータスウィンドウを非表示に
        if (window_Status.gameObject.activeSelf)
        {
            window_Status.isViewStatus = false;
            window_Status.isViewStatusId = false;
            window_Status.ViewUnits();
        }

        //ユニット入手ウィンドウを非表示に
        if (window_GetUnit.gameObject.activeSelf)
        {
            window_GetUnit.ViewUnits();
        }

        //各ウィンドウを非表示に
        window_Event.gameObject.SetActive(false);
        window_GetUnit.gameObject.SetActive(false);
        window_Status.gameObject.SetActive(false);

        canvas[0].SetActive(false);

        if (FindObjectOfType(System.Type.GetType("MapManager")) != null) MapManager.Instance.DebugStageEnd();
        isEvent = false;
    }

    /// <summary>
    /// ユニットのステータスを表示する　引数でIDを指定すると初期ステータスを表示する
    /// </summary>
    public void ViewStatus(int id = -1)
    {
        canvas[0].SetActive(true);

        //ID指定をする場合
        if (id >= 0)
        {
            if (!window_Status.gameObject.activeSelf)
            {
                window_Status.gameObject.SetActive(true);
                window_Status.ViewUnits();
                window_Status.ViewStatusId(id);
            }
            else
            {
                window_Status.ViewUnits();

                //イベント中でない場合はそのままCanvasを閉じる
                if (!isEvent)
                {
                    if (window_Status.isActive) window_Status.ViewUnits();
                    canvas[0].SetActive(false);
                }
            }
        }
        //ID指定をしない場合
        else
        {
            if (!window_Status.gameObject.activeSelf)
            {
                window_Status.gameObject.SetActive(true);
                window_Status.ViewUnits();
            }
            else
            {
                window_Status.ViewUnits();

                //イベント中でない場合はそのままCanvasを閉じる
                if (!isEvent)
                {
                    if (window_Status.isActive) window_Status.ViewUnits();
                    canvas[0].SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// 持っているユニットのステータスを表示する
    /// </summary>
    public void ViewStatusIndex(int index = -1)
    {
        canvas[0].SetActive(true);

        //Index指定をする場合
        if (index >= 0)
        {
            if (!window_Status.gameObject.activeSelf)
            {
                window_Status.gameObject.SetActive(true);
                window_Status.ViewUnits();
                window_Status.ViewStatus(index);
            }
            else
            {
                window_Status.ViewUnits();

                //イベント中でない場合はそのままCanvasを閉じる
                if (!isEvent)
                {
                    if (window_Status.isActive) window_Status.ViewUnits();
                    canvas[0].SetActive(false);
                }
            }
        }
        //Index指定をしない場合
        else
        {
            if (!window_Status.gameObject.activeSelf)
            {
                window_Status.gameObject.SetActive(true);
                window_Status.ViewStatus();
            }
            else
            {
                window_Status.ViewUnits();

                //イベント中でない場合はそのままCanvasを閉じる
                if (!isEvent)
                {
                    if (window_Status.isActive) window_Status.ViewUnits();
                    canvas[0].SetActive(false);
                }
            }
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
    /// 新たなユニットの入手画面を開く　引数で表示するユニットの数を指定
    /// </summary>
    public void GetUnit(int value = -1)
    {
        if (!window_GetUnit.gameObject.activeSelf)
        {
            window_GetUnit.gameObject.SetActive(true);
            window_GetUnit.ViewUnits(value);
        }
        else
        {
            window_GetUnit.ViewUnits();
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
            SoundManager.Instance.PlaySE_Sys(1);

            //ポーズ画面を表示
            canvas[1].SetActive(true);
        }
        //ポーズ終了
        else
        {
            SoundManager.Instance.PlaySE_Sys(2);

            //ポーズ画面を非表示
            canvas[1].SetActive(false);
        }
    }
}
