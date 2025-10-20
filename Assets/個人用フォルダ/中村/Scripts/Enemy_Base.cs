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
    public int routeIndex;
    int currentRoute = 0;

    //向く方向に関する変数
    float rotateSpeed = 8f;
    Quaternion dir;

    //状態を表すフラグ
    bool isMove = true, isRotation, isTarget;

    /// <summary>
    /// ダメージを受ける処理　引数でダメージ量を指定
    /// </summary>
    public void Damage(int damage)
    {

    }

    void FixedUpdate()
    {
        //プレイヤーの陣地に向かって移動する処理
        if (isMove)
        {
            Vector3 targetPos = new Vector3(spawnPoint.routePoint[routeIndex].pos[currentRoute].x, transform.position.y, spawnPoint.routePoint[routeIndex].pos[currentRoute].z);

            if (transform.position != targetPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                //次のルートポイントへ
                if (currentRoute < spawnPoint.routePoint[routeIndex].pos.Length - 1)
                {
                    currentRoute++;
                    //向きを変更
                    if (!isTarget)
                    {
                        Quaternion targetDir = Quaternion.LookRotation(spawnPoint.routePoint[routeIndex].pos[currentRoute] - transform.position);
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

    /// <summary>
    /// 次に向く報告を指定　第２引数をtrueにするとその位置を優先して向く
    /// </summary>
    public void DirectionChange(Quaternion targetDir, bool targetChange = false)
    {
        dir = targetDir;
        isTarget = targetChange;

        isRotation = true;
    }
}
