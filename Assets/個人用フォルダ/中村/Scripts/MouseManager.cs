using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : Singleton<MouseManager>
{
    //マウスの座標
    public Vector3 mousePos { get; private set; }
    public Vector3 worldPos { get; private set; }

    //現在マウスが当たっているオブジェクト
    HashSet<GameObject> currentMouseHits = new HashSet<GameObject>();

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
        //今フレームでヒットしたRay衝突オブジェクト
        HashSet<GameObject> newHits = new HashSet<GameObject>();

        //OnMouseEnter
        foreach (RaycastHit hit in hits)
        {
            //クリック可能オブジェクトにマウスが乗った場合
            if (hit.collider.CompareTag("ClickObj"))
            {
                newHits.Add(hit.collider.gameObject);

                //新しくマウスが乗ったオブジェクト（前フレームにはなかった）
                if (!currentMouseHits.Contains(hit.collider.gameObject))
                {
                    mouseRayHits = hit;
                }
            }
        }
        //OnMouseExit
        foreach (GameObject old in currentMouseHits)
        {
            //クリック可能オブジェクトからマウスが離れた場合
            if (!newHits.Contains(old))
            {
                mouseRayHits = new RaycastHit();
            }
        }

        //状態を更新
        currentMouseHits = newHits;
    }
}
