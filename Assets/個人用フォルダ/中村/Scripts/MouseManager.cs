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
        //マウスの位置からRayを飛ばす
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        CheckMousePos(inputRay);
        CheckRayCollision(inputRay);
        ViewRay(inputRay);
    }

    //マウスカーソルの位置を取得
    void CheckMousePos(Ray ray)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000f, LayerMask.GetMask("Ground"));

        if (hits.Length > 0)
        {
            //手前（距離が短い）順にソート
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            RaycastHit nearest = hits[0];

            worldPos = nearest.point;
        }
    }

    //Rayに衝突しているオブジェクトを取得
    void CheckRayCollision(Ray ray)
    {
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
    }

    //デバッグ用 Rayを表示
    void ViewRay(Ray ray)
    {
        if (viewRay)
        {
            float maxDistance = 20;
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green, 5, false);
        }
    }
}
