using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    //--------------------------------------
    //ゲームに必要なエフェクトを管理する
    //必要な時に各スクリプトから呼び出せるようにしておく
    //--------------------------------------

    [Header("Playerの挙動に関するエフェクト")]
    public GameObject[] player;

    [Header("敵に関するエフェクト")]
    public GameObject[] enemy;

    [Header("ステージに関するエフェクト")]
    public GameObject[] stage;

    [Header("進行等に関わるエフェクト")]
    public GameObject[] other;

    //個別にパーティクルのコンポーネントを取得したい時
    //public ParticleSystem fx001;


    void Start()
    {
        //fx001 = GetComponent<ParticleSystem>();
    }

}
