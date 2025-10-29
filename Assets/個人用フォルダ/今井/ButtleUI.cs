using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject targetUI; // 表示・非表示するUI。小さい方
    [SerializeField] private GameObject ImageUI;//大きい方
    StageInfo stageInfo;
    public GameObject DESTROY;

    void Start()
    {
        stageInfo = this.GetComponent<StageInfo>();
    }
    void Update()
    {
        if(stageInfo.Start == true)
        {
            Destroy(DESTROY);
        }
    }

    // マウスが乗った時
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetUI != null)
            targetUI.SetActive(true);
        /*if (ImageUI != false)
            targetUI.SetActive(false);*/
    }

    // マウスが離れた時
    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetUI != null)
            targetUI.SetActive(false);
    }
}