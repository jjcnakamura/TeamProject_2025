using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_SpeedDebuff : MonoBehaviour
{
    [SerializeField] CapsuleCollider col; //攻撃判定のCollider
    [SerializeField] MeshRenderer mesh;   //MeshRenderer

    //弾のパラメーター
    float value;
    float time;
    float range;
    float speed = 20f;

    //エフェクト
    GameObject effect;

    //弾の向かう位置
    Vector3 targetPos;

    //攻撃が当たった敵のリスト
    List<Collider> hitEnemy = new List<Collider>();

    //タイマー
    [System.NonSerialized] public float explosionTime = 0.5f; //爆発の持続時間
    float timer_Explosion;

    //状態を表すフラグ
    public bool isShot, isExplosion;

    /// <summary>
    /// 弾が射出される時に呼び出す
    /// </summary>
    public void Shot(float arg_Value, float arg_Time, float arg_Range, Vector3 arg_StartPos, Vector3 arg_TargetPos, GameObject arg_Effect = null)
    {
        mesh.enabled = true;
        col.enabled = false;

        //パラメーターを読み込み
        value = arg_Value;
        time = arg_Time;
        range = arg_Range;

        //発射位置と目標地点を読み込み
        transform.position = arg_StartPos;
        targetPos = arg_TargetPos;

        //エフェクトを読み込み
        if (arg_Effect != null) effect = arg_Effect;

        //フラグを設定
        isShot = true;
        isExplosion = false;
    }

    void FixedUpdate()
    {
        if (isShot)
        {
            //目標地点に向かう
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);

            //目標地点に到達(着弾)した場合は判定を出す
            if (transform.position == targetPos)
            {
                mesh.enabled = false;
                col.enabled = true;

                //エフェクトを生成
                if (effect != null)
                {
                    GameObject effectObj = Instantiate(effect);
                    effectObj.transform.position = transform.position;
                    effectObj.transform.localScale = new Vector3(range, range, range);
                }

                transform.localScale = new Vector3(range, transform.localScale.y, range);

                isShot = false;
                isExplosion = true;
            }
        }
        else if (isExplosion)
        {
            //攻撃判定の持続をカウント
            if (timer_Explosion < explosionTime)
            {
                timer_Explosion += Time.fixedDeltaTime;
            }
            else
            {
                col.enabled = false;

                isShot = false;
                isExplosion = false;

                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isExplosion) return;

        //敵に当たった場合
        if (other.transform.tag == "Enemy")
        {
            //既に攻撃が当たった敵か判定する
            if (hitEnemy.Count > 0)
            {
                for (int i = 0; i < hitEnemy.Count; i++)
                {
                    //既に攻撃が当たっていた場合は戻る
                    if (hitEnemy[i] != null && other == hitEnemy[i])
                        return;
                }
            }
            Enemy_Base enemy = other.transform.parent.GetComponent<Enemy_Base>();
            if (enemy != null)
            {
                //敵に速度デバフをかける
                enemy.SpeedChange(value, time);
                //敵をリストに加える
                hitEnemy.Add(other);
            }
        }
    }
}
