using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datamanager : Singleton<Datamanager>
{
    void Awake()
    {
        if (gameObject.transform.parent != null) gameObject.transform.parent = null;
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        //セーブデータがない場合は生成する
        if (PlayerPrefs.GetInt("DataExist") <= 0) DataInit();
    }

    void Start()
    {
        //FPSを固定
        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// セーブデータの初期化
    /// </summary>
    public void DataInit()
    {
        int bgmVol = PlayerPrefs.GetInt("BGMVol");
        int seVol = PlayerPrefs.GetInt("SEVol");

        if (PlayerPrefs.GetInt("DataExist") <= 0)
        {
            bgmVol = 8;
            seVol = 8;
        }

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("DataExist", 1);

        PlayerPrefs.SetInt("BGMVol", bgmVol);
        PlayerPrefs.SetInt("SEVol", seVol);
    }
}
