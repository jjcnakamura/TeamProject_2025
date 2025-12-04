using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtelUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject targetUI; // 表示・非表示するUI。小さい方


    void Update()
    {

    }

    // マウスが乗った時
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetUI != null)
            targetUI.SetActive(true);
        Transform Player = this.transform.GetChild(2);
        if (Player != null)
        {
            targetUI.SetActive(false);
        }
    }

    // マウスが離れた時
    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetUI != null)
            targetUI.SetActive(false);
    }
}