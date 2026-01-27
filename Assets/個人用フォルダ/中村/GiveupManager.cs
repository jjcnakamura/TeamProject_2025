using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// ギブアップ処理用
/// </summary>
public class GiveupManager : Singleton<GiveupManager>
{
    [SerializeField] GameObject giveupCanvas;

    MapManager mapManager;

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

        //マップを取得
        if (FindObjectOfType(System.Type.GetType("MapManager")) != null)
        {
            mapManager = MapManager.Instance;
        }
    }

    /// <summary>
    /// あきらめるボタン
    /// </summary>
    public void GiveupWindow()
    {
        if (!giveupCanvas.activeSelf)
        {
            SoundManager.Instance.PlaySE_Sys(1);
            giveupCanvas.SetActive(true);
        }
        else
        {
            SoundManager.Instance.PlaySE_Sys(2);
            giveupCanvas.SetActive(false);
        }
    }

    /// <summary>
    /// タイトルに戻る(データは保存されない)
    /// </summary>
    public void BackTitle()
    {
        SoundManager.Instance.PlaySE_Sys(1);

        //全てのステータスをリセット
        ParameterManager.Instance.StatusInit();

        //タイトルに戻った時にMapManagerを消すためにアクティブにしておく
        if (mapManager != null) mapManager.gameObject.SetActive(true);
        else if (FindObjectOfType(System.Type.GetType("MapManager")) != null) MapManager.Instance.gameObject.SetActive(true);
        else if (FindObjectOfType(System.Type.GetType("BattleManager")) != null && BattleManager.Instance.mapManager) BattleManager.Instance.mapManager.gameObject.SetActive(true);

        FadeManager.Instance.LoadSceneIndex(0, 0.5f);
    }
}
