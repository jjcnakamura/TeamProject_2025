using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MapManager : Singleton<MapManager>
{
    public ParameterManager.UnitStatus unitStatus;
    public GameObject[] MapStageImage;//マップで使うプレハブなど
    public GameObject[] MapRoute;//マップのルート ゲームオブジェクト用
    public Transform[] RouteChildren;
    public int x; //マップのルートのint
    public int y; // マップの進行度
    public GameObject[] MapEnterButton;//マップにあるエンターボタン
    public Transform nextStage;//現在のステージを進めるための場所
    public Transform BossEnemy;//そのフロアのボス

    public int floor = 0;// 現在のフロア数
    public int[] Stageint;//ステージ数
    public int worldLevel;//難易度
    public GameObject[] EasyworldBoss;//ボス簡単
    public GameObject[] NormalworldBoss;//ボス普通
    public GameObject[] ExtraworldBoss;//ボス難しい
    public int max = 3;//ステージ作成数

    public GameObject[] Charactermen;
    public TextMeshProUGUI[] MapText;
    public GameObject Map;
    public bool Nextfloorbool;
    public GameObject charaLevelCanvas;
    public GameObject MapTextStageImage;
    public GameObject MapStageMenuButton;
    public Transform PlayerStartPos;

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

    void Update()
    {
        MapText[0].text = floor.ToString();

        if (worldLevel == 0) max = 3;
        if (worldLevel == 1) max = 4;
        if (worldLevel == 2) max = 5;

        var status = ParameterManager.Instance.unitStatus;
        if (Input.GetKeyDown(KeyCode.A)) worldLevel = 0;
        if (Input.GetKeyDown(KeyCode.S)) worldLevel = 1;
        if (Input.GetKeyDown(KeyCode.D)) worldLevel = 2;

        foreach (var ID in status)
        {
            Charactermen[ID.id].SetActive(true);
        }

        Transform[] transforms = new Transform[MapRoute.Length];
        for (int i = 0; i < MapRoute.Length; i++)
        {
            transforms[i] = MapRoute[i].transform;
        }
        if (ParameterManager.Instance.isBattleClear == true)//クリアしたら勝手に次のステージに進むやつ
        {
            Transform child = nextStage.GetChild(0);
            StageInfo stageInfo = child.GetComponent<StageInfo>();
            if (stageInfo != null)
            {
                stageInfo.StageEnd = true;
                ParameterManager.Instance.isBattleClear = false;
            }
        }
        MapTextUpdeta();
    }
    public void ButtonNextStage(int i)
    {
        Transform child = nextStage.GetChild(0);
        StageInfo stageInfo = child.GetComponent<StageInfo>();
        if (stageInfo != null)
        {
            stageInfo.StageEnd = true;
            ParameterManager.Instance.isBattleClear = false;
        }
    }

    public void NextFloor()//フロアを進める処理
    {
        y = 0;
        x = 1;
        Nextfloorbool = false;
        floor += 1;
        ParameterManager.Instance.hp += 3;
        if(ParameterManager.Instance.hp > 10)
        {
            ParameterManager.Instance.hp = 10;
        }
        NextFloorButton();
        foreach (GameObject parent in MapRoute)
        {
            if (parent == null) continue;

            foreach (Transform child in parent.transform)
            {
                Destroy(child.gameObject);
            }
        }
        MakeRoute();
    }

    public void GoNextStage() //決めたルートのステージを進める処理
    {
        Transform[] transforms = new Transform[MapRoute.Length];
        for (int i = 0; i < MapRoute.Length; i++)
        {
            transforms[i] = MapRoute[i].transform;
        }
        if(y > (worldLevel + 2))//ボスに行く処理
        {
            Debug.Log("わわわわわわわわわわわ");
            nextStage.SetParent(BossEnemy, true);
            nextStage.localPosition = Vector3.zero;
            return;
        }
        //普通のステージの処理
        Transform child = transforms[x].GetChild(y);
        nextStage.SetParent(child, true);
        nextStage.localPosition = Vector3.zero;
        StageInfo stageinfo = child.GetComponent<StageInfo>();

        if (nextStage != null)
        {
            if (stageinfo != null)
            {
                stageinfo.Start = true;
                Debug.Log("今回は" + stageinfo.Stage + "の" + stageinfo.namber + "です");
            }
        }
        else
        {
            Debug.Log("何も情報なし");
        }
    }

    public void MakeRoute()//ルートのステージを作る
    {
        if (Stageint == null || Stageint.Length == 0)
        {
            Stageint = new int[3];
        }

        for (int i = 0; i < Stageint.Length; i++)
        {
            Stageint[i] = max;
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
        BossMake();
        foreach (GameObject obj in MapRoute)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
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

    public void UnitLevelUpBottun(int i)//キャラクターのレベルあげる所
    {
        //i はParameterのunitStatusのidを元に動かすもの
        if(ParameterManager.Instance.getExp > 0)
        {
            ParameterManager.Instance.unitStatus[i].exp += 1;
            ParameterManager.Instance.getExp -= 1;
            if (ParameterManager.Instance.unitStatus[i].exp >= 10 * ParameterManager.Instance.unitStatus[i].lv)
            {
                ParameterManager.Instance.LevelUp(i);
            }
            UnitLevelAndExp.Instance.NewStatus();
        }
        else
        {
            //音かテキストを出して出来ないことをしめす
        }
    }

    public void LoadScene(int i)//シーンを流す用　ボタン用
    {
        SceneManager.LoadScene(i);
    }

    public void GameStart(GameObject i)//ゲームスタート　ボタン用
    {
        Map.SetActive(true);
        i.gameObject.SetActive(false);
        NextFloor();
    }

    public void BossMake()//難易度参照のボス
    {
        Debug.Log("通ってるよ");
        if (worldLevel == 0)
        {
            int i = floor - 1;
            GameObject Boss = Instantiate(EasyworldBoss[i], BossEnemy.transform);
        }
        if (worldLevel == 1)
        {
            int i = floor - 1;
            GameObject Boss = Instantiate(NormalworldBoss[i], BossEnemy.transform);
        }
        if (worldLevel == 2)
        {
            int i = floor - 1;
            GameObject Boss = Instantiate(ExtraworldBoss[i], BossEnemy.transform);
        }
    }

    public void CharaLevelCanvas(bool i)
    {
        charaLevelCanvas.SetActive(i);
        UnitLevelAndExp.Instance.NewStatus();
    }

    public void MapTextUpdeta()//マップのステージ情報出すやつ
    {
        Transform stageChild = nextStage.parent;
        StageInfo stageinfo = stageChild.GetComponent<StageInfo>();
        if (stageinfo != null)
        {
            if (stageinfo.Start == true && stageinfo.StageEnd == true)
            {
                MapTextStageImage.SetActive(false);
            }

            if (stageinfo.Start == true && stageinfo.StageEnd == false)
            {
                MapTextStageImage.SetActive(true);
                MapText[1].text = stageinfo.StageName.ToString();
                MapText[2].text = stageinfo.StageNaiyou.ToString();
                MapText[3].text = stageinfo.Enemyint.ToString();
                MapStageMenuButton.SetActive(true);
            }

            if(stageinfo.Start == false)
            {
                MapTextStageImage.SetActive(false);
            }
        }
    }

    public void MapTextUpdetaButton()//マップのステージのメニューを一時的に閉じるやつ
    {
        Transform stageChild = nextStage.transform;
        Transform parent = stageChild.parent;
        StageInfo stageinfo = parent.GetComponent<StageInfo>();
        stageinfo.Start = !stageinfo.Start;
        MapStageMenuButton.SetActive(!stageinfo.Start);
        if (stageinfo.Start == true)
        {
            MapStageMenuButton.SetActive(true);
        }
    }

    public void DebugStageEnd()//デバッグ用　ステージ終わらせるやつ
    {
        Transform stageChild = nextStage.transform;
        Transform parent = stageChild.parent;
        StageInfo stageinfo = parent.GetComponent<StageInfo>();
        if (stageinfo != null)
        {
            stageinfo.StageEnd = true;
        }
    }

    public void NextFloorButton()
    {
        Transform stageChild = nextStage.transform;
        Transform parent = stageChild.parent;
        Transform child = parent.GetChild(0);
        StageInfo stageinfo = child.GetComponent<StageInfo>();
        if (stageinfo != null)
        {
            if(y > (worldLevel + 2))
            {
                nextStage.SetParent(PlayerStartPos, true);
                nextStage.localPosition = Vector3.zero;
                Transform boss = BossEnemy.GetChild(0);
                Destroy(boss);
            }
        }
    }
}
