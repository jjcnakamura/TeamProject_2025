using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MouseManager : Singleton<MouseManager>
{
    //RayをSceneビュー上で表示するか
    [SerializeField] bool viewRay = false;

    //マウスの座標
    public Vector3 mousePos { get; private set; }
    public Vector3 worldPos { get; private set; }

    //マウスのRayに衝突しているClickObjタグ付きオブジェクト
    public RaycastHit mouseRayHits { get; private set; }

    void Update()
    {
        //マウスカーソルの座標を格納
        mousePos = Input.mousePosition;
        //スクリーン座標をワールド座標に変換
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        ////////////////////////////////////////////////////////////////////////////////////

        //マウスの位置からRayを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        //TagがClickObjのオブジェクトのみ参照
        var clickables = hits
            .Where(h => h.collider.CompareTag("ClickObj"))
            .OrderBy(h => h.distance)  //手前（距離が短い）順にソート
            .ToArray();

        //OnMouseEnter
        if (clickables.Length > 0)
        {
            mouseRayHits = clickables[0];
        }
        //OnMouseExit
        else
        {
            mouseRayHits = new RaycastHit();
        }

        //デバッグ用 Rayを表示
        if (viewRay)
        {
            float maxDistance = 20;
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green, 5, false);
        }
    }
}
