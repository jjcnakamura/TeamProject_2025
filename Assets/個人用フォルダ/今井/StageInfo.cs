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
    public int Enemyint;//敵の数
    public TextMeshProUGUI[] StageInfoText;
    public bool StageEnd;
    public bool FloorEnd;

    void Update()
    {
        if (StageInfoText[0] != null) StageInfoText[0].text = StageName.ToString(); //何をするかをテキストで反映
        if (StageInfoText[1] != null) StageInfoText[1].text = StageNaiyou.ToString();//内容
        if (StageName == "バトル")//バトルステージの時は表示
        {
            if (StageInfoText[1] != null) StageInfoText[1].text = Enemyint.ToString();
        }
        if (StageEnd == true && FloorEnd == true)//自分のステージが終わったら消える処理
        {
            //Destroy(gameObject);
        }
    }

    public void StageEndDebug()
    {
        StageEnd = true;
    }

    
}
