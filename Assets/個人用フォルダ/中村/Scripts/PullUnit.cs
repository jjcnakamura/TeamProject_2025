using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// UI��̃��j�b�g���N���b�N�Ŏ擾���鏈��
/// </summary>
public class PullUnit : MonoBehaviour
{
    public int index;                                 //�ǂ̃��j�b�g��

    [Space(10)]

    [SerializeField] RectTransform image;             //�{�^���̔w�i

    [Space(10)]

    [SerializeField] float noClickOffsetY;            //�N���b�N�s�\�ȏꍇ��Y����������
    Vector3 defaultPos;
    Vector3 noClickPos;

    [Space(10)]

    public           TextMeshProUGUI text_Cost;       //�R�X�g��\���e�L�X�g
    [SerializeField] TextMeshProUGUI text_RecastTime; //���L���X�g���Ԃ�\���e�L�X�g
    [SerializeField] GameObject noClickWindow;        //�N���b�N�s�\�ȏꍇ�ɕ\������I�u�W�F�N�g

    float timer_Recast;                               //���L���X�g�p�^�C�}�[

    //��Ԃ�\���t���O
    bool isNoPoint, isRecast, isDrag;

    void Start()
    {
        //�����蔻��̃T�C�Y�������ڂɍ��킹��
        RectTransform rect = GetComponent<RectTransform>();
        BoxCollider col = GetComponent<BoxCollider>();
        col.size = new Vector3(rect.sizeDelta.x, rect.sizeDelta.y, 1);

        //�N���b�N�s�\���̈ʒu�����߂�
        defaultPos = image.localPosition;
        noClickPos = new Vector3(defaultPos.x, defaultPos.y - noClickOffsetY, defaultPos.z);

        //�N���b�N�s�\�I�u�W�F�N�g���\��
        text_RecastTime.gameObject.SetActive(false);
        noClickWindow.SetActive(false);
    }

    void Update()
    {
        //�R�X�g�p�̃|�C���g������Ă��Ȃ��ꍇ��true�ɂȂ�
        isNoPoint = (BattleManager.Instance.point < BattleManager.Instance.unitCost[index]) ? true : false;

        //�N���b�N�s�\�t���O�������Ă���ꍇ�͈ʒu�������A�N���b�N�s�\�I�u�W�F�N�g��\��
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

        //���̃R���|�[�l���g���烆�j�b�g���h���b�O����Ă���ꍇ
        if (isDrag)
        {
            //���̃R���|�[�l���g�̃��j�b�g���u���ꂽ�ꍇ�̏���
            if (BattleManager.Instance.isUnitPlace)
            {
                //���L���X�g�J�n
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
        //���L���X�g���Ԃ��J�E���g
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

    //�}�E�X�N���b�N�Ń��j�b�g������
    void OnMouseDown()
    {
        if (isNoPoint || isRecast || isDrag) return;

        BattleManager.Instance.PullUnit(index);
        isDrag = true;
    }
}
