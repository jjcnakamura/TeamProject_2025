using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    public int maxHp;
    public int hp;
    public int value;
    public float interval;
    public float distance;
    public float range;
    public float moveSpeed;

    public EnemySpawnPoint spawnPoint;
    int routeIndex = 0;

    bool isMove = true, isTarget;

    void FixedUpdate()
    {
        //プレイヤーの陣地に向かって移動する処理
        if (isMove)
        {
            Vector3 nextPos = new Vector3(spawnPoint.routePoint[routeIndex].x, transform.position.y, spawnPoint.routePoint[routeIndex].z);

            if (transform.position != nextPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPos, moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                //次のルートポイントへ
                if (routeIndex < spawnPoint.routePoint.Length - 1)
                {
                    routeIndex++;
                    //向きを変更
                    if (!isTarget)
                    {

                    }
                }
                //プレイヤーの陣地に到達した場合
                else
                {
                    //プレイヤーにダメージを与えて消滅
                    BattleManager.Instance.Damage();
                    Destroy(gameObject);
                }
            }
        }
    }
}
