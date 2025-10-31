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

    public bool placed;    //配置済みか

    [Space(10)]

    [SerializeField] GameObject unitPointObj; //ユニット設置時の位置
    BoxCollider col;                          //Collider
    public Vector3 unitPoint { get; private set; }

    [SerializeField] GameObject placeGuide; //ユニットドラッグ中に設置可能かを示すオブジェクト
    [SerializeField] GameObject onMouseObj; //マウスホバー時に出現するオブジェクト

    bool unitDrag;         //ユニットがドラッグされているか
    bool onMouse;          //マウスホバー中か
    bool placeOnMouse;     //ユニット配置後にマウスホバー中か

    void Start()
    {
        col = GetComponent<BoxCollider>();

        unitDrag = false;
        placeGuide.SetActive(false);

        onMouse = false;
        onMouseObj.SetActive(onMouse);

        unitPoint = unitPointObj.transform.position;
        Destroy(unitPointObj);
    }

    void Update()
    {
        if (!BattleManager.Instance.isMainGame) return; //メインゲーム中でなければ戻る

        //ユニットがドラッグされた場合は配置可能の表示を出す
        if (!unitDrag && BattleManager.Instance.isUnitDrag)
        {
            unitDrag = true;
            placeGuide.SetActive(unitZone && BattleManager.Instance.place_UnitZone || !unitZone && BattleManager.Instance.place_Floor);
        }
        else if (unitDrag && !BattleManager.Instance.isUnitDrag)
        {
            unitDrag = false;
            placeGuide.SetActive(false);
        }

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

                        //placed = false;
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
