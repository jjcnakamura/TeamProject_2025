using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 各敵の名前と説明
/// </summary>
public class EnemiesData : Singleton<EnemiesData>
{
    public Status[] enemy;

    void Awake()
    {
        //シーンを遷移しても残る
        if (gameObject.transform.parent != null) gameObject.transform.parent = null;
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    //ステータスの構造体
    [System.Serializable]
    public struct Status
    {
        public string name;
        public string info;
        public int actId;
    }
}
