using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI上のユニットをクリックで取得する処理
/// </summary>
public class PullUnit : MonoBehaviour
{
    public int index; //どのユニットか

    void Awake()
    {
        //当たり判定のサイズを見た目に合わせる
        BoxCollider col = GetComponent<BoxCollider>();
        RectTransform rect = GetComponent<RectTransform>();
        col.size = new Vector3(rect.sizeDelta.x, rect.sizeDelta.y, 1);
    }

    void OnMouseDown()
    {
        BattleManager.Instance.PullUnit(index);
    }
}
