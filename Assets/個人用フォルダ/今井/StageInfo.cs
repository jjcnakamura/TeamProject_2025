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
    public int namber;//中の種類
    public string StageName;//テキストで何を行うか
    public string StageNaiyou;//ステージで何をするかイベント専用
    public int Enemyint;//敵の数
    public TextMeshProUGUI[] StageInfoText;
    public bool StageEnd;
    public bool FloorEnd;
    public GameObject NextButton;

    void Update()
    {
        if (StageInfoText[0] != null) StageInfoText[0].text = StageName.ToString(); //何をするかをテキストで反映
        if (StageInfoText[1] != null) StageInfoText[1].text = StageNaiyou.ToString();//内容
        if (StageName == "バトル")//バトルステージの時は表示
        {
            if (StageInfoText[1] != null) StageInfoText[1].text = Enemyint.ToString();
        }
        if (StageEnd == true && Start == true)//自分のステージが終わったら消える処理
        {
            bool i = false;
            if(i == false)
            {
                MapManager.Instance.y += 1;
                i = true;
            }
        }
        int index = GetParentIndexOf(this.transform);//親から見て何番目の子か
        Debug.Log(index);

        int indexint = transform.GetSiblingIndex();
        Debug.Log(indexint);
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
        int index = GetParentIndexOf(this.transform);
        Debug.Log("次のルートは" + index);
        MapManager.Instance.x = index;
        MapManager.Instance.GoNextStage();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        int index = GetParentIndexOf(this.transform);//今ステージがどこのルートにいるか
        int indexint = transform.GetSiblingIndex();//今の親ルートのステージが何番目か
        if (MapManager.Instance.y == indexint)
        {
            if (MapManager.Instance.x == index || MapManager.Instance.x == index - 1 || MapManager.Instance.x == index + 1)
            {
                if (NextButton != null) NextButton.SetActive(true);
            }
        }
    }

    // マウスが離れた時
    public void OnPointerExit(PointerEventData eventData)
    {
        if (NextButton != null) NextButton.SetActive(false);
    }
}
