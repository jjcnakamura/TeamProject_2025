using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject targetUI; // �\���E��\������UI
    [SerializeField] private GameObject ImageUI;

    void Update()
    {

    }

    // �}�E�X���������
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetUI != null)
            targetUI.SetActive(true);
        if (ImageUI == true)
            targetUI.SetActive(false);
    }

    // �}�E�X�����ꂽ��
    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetUI != null)
            targetUI.SetActive(false);
    }
}