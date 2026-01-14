using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// 各イベントのデータ
/// </summary>
public class EventsData : Singleton<EventsData>
{
    public BattlIdPool[] battlIdPool;

    [Space(10)]

    public Content[] eventData;
    public enum Catergory
    {
        短縮系,
        増加系,
        回復,
        複合系
    }
    public enum ContentType
    {
        なし,
        コスト削減,
        再配置短縮,
        ユニット増加,
        所持最大数増加,
        同ユニット配置数増加,
        HP回復
    }

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

        Init();
    }

    //初期化処理
    void Init()
    {
        //戦闘IDのプール
        for (int i = 0; i < battlIdPool.Length; i++)
        {
            //戦闘IDをタイトル、マップのシーン番号を飛ばしたものにする
            for (int j = 0; j < battlIdPool[i].id.Length; j++)
            {
                battlIdPool[i].id[j] = Mathf.Max(Mathf.Min(battlIdPool[i].id[j] + 2, SceneManager.sceneCountInBuildSettings + 2), 2);
            }
        }

        //選択肢関連
        for (int i = 0; i < eventData.Length; i++)
        {
            //選択肢用の配列の要素数を３までに制限
            Array.Resize(ref eventData[i].choice, Mathf.Min(eventData[i].choice.Length, 3));

            //Catergoryに合わせてValueが正か負か決める
            for (int j = 0; j < eventData[i].choice.Length; j++)
            {
                if (eventData[i].catergory != Catergory.複合系)
                {
                    float multiplier = (eventData[i].catergory == Catergory.短縮系) ? -1 : 1;
                    eventData[i].choice[j].value = Mathf.Abs(eventData[i].choice[j].value) * multiplier;
                }
            }
        }
    }

    //戦闘シーンIDのプールの構造体
    [System.Serializable]
    public struct BattlIdPool
    {
        public int[] id;
    }

    //イベントの構造体
    [System.Serializable]
    public struct Content
    {
        [Header("イベントの情報")]
        public string name;
        public Catergory catergory;
        public Sprite sprite;
        [Multiline(2)] public string text;

        [Header("選択肢と内容（3つまで）")]
        public Choice[] choice;
    }
    //イベント内の選択肢
    [System.Serializable]
    public struct Choice
    {
        public ContentType type;
        public string text;
        public float value;
    }
}
