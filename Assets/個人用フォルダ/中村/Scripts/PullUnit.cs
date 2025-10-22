using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// UI上のユニットをクリックで取得する処理
/// </summary>
public class PullUnit : MonoBehaviour
{
    public int index;                                 //どのユニットか

    [Space(10)]

    [SerializeField] RectTransform image;             //ボタンの背景

    [Space(10)]

    [SerializeField] float noClickOffsetY;            //クリック不可能な場合にY軸を下げる
    Vector3 defaultPos;
    Vector3 noClickPos;

    [Space(10)]

    public           TextMeshProUGUI text_Cost;       //コストを表すテキスト
    [SerializeField] TextMeshProUGUI text_RecastTime; //リキャスト時間を表すテキスト
    [SerializeField] GameObject noClickWindow;        //クリック不可能な場合に表示するオブジェクト

    float timer_Recast;                               //リキャスト用タイマー

    //状態を表すフラグ
    bool isNoPoint, isRecast, isDrag;

    void Start()
    {
        //当たり判定のサイズを見た目に合わせる
        RectTransform rect = GetComponent<RectTransform>();
        BoxCollider col = GetComponent<BoxCollider>();
        col.size = new Vector3(rect.sizeDelta.x, rect.sizeDelta.y, 1);

        //クリック不可能時の位置を決める
        defaultPos = image.localPosition;
        noClickPos = new Vector3(defaultPos.x, defaultPos.y - noClickOffsetY, defaultPos.z);

        //クリック不可能オブジェクトを非表示
        text_RecastTime.gameObject.SetActive(false);
        noClickWindow.SetActive(false);
    }

    void Update()
    {
        //コスト用のポイントが足りていない場合はtrueになる
        isNoPoint = (BattleManager.Instance.point < BattleManager.Instance.unitCost[index]) ? true : false;

        //クリック不可能フラグが立っている場合は位置を下げ、クリック不可能オブジェクトを表示
        if  (isNoPoint && !noClickWindow.activeSelf || isRecast && !noClickWindow.activeSelf)
        {
            image.localPosition = noClickPos;
            noClickWindow.SetActive(true);
        }
        else if (!isNoPoint && !isRecast && noClickWindow.activeSelf)
        {
            image.localPosition = defaultPos;
            noClickWindow.SetActive(false);
        }                       

        //このコンポーネントからユニットがドラッグされている場合
        if (isDrag)
        {
            //このコンポーネントのユニットが置かれた場合の処理
            if (BattleManager.Instance.isUnitPlace)
            {
                //リキャスト開始
                timer_Recast = BattleManager.Instance.unitRecast[index] - Time.fixedDeltaTime;
                text_RecastTime.gameObject.SetActive(true);
                isRecast = true;

                BattleManager.Instance.isUnitPlace = false;
                isDrag = false;
            }
            else if (!BattleManager.Instance.isUnitDrag)
            {
                isDrag = false;
            }
        }
    }

    void FixedUpdate()
    {
        //リキャスト時間をカウント
        if (isRecast)
        {
            if (timer_Recast > 0)
            {
                timer_Recast -= Time.fixedDeltaTime;
                text_RecastTime.text = Mathf.Max(Mathf.CeilToInt(timer_Recast), 1f).ToString();
            }
            else
            {
                text_RecastTime.gameObject.SetActive(false);
                isRecast = false;
            }
        }
    }

    //マウスクリックでユニットを持つ
    void OnMouseDown()
    {
        if (isNoPoint || isRecast || isDrag) return;

        BattleManager.Instance.PullUnit(index);
        isDrag = true;
    }
}
