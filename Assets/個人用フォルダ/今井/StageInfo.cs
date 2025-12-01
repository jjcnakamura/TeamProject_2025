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
        if (StageEnd == true && FloorEnd == true)//自分のステージが終わったら消える処理
        {
            //Destroy(gameObject);
        }
        int index = GetParentIndexOf(this.transform);
        Debug.Log(index);

        int indexint = transform.GetSiblingIndex();
        Debug.Log(indexint);
    }

    public void StageEndDebug()
    {
        StageEnd = true;
    }

    public void GoSelectRoute()
    {
        int index = GetParentIndexOf(this.transform);
        MapManager.Instance.x = index;
    }

    public int GetParentIndexOf(Transform child)//どこのルート(番号)にいるステージか
    {
        Transform childs = this.gameObject.transform;
        Transform parent = childs.parent;

        for (int i = 0; i < MapManager.Instance.MapRoute.Length; i++)
        {
            if (MapManager.Instance.MapRoute[i] != null && MapManager.Instance.MapRoute[i].transform == parent)
            {
                MapManager.Instance.x = i;
                return i;   // 何番目の親なので返す
            }
        }
        if(FloorEnd == true)
        {
            Debug.Log("私はボスです");
        }
        return -1;
    }

    public void GetStageIndex()//今のルートのステージが何番目か
    {
        int indexint = transform.GetSiblingIndex();
        MapManager.Instance.y = indexint;
    }

    public void GoNextStageButton()
    {
        int indexint = transform.GetSiblingIndex();
        MapManager.Instance.GoNextStage();
        MapManager.Instance.x = indexint;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        int index = GetParentIndexOf(this.transform);
        int indexint = transform.GetSiblingIndex();
        int[] around = { indexint - 1, indexint, indexint + 1 };
        if (MapManager.Instance.y == index)
        {
            if (MapManager.Instance.x == indexint)
            {
                if (NextButton != null) NextButton.SetActive(true);
                return;
            }
            if (MapManager.Instance.x == indexint - 1)
            {
                if (NextButton != null) NextButton.SetActive(true);
                return;
            }
            if (MapManager.Instance.x == indexint + 1)
            {
                if (NextButton != null) NextButton.SetActive(true);
                return;
            }
        }
    }

    // マウスが離れた時
    public void OnPointerExit(PointerEventData eventData)
    {
        int index = GetParentIndexOf(this.transform);
        int indexint = transform.GetSiblingIndex();
        if (MapManager.Instance.y == index && MapManager.Instance.x == indexint)
        {
            if (NextButton != null) NextButton.SetActive(false);
        }
    }
}
