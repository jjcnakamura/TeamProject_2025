using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject targetUI; // �\���E��\������UI�B��������
    [SerializeField] private GameObject ImageUI;//�傫����
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

    // �}�E�X���������
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetUI != null)
            targetUI.SetActive(true);
        /*if (ImageUI != false)
            targetUI.SetActive(false);*/
    }

    // �}�E�X�����ꂽ��
    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetUI != null)
            targetUI.SetActive(false);
    }
}