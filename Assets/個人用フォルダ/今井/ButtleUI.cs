using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtelUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject targetUI; // 表示・非表示するUI。小さい方
    StageInfo stageInfo;

    void Start()
    {
        stageInfo = this.GetComponent<StageInfo>();
    }
    void Update()
    {
        if(stageInfo.Start == true)
        {
            targetUI.SetActive(false);
        }
    }

    // マウスが乗った時
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(stageInfo.Start == false || stageInfo.StageEnd == true)
        {
            if (targetUI != null)
                targetUI.SetActive(true);
        }
    }

    // マウスが離れた時
    public void OnPointerExit(PointerEventData eventData)
    {
        if (stageInfo.Start == false || stageInfo.StageEnd == true)
        {
            if (targetUI != null)
                targetUI.SetActive(false);
        }
    }
}