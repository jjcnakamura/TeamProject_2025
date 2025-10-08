using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopManager : Singleton<HitStopManager>
{
    [SerializeField, TextArea(1, 5)] string manual;

    float elapsedTime = 0f;     //ヒットストップ時の経過時間
    bool hitStopFLG;
    int settingnumber;

    //ヒットストップの強弱を構造体(Structure)で設定
    [Header("ヒットストップ設定")]
    public HitStopParameter[] hitStop;

    [System.Serializable]
    public struct HitStopParameter
    {
        [SerializeField, Label("ScaleTime")] public float timeScale;   //Time.timeScaleに設定する値 
        [SerializeField, Label("効果時間")] public float slowTime;    //時間を遅くしている時間
    }

    void Start()
    {
    }

    void Update()
    {
        //GameManagerのpuaseフラグがたっていなければ
        //if (!GameManager.Instance.pause) HitStopTime();
    }

    //ヒットストップ発生時
    public void HitStopStart(int number)
    {
        settingnumber = number;
        elapsedTime = 0f;
        hitStopFLG = true;
        Time.timeScale = hitStop[settingnumber].timeScale;
    }

    //ヒットストップ発生後時間計測
    void HitStopTime()
    {
        if (hitStopFLG)
        {
            //時間計測
            elapsedTime += Time.unscaledDeltaTime;

            //時間経過で元の早さに戻る
            if (elapsedTime >= hitStop[settingnumber].slowTime)
            {
                Time.timeScale = 1f;
                hitStopFLG = false;
            }
        }
    }
}
