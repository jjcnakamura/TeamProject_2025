using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���j�b�g���X�e�[�W�ɔz�u���鏈��
/// </summary>
public class UnitZone : MonoBehaviour
{
    public int index;      //�ǂ̔z�u�ꏊ��
    public bool placed;    //�z�u�ς݂�

    BoxCollider col;       //Collider

    GameObject unitPointObj; //���j�b�g�̐ݒu�ꏊ
    public Vector3 unitPoint { get; private set; }

    GameObject onMouseObj; //�}�E�X�z�o�[���ɏo������I�u�W�F�N�g
    bool onMouse;          //�}�E�X�z�o�[����
    bool placeOnMouse;     //���j�b�g�z�u��Ƀ}�E�X�z�o�[����

    void Awake()
    {
        col = GetComponent<BoxCollider>();

        onMouseObj = transform.GetChild(1).gameObject;
        onMouse = false;
        onMouseObj.SetActive(onMouse);

        unitPointObj = transform.GetChild(0).gameObject;
        unitPoint = unitPointObj.transform.position;
        Destroy(unitPointObj);
    }

    void Update()
    {
        //OnMouseOver
        if (onMouse)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                //�h���b�O���Ă��郆�j�b�g��u������
                if (BattleManager.Instance.isUnitDrag)
                {
                    if (!placed)
                    {
                        BattleManager.Instance.PlaceUnit(index);
                        onMouse = false;
                        onMouseObj.SetActive(onMouse);

                        placeOnMouse = true;
                        placed = true;
                    }
                    else
                    {
                        BattleManager.Instance.LetgoUnit();
                        onMouseObj.SetActive(onMouse);
                    }
                }
                //�u���Ă��郆�j�b�g���폜���鏈��
                else
                {
                    if (placed)
                    {
                        BattleManager.Instance.OutUnit(index);
                        onMouse = false;
                        onMouseObj.SetActive(onMouse);

                        placed = false;
                    }
                }
            }
        }
        //OnMouseEnter
        if (!onMouse && col == MouseManager.Instance.mouseRayHits.collider)
        {
            if (!placeOnMouse)
            {
                onMouse = true;
                if (BattleManager.Instance.isUnitDrag && !placed || !BattleManager.Instance.isUnitDrag && placed)
                    onMouseObj.SetActive(onMouse);

                BattleManager.Instance.isOnMouseUnitZone = true;
            }   
        }
        //OnMouseExit
        else if (onMouse && col != MouseManager.Instance.mouseRayHits.collider || placeOnMouse && col != MouseManager.Instance.mouseRayHits.collider)
        {
            placeOnMouse = false;
            onMouse = false;
            onMouseObj.SetActive(onMouse);

            BattleManager.Instance.isOnMouseUnitZone = false;
        }
    }

    //OnMouse�֐��̑����Ray���g��

    /*
    void OnMouseOver()
    {
        //�h���b�O���Ă��郆�j�b�g��u������
        if (Input.GetKeyUp(KeyCode.Mouse0) && BattleManager.Instance.isUnitDrag)
        {
            if (!placed)
            {
                BattleManager.Instance.PlaceUnit(index);
                onMouse = false;
                onMouseObj.SetActive(onMouse);

                placed = true;
            }
            else
            {
                BattleManager.Instance.LetgoUnit();
                onMouseObj.SetActive(onMouse);
            }
        }
    }

    void OnMouseUpAsButton()
    {
        //�u���Ă��郆�j�b�g���폜���鏈��
        if (Input.GetKeyUp(KeyCode.Mouse0) && !BattleManager.Instance.isUnitDrag)
        {
            if (placed && onMouse)
            {
                BattleManager.Instance.OutUnit(index);
                onMouse = false;
                onMouseObj.SetActive(onMouse);

                placed = false;
            }
        }
    }

    void OnMouseEnter()
    {
        if (BattleManager.Instance.isUnitDrag && !placed || !BattleManager.Instance.isUnitDrag && placed)
        {
            onMouse = true;
            onMouseObj.SetActive(onMouse);
        }
        else if (BattleManager.Instance.isUnitDrag)
        {
            onMouse = true;
        }
        
        BattleManager.Instance.isOnMouseUnitZone = true;
    }

    void OnMouseExit()
    {
        onMouse = false;
        onMouseObj.SetActive(onMouse);

        BattleManager.Instance.isOnMouseUnitZone = false;
    }
    */
}
