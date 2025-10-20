using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ユニットをステージに配置する処理
/// </summary>
public class UnitZone : MonoBehaviour
{
    public int index;      //どの配置場所か
    public bool placed;    //配置済みか

    BoxCollider col;       //Collider

    GameObject unitPointObj; //ユニットの設置場所
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

    //OnMouse関数の代わりにRayを使う

    /*
    void OnMouseOver()
    {
        //ドラッグしているユニットを置く処理
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
        //置いているユニットを削除する処理
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
