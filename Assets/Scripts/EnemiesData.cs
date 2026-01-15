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
        public GameObject prefab;  //敵のPrefab
        public Sprite sprite;      //敵の画像
        public string name;        //敵の名前
        public string info;        //敵の情報

        [Space(5)]

        public string anim_W_Name; //移動のアニメーション名
        public string anim_A_Name; //攻撃のアニメーション名
        public float anim_A_Time;  //攻撃のアニメーション時間
        public string anim_D_Name; //ダメージのアニメーション名
    }
}
