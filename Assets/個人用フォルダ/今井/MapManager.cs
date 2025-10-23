using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


public class MapManager : MonoBehaviour
{
    public ParameterManager.UnitStatus unitStatus;
    public GameObject[] MapStageImage;//マップで使うプレハブなど
    public GameObject[] MapRoute;//マップのルート
    public GameObject[] MapEnterButton;//マップにあるエンターボタン
    public Transform nextStage;//現在のステージを進めるための場所
    public Transform BossEnemy;//そのフロアのボス

    public int floor;// 現在のフロア数
    public int[] Stageint;//ステージ数
    public int max = 5;//ステージ最大数
    public int min = 2;//ステージ最小数

    public int[] UnitUpGold;//必要ゴールド数
    public int Gold;//(仮)

    private void Start()
    {
        MakeRoute(); //仮
    }
    void Update()
    {
        GoNextStage();
        DontDestroyOnLoad(gameObject);
    }

    public void GoNextStage() //決めたルートのステージを進める処理
    {
        Transform child = nextStage.GetChild(0);
        StageInfo stageinfo = child.GetComponent<StageInfo>();
        if (nextStage != null)
        {
            if (stageinfo != null)
            {
                stageinfo.Start = true;
                Debug.Log("今回は" + stageinfo.Stage + "の" + stageinfo.namber + "です");
            }
            else
            {
                Debug.Log("何も情報なし");
            }
        }
    }
    public void OnButtonPressed(Transform Pos)//ボタンに入っているルートで決める
    {
        if (Pos == null)
        {
            Debug.LogWarning("親の参照が設定されていません！");
            return;
        }

        if (nextStage.childCount == 0)
        {
            if(Pos.childCount == 0)
            {
                Transform Boss = BossEnemy.GetChild(0);
                Boss.SetParent(nextStage, true);
                Boss.localPosition = Vector3.zero;
            }
            Transform child = Pos.GetChild(0);
            child.SetParent(nextStage, true);
            child.localPosition = Vector3.zero;
        }
    }
    public void PassiveeEnterButton()//マップのEnterボタンを消すためのやつ ENTERボタンに付ける用
    {
        if (MapEnterButton == null || MapEnterButton.Length == 0)
        {
            Debug.LogWarning("配列にオブジェクトが設定されていません");
            return;
        }

        foreach (GameObject obj in MapEnterButton)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
    public void SelectRoute(int x)//選んだルート以外をパッシブにするもの　ENTERボタンに付ける用
    {
        // 配列が空の場合は何もしない
        if (MapRoute == null || MapRoute.Length == 0)
        {
            Debug.LogWarning("オブジェクト配列が空です");
            return;
        }

        // keepIndex が範囲外なら修正
        if (x < 0 || x >= MapRoute.Length)
        {
            Debug.LogWarning("keepIndex が範囲外です");
            return;
        }

        for (int i = 0; i < MapRoute.Length; i++)
        {
            if (i != x && MapRoute[i] != null)
            {
                MapRoute[i].SetActive(false);
            }
        }/*一応全部のルートをパッシブにするやつ
        foreach (GameObject obj in MapRoute)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }*/
    }
    public void MakeRoute()//ルート一つのステージを作る
    {
        if (Stageint == null || Stageint.Length == 0)
        {
            Stageint = new int[3];
        }

        for (int i = 0; i < Stageint.Length; i++)
        {
            int randomNum = Random.Range(min, max);
            Stageint[i] = randomNum;
            //Debug.Log($"範囲 {min}～{max} の中から {randomNum} を出力");
        }
        Debug.Log("出力結果一覧:");
        for (int i = 0; i < Stageint.Length; i++)
        {
            Debug.Log($"numbers[{i}] = {Stageint[i]}");
        }
        for (int i = 0; i < Stageint[0]; i++)
        {
            int Stage = Random.Range(0, 2);
            GameObject Route = Instantiate(MapStageImage[Stage], MapRoute[0].transform);
            Debug.Log($"範囲 0～2 の中から {Stage} を出力");
        }

        for (int i = 0; i < Stageint[1]; i++)
        {
            int Stage = Random.Range(0, 2);
            GameObject Route = Instantiate(MapStageImage[Stage], MapRoute[1].transform);
        }

        for (int i = 0; i < Stageint[2]; i++)
        {
            int Stage = Random.Range(0, 2);
            GameObject Route = Instantiate(MapStageImage[Stage], MapRoute[2].transform);
        }
    }
    public void EventContDown()//設置時のコスト　(短縮系)
    {
        unitStatus.cost -= 1;//（仮）
    }
    public void EventrecastDown()//再配置までの時間　(短縮系)
    {
       unitStatus.recast -= 1;//（仮）
    }
    public void EventmaxInstallationUp()//ユニット最大配置数　(増加系)
    {
        ParameterManager.Instance.maxInstallation += 1;//（仮）
    }
    public void EventmaxUnitPossessionUp()//最大ユニット所持数　(増加系)
    {
        ParameterManager.Instance.maxUnitPossession += 1;//（仮）
    }
    public void EventsameUnitMaxInstallationUp()//同じユニットの最大配置数　(増加系)
    {
        ParameterManager.Instance.sameUnitMaxInstallation += 1;//（仮）
    }
    public void Event()//HP回復する（回復）
    {
        ParameterManager.Instance.hp += 1;//（仮）
    }
    public void UnitLevelUpBottun()
    {
        if (Gold >= UnitUpGold[unitStatus.lv])
        {
            Gold -= UnitUpGold[unitStatus.lv];
            unitStatus.lv += 1;//レベル
            unitStatus.hp += 1;//耐久値（最大HP）
            unitStatus.value += 1;//DPSの場合は攻撃力、サポートの場合は回復量、ポイント増加量など
            unitStatus.interval += 1;//行動速度（攻撃、回復をする間隔）
            unitStatus.distance += 1;//攻撃、回復の射程
            unitStatus.range += 1;//範囲攻撃の範囲
            //音も鳴らす
        }
        if(Gold < UnitUpGold[unitStatus.lv])
        {
            //音かテキストを出して出来ないことをしめす
        }
    }
    public void LoadScene(int i)//シーンを流す用　ボタン用
    {
        SceneManager.LoadScene(i);
    }
}
