using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���j�b�g���X�e�[�W�ɔz�u�ł��邩���肷�鏈��
/// </summary>
public class UnitZone : MonoBehaviour
{
    public int index;       //�ǂ̔z�u�ꏊ��
    public bool unitZone;   //�G�̒ʂ铹�ł͂Ȃ����j�b�g�ݒu��p�̏ꏊ��

    public bool placed;    //�z�u�ς݂�

    [Space(10)]

    [SerializeField] GameObject unitPointObj; //���j�b�g�ݒu���̈ʒu
    BoxCollider col;                          //Collider
    public Vector3 unitPoint { get; private set; }

    [SerializeField] GameObject onMouseObj; //�}�E�X�z�o�[���ɏo������I�u�W�F�N�g
    bool onMouse;          //�}�E�X�z�o�[����
    bool placeOnMouse;     //���j�b�g�z�u��Ƀ}�E�X�z�o�[����

    void Start()
    {
        col = GetComponent<BoxCollider>();

        onMouse = false;
        onMouseObj.SetActive(onMouse);

        unitPoint = unitPointObj.transform.position;
        Destroy(unitPointObj);
    }

    void Update()
    {
        if (!BattleManager.Instance.isMainGame) return; //���C���Q�[�����łȂ���Ζ߂�

        //OnMouseOver
        if (onMouse)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                //�h���b�O���Ă��郆�j�b�g��u������
                if (BattleManager.Instance.isUnitDrag)
                {
                    if (unitZone && BattleManager.Instance.place_UnitZone || !unitZone && BattleManager.Instance.place_Floor)
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
                    else
                    {
                        BattleManager.Instance.LetgoUnit();
                        onMouseObj.SetActive(false);
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
                if (BattleManager.Instance.isUnitDrag && !placed)
                {
                    if (unitZone && BattleManager.Instance.place_UnitZone || !unitZone && BattleManager.Instance.place_Floor)
                    {
                        onMouseObj.SetActive(onMouse);
                    }     
                }
                else if (!BattleManager.Instance.isUnitDrag && placed)
                {
                    onMouseObj.SetActive(onMouse);
                }

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
}
