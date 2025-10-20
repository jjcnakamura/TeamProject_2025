using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI��̃��j�b�g���N���b�N�Ŏ擾���鏈��
/// </summary>
public class PullUnit : MonoBehaviour
{
    public int index; //�ǂ̃��j�b�g��

    void Awake()
    {
        //�����蔻��̃T�C�Y�������ڂɍ��킹��
        BoxCollider col = GetComponent<BoxCollider>();
        RectTransform rect = GetComponent<RectTransform>();
        col.size = new Vector3(rect.sizeDelta.x, rect.sizeDelta.y, 1);
    }

    void OnMouseDown()
    {
        BattleManager.Instance.PullUnit(index);
    }
}
