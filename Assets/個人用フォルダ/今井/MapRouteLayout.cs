using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapRouteLayout : MonoBehaviour
{
    public GridLayoutGroup grid;//GridLayoutGroupの指定

    public int Stageint;
    public bool Start;

    void Update()
    {
        Stageint = transform.childCount;
        Layout();
        if (Start == false)
        {
            Debug.Log("子オブジェクトの数: " + Stageint);
            Start = true;
        }
    }

    public void Layout()
    {
        if (Stageint >= 8)
        {
            int BlockX = Stageint - 4;
            int BlockXY = -10 * BlockX;
            grid.spacing = new Vector2(BlockXY, 0);
        }
    }
}
