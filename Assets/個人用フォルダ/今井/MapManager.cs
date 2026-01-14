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

    public TextMeshProUGUI[] MapText;
    public GameObject Map;
    public bool Nextfloorbool;
    public GameObject charaLevelCanvas;
    public GameObject MapTextStageImage;
    public GameObject MapStageMenuButton;
    public Transform PlayerStartPos;
    public GameObject NextFloorButtonImage;
    public GameObject GameEndImage;
    public GameObject EventOBJ;


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
        if (Input.GetKeyDown(KeyCode.F)) GameEndCharaSelectButton();

        Transform[] transforms = new Transform[MapRoute.Length];
        for (int i = 0; i < MapRoute.Length; i++)
        {
            transforms[i] = MapRoute[i].transform;
        }

        Transform bossPis = nextStage.parent;
        Transform boss = bossPis.GetChild(0);
        StageInfo StageInfo = boss.GetComponent<StageInfo>();
        if (StageInfo != null)//ステージを進める際の次のフロアかゲームクリアか分けるところ
        {
            if (StageInfo.FloorEnd == true && StageInfo.StageEnd == true)
            {
                NextFloorButtonImage.SetActive(true);
                if(floor > worldLevel + 2)
                {
                    NextFloorButtonImage.SetActive(false);
                    GameEnd();
                    return;
                }
            }
        }

        if (ParameterManager.Instance.isBattleClear == true)//クリアしたら勝手に次のステージに進むやつ
        {
            Transform child = nextStage.parent;
            StageInfo stageInfo = child.GetComponent<StageInfo>();
            if (stageInfo != null)
            {
                //stageInfo.StageEnd = true;
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
            Transform bossPis = nextStage.parent;
            Transform boss = bossPis.GetChild(0);
            StageInfo stageInfo = boss.GetComponent<StageInfo>();
            if (nextStage != null)
            {
                if (stageInfo != null)
                {
                    stageInfo.Start = true;
                    Debug.Log("今回は" + stageInfo.Stage + "の" + stageInfo.namber + "です");
                }
            }
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

    /*
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
    */

    public void LoadScene(int i)//シーンを流す用　ボタン用
    {
        SceneManager.LoadScene(i);
    }

    public void ButtleLoadSceneAndEvent()//バトルかイベントのシーンを流す　ボタン用
    {
        Transform bossPis = nextStage.parent;
        if (bossPis != null)
        {
            StageInfo StageInfo = bossPis.GetComponent<StageInfo>();
            if (StageInfo != null)
            {
                if (StageInfo.StageName == "バトル")
                {
                    int i = 0;
                    i = StageInfo.namber[0] + 2;
                    SceneManager.LoadScene(i);
                    Debug.Log("ステージは" + i + "をロードしたよ");
                    return;
                }
                if(StageInfo.StageName == "イベント")
                {
                    int i = 0;
                    i = StageInfo.namber[0];
                    EventWindowManager.Instance.CallEventAt(i);
                    Debug.Log("ステージは" + i + "をロードしたよ");
                    return;
                }
            }
            Transform boss = bossPis.GetChild(0);
            StageInfo stageInfo = boss.GetComponent<StageInfo>();
            if (stageInfo != null)
            {
                if (stageInfo.StageName == "ボス")
                {
                    int i = 0;
                    i = stageInfo.namber[0];
                    SceneManager.LoadScene(i);
                    Debug.Log("ステージは" + i + "をロードしたよ");
                }
            }
        }
        
    }

    public void GameStart(GameObject i)//ゲームスタート　ボタン用
    {
        Map.SetActive(true);
        i.gameObject.SetActive(false);
        NextFloor();
    }

    public void BossMake()//難易度参照のボス
    {
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
        StageInfo stageinfo = nextStage.parent.GetComponent<StageInfo>();
        if (stageinfo != null)
        {
            if (stageinfo.Start == true && stageinfo.StageEnd == true)
            {
                MapTextStageImage.SetActive(false);
                MapStageMenuButton.SetActive(false);
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
        Transform bossPis = nextStage.parent;
        Transform boss = bossPis.GetChild(0);
        StageInfo stageInfo = boss.GetComponent<StageInfo>();
        if (stageInfo != null)
        {
            if (stageInfo.Start == true && stageInfo.StageEnd == true)
            {
                MapTextStageImage.SetActive(false);
                MapStageMenuButton.SetActive(false);
            }

            if (stageInfo.Start == true && stageInfo.StageEnd == false)
            {
                MapTextStageImage.SetActive(true);
                MapText[1].text = stageInfo.StageName.ToString();
                MapText[2].text = stageInfo.StageNaiyou.ToString();
                MapText[3].text = stageInfo.Enemyint.ToString();
                MapStageMenuButton.SetActive(true);
            }

            if (stageInfo.Start == false)
            {
                MapTextStageImage.SetActive(false);
            }
        }
    }

    public void MapTextUpdetaButton()//マップのステージのメニューを一時的に閉じるやつ
    {
        Transform stageChild = nextStage.parent;
        StageInfo stageinfo = stageChild.GetComponent<StageInfo>();
        if (stageinfo != null)
        {
            stageinfo.Start = !stageinfo.Start;
            MapStageMenuButton.SetActive(!stageinfo.Start);
            if (stageinfo.Start == true)
            {
                MapStageMenuButton.SetActive(true);
            }
        }
        Transform bossPis = nextStage.parent;
        Transform boss = bossPis.GetChild(0);
        StageInfo stageInfo = boss.GetComponent<StageInfo>();
        if (stageInfo != null)
        {
            stageInfo.Start = !stageInfo.Start;
            MapStageMenuButton.SetActive(!stageInfo.Start);
            if (stageInfo.Start == true)
            {
                MapStageMenuButton.SetActive(true);
            }
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
        Transform bossPis = nextStage.parent;
        Transform boss = bossPis.GetChild(0);
        StageInfo stageInfo = boss.GetComponent<StageInfo>();
        if (stageInfo != null)
        {
            stageInfo.StageEnd = true;
        }
    }

    public void NextFloorButton()//使用中
    {
        Transform bossPis = nextStage.parent;
        Transform boss = bossPis.GetChild(0);
        StageInfo StageInfo = boss.GetComponent<StageInfo>();
        if (StageInfo != null)
        {
            if (StageInfo.FloorEnd == true && StageInfo.StageEnd == true)
            {
                nextStage.SetParent(PlayerStartPos, true);
                nextStage.localPosition = Vector3.zero;

                Transform bossPos = BossEnemy.GetChild(0); 
                Destroy(bossPos.gameObject);

                NextFloor();
                NextFloorButtonImage.SetActive(false);
            }
        }
    }

    public void GameEnd()
    {
        GameEndImage.SetActive(true);
    }

    public void GameEndCanvas()
    {
        var status = ParameterManager.Instance.unitStatus;
        foreach (var ID in status)
        {
            //ID.id
        }
        unitStatus.id = 0;
    }

    public void GameEndCharaSelectButton()//仮
    {
        //EventOBJ.SetActive(true);
        EventWindowManager.Instance.EventRandomChoice(4);
        EventWindowManager.Instance.CallEventAt(4);
    }
}
