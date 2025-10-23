using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全ての敵が戦闘中に持つパラメーター
/// </summary>
public class Enemy_Base : MonoBehaviour
{
    [Header("Enemy_Base")]

    //Collider
    public BoxCollider col_Body;

    [Space(10)]

    //ゲーム中のパラメーター
    public int maxHp;
    public int hp;
    public int value;
    public float interval;
    public float distance;
    public float range;
    public float moveSpeed;
    public float knockBackTime;

    //タイマー
    float timer_KnockBack;

    //次に進む場所に関する変数
    public EnemySpawnPoint spawnPoint;
    public int routeIndex;
    public int currentRoute = 0;

    //向く方向に関する変数
    float rotateSpeed = 8f;
    Quaternion dir;

    //状態を表すフラグ
    public bool isMove, isRotation, isTarget, isKnockBack, isStatusChange, isDead;

    void Start()
    {
        isMove = true;
    }

    protected virtual void FixedUpdate()
    {
        KnockBack();
        DeadCheck();
        Move();
    }

    //ダメージ
    public bool Damage(int damage)
    {
        if (isDead) return true;

        hp = Mathf.Max(hp - damage, 0);
        timer_KnockBack = 0;
        isKnockBack = true;

        //HPが0になった場合死亡
        isDead = (hp <= 0);
        if (isDead)
        {
            col_Body.gameObject.SetActive(false);
            isMove = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    void KnockBack()
    {
        if (!isKnockBack) return;

        if (timer_KnockBack < knockBackTime)
        {
            timer_KnockBack += Time.fixedDeltaTime;

            //仮のダメージモーション
            transform.eulerAngles += new Vector3(0, 1000 * Time.fixedDeltaTime, 0);
        }
        else
        {
            //仮のダメージモーション
            Quaternion targetDir = Quaternion.LookRotation(spawnPoint.routePoint[routeIndex].pos[currentRoute] - transform.position);
            Quaternion lookDir = new Quaternion(transform.rotation.x, targetDir.y, transform.rotation.z, targetDir.w);
            DirectionChange(lookDir);

            timer_KnockBack = 0;
            isKnockBack = false;
        }
    }
    //死亡している場合は自身を削除する
    void DeadCheck()
    {
        if (isDead && !isKnockBack)
        {
            BattleManager.Instance.nowEnemyNum--;
            BattleManager.Instance.text_EnemyNum.text = BattleManager.Instance.nowEnemyNum.ToString();
            Destroy(gameObject);
        }
    }
    //プレイヤーの陣地に向かって移動する処理
    void Move()
    {
        if (isMove && !isKnockBack)
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
                    BattleManager.Instance.nowEnemyNum--;
                    BattleManager.Instance.text_EnemyNum.text = BattleManager.Instance.nowEnemyNum.ToString();
                    Destroy(gameObject);
                }
            }
        }

        //向きを変更
        if (isRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, dir, rotateSpeed);
        }
    }
    /// <summary>
    /// 次に向く報向を指定　第２引数をtrueにするとその位置を優先して向く
    /// </summary>
    public void DirectionChange(Quaternion targetDir, bool targetChange = false)
    {
        dir = targetDir;
        if (targetChange) isTarget = true;

        isRotation = true;
    }
}
