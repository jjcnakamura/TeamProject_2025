using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject targetUI; // 表示・非表示するUI
    [SerializeField] private GameObject ImageUI;

    void Update()
    {

    }

    // マウスが乗った時
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetUI != null)
            targetUI.SetActive(true);
        if (ImageUI == true)
            targetUI.SetActive(false);
    }

    // マウスが離れた時
    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetUI != null)
            targetUI.SetActive(false);
    }
}