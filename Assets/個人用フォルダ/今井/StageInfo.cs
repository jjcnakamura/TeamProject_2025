using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// ステージの情報を積むスクリプト
/// </summary>
public class StageInfo : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler
{
    [Header("ステージ中の情報")]
    public bool Start;
    public int Stage;//バトルかイベントか
    public int[] namber;//中の種類
    public string StageName;//テキストで何を行うか
    public string StageNaiyou;//ステージで何をするかイベント専用
    public int Enemyint;//敵の数
    public TextMeshProUGUI[] StageInfoText;
    public bool StageEnd;
    public bool FloorEnd;
    public GameObject NextButton;
    public GameObject IgnoreImage;
    public GameObject targetUI;
    public bool i = false;
    public int indexint;

    void Awake()
    {
        //EventWindowManager.Instance.BattleRandomChoice(int namber,int Stage);
        if(StageName == "イベント")
        {
            namber = EventWindowManager.Instance.EventRandomChoice(1);
        }
        if (StageName == "バトル")
        {
            //namber = EventWindowManager.Instance.BattleRandomChoice(1, MapManager.Instance.worldLevel);
            if (MapManager.Instance.floor <= 2)
            {
                namber[0] = ((MapManager.Instance.floor - 1) * 3) + Random.Range(1, 4);
            }
            else
            {
                namber[0] = Random.Range(7, 11);
            }
            
        }
        if (StageName == "ボス")
        {
            //namber[0] = MapManager.Instance.worldLevel + MapManager.Instance.floor;
        }
    }

    void Update()
    {
        if (StageInfoText[0] != null) StageInfoText[0].text = StageName.ToString(); //何をするかをテキストで反映
        if (StageInfoText[1] != null)
        {
            if(StageName == "イベント")
            {
                StageInfoText[1].text = EventsData.Instance.eventData[namber[0]].name;//内容
            }
            if (StageName == "バトル")
            {
                //StageInfoText[1].text = Enemyint.ToString();
                StageInfoText[1].text = namber[0].ToString();
            }
        }
        if (StageEnd == true && Start == true)//自分のステージが終わったら消える処理
        {
            if(i == false)
            {
                MapManager.Instance.y += 1;
                i = true;
            }
        }
        if(FloorEnd == false)
        {
            int index = GetParentIndexOf(this.transform);//親から見て何番目の子か

            indexint = transform.GetSiblingIndex();
            
        }
        if (FloorEnd == true)
        {
            indexint = MapManager.Instance.worldLevel + 3;
        }
        if (MapManager.Instance.y > indexint)
        {
            IgnoreImage.SetActive(true);
            if(StageEnd == true)
            {
                IgnoreImage.SetActive(false);
            }
        }
    }

    public void StageEndDebug()
    {
        StageEnd = true;
    }

    public int GetParentIndexOf(Transform child)//今ステージがどこのルートにいるか
    {
        Transform childs = this.gameObject.transform;
        Transform parent = childs.parent;

        for (int i = 0; i < MapManager.Instance.MapRoute.Length; i++)
        {
            if (MapManager.Instance.MapRoute[i] != null && MapManager.Instance.MapRoute[i].transform == parent)
            {
                return i;   // 何番目の親なので返す
            }
        }
        if(FloorEnd == true)
        {
            Debug.Log("私はボスです");
        }
        return -1;
    }

    public void GetStageIndex()//今の親ルートのステージが何番目か
    {
        int indexint = transform.GetSiblingIndex();
        MapManager.Instance.y = indexint;
    }

    public void GoNextStageButton()
    {
        SoundManager.Instance.PlaySE_Sys(1);
        int index = GetParentIndexOf(this.transform);
        Debug.Log("次のルートは" + index);
        MapManager.Instance.x = index;
        MapManager.Instance.GoNextStage();
    }

    //マウスが重なった時
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetUI != null)
            targetUI.SetActive(true);

        int index = GetParentIndexOf(this.transform);//今ステージがどこのルートにいるか
        if (MapManager.Instance.y == indexint)
        {
            if (MapManager.Instance.x == index || MapManager.Instance.x == index - 1 || MapManager.Instance.x == index + 1)
            {
                if (NextButton != null)
                {
                    targetUI.SetActive(true);
                    NextButton.SetActive(true);
                }
            }
            if (FloorEnd == true)
            {
                if (NextButton != null)
                {
                    targetUI.SetActive(true);
                    NextButton.SetActive(true);
                }
            }
        }

        Transform ThisObject = this.transform;
        if (ThisObject.childCount >= 6)
        {
            Transform player = this.transform.GetChild(5);
            if (player != null)
            {
                targetUI.SetActive(false);
            }
        }

        Transform Player = MapManager.Instance.nextStage.parent;
        StageInfo stageinfo = Player.GetComponent<StageInfo>();
        if (stageinfo != null)
        {
            if(stageinfo.Start == false)
            {
                NextButton.SetActive(false);
            }
        }
    }

    // マウスが離れた時
    public void OnPointerExit(PointerEventData eventData)
    {
        if (NextButton != null)
            NextButton.SetActive(false);
        if (targetUI != null)
            targetUI.SetActive(false);
    }

    public void ButtleIndex()
    {
        
    }
}
