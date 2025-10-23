using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapRouteLayout : MonoBehaviour
{
    public GridLayoutGroup grid;//GridLayoutGroup�̎w��

    public int Stageint;
    public bool Start;

    void Update()
    {
        Stageint = transform.childCount;
        Layout();
        if (Start == false)
        {
            Debug.Log("�q�I�u�W�F�N�g�̐�: " + Stageint);
            Start = true;
        }
    }

    public void Layout()
    {
        if (Stageint >= 6)
        {
            int BlockX = Stageint - 5;
            int BlockXY = -10 * BlockX;
            grid.spacing = new Vector2(BlockXY, 0);
        }
    }
}
