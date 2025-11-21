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
        if (Stageint == 3)
        {
            int BlockX = 240;
            grid.spacing = new Vector2(BlockX, 0);
            return;
        }
        else if (Stageint == 4)
        {
            int BlockX = 100;
            grid.spacing = new Vector2(BlockX, 0);
            return;
        }
        else if (Stageint == 5)
        {
            int BlockX = 30;
            grid.spacing = new Vector2(BlockX, 0);
        }
    }
}
