using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    //ゲーム中のパラメーター
    public int maxHp;
    public int hp;
    public int value;
    public float interval;
    public float distance;
    public float range;
    public float moveSpeed;

    //次に進む場所に関する変数
    public EnemySpawnPoint spawnPoint;
    int routeIndex = 0;

    //向く方向に関する変数
    float rotateSpeed = 8f;
    Quaternion dir;

    //状態を表すフラグ
    bool isMove = true, isRotation, isTarget;

    void FixedUpdate()
    {
        //プレイヤーの陣地に向かって移動する処理
        if (isMove)
        {
            Vector3 targetPos = new Vector3(spawnPoint.routePoint[routeIndex].x, transform.position.y, spawnPoint.routePoint[routeIndex].z);

            if (transform.position != targetPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime);
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
                        Quaternion targetDir = Quaternion.LookRotation(spawnPoint.routePoint[routeIndex] - transform.position);
                        Quaternion lookDir = new Quaternion(transform.rotation.x, targetDir.y, transform.rotation.z, targetDir.w);
                        DirectionChange(lookDir);
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

        //向きを変更（動かない）
        if (isRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, dir, rotateSpeed);
        }
    }

    public void DirectionChange(Quaternion targetDir, bool targetChange = false)
    {
        dir = targetDir;
        isTarget = targetChange;

        isRotation = true;
    }
}
