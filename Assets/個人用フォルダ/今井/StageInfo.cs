using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// ステージの情報を積むスクリプト
/// </summary>
public class StageInfo : MonoBehaviour
{
    [Header("ステージ中の情報")]
    public bool Start;
    public int Stage;//バトルかイベントか
    public int namber;//中の種類
    public string StageName;//テキストで何を行うか
    public string StageNaiyou;//ステージで何をするかイベント専用
    public GameObject[] image;
    public int Enemyint;//敵の数
    public TextMeshProUGUI[] StageInfoText;
    public bool StageEnd;

    void Update()
    {
        StageInfoText[0].text = StageName; //何をするかをテキストで反映
        StageInfoText[1].text = StageNaiyou;//内容
        if (StageName == "バトル")//バトルステージの時は表示
        {
            StageInfoText[1].text = Enemyint.ToString();
        }

        if (Start == true)//ステージをが始まる前
        {
            image[0].SetActive(true);//イメージ用

        }
        if (StageEnd == true)//自分のステージが終わったら消える処理
        {
            Destroy(gameObject);
        }
    }
}
