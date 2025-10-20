using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ユニットをステージに配置できるか判定する処理
/// </summary>
public class UnitZone : MonoBehaviour
{
    public int index;       //どの配置場所か
    public bool unitZone;   //敵の通る道ではないユニット設置専用の場所か

    [Space(10)]
    public bool placed;    //配置済みか

    BoxCollider col;         //Collider
    GameObject unitPointObj; //ユニット設置時の位置
    public Vector3 unitPoint { get; private set; }

    GameObject onMouseObj; //マウスホバー時に出現するオブジェクト
    bool onMouse;          //マウスホバー中か
    bool placeOnMouse;     //ユニット配置後にマウスホバー中か

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
                //ドラッグしているユニットを置く処理
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
                //置いているユニットを削除する処理
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
