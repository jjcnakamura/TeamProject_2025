using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Heal : MonoBehaviour
{
    //弾のパラメーター
    int value;
    [SerializeField] float moveTime = 0.75f; //到達までの時間
    [SerializeField] float arcHeight = 4f;   //放物線の高さ

    //エフェクト
    GameObject effect;

    //弾の発射地点と向かう位置
    Vector3 startPos;
    Vector3 targetPos;

    //対象のユニット
    BattleUnit_Base targetUnit;

    //タイマー
    float timer_Shot;

    //状態を表すフラグ
    public bool isShot;

    /// <summary>
    /// 弾が射出される時に呼び出す
    /// </summary>
    public void Shot(int arg_Value, Vector3 arg_StartPos, Vector3 arg_TargetPos, BattleUnit_Base arg_TargetUnit, GameObject arg_Effect = null)
    {
        //パラメーターを読み込み
        value = arg_Value;

        //発射位置と目標地点を読み込み
        transform.position = arg_StartPos;
        startPos = arg_StartPos;
        targetPos = arg_TargetPos;

        //回復の対象を読み込み
        targetUnit = arg_TargetUnit;

        //エフェクトを読み込み
        if (arg_Effect != null) effect = arg_Effect;

        //フラグを設定
        isShot = true;
    }

    void FixedUpdate()
    {
        if (isShot)
        {
            timer_Shot += Time.fixedDeltaTime;
            float time = Mathf.Clamp01(timer_Shot / moveTime);

            //直線補間
            Vector3 pos = Vector3.Lerp(startPos, targetPos, time);

            //放物線成分（同じ軌道が常に維持される）
            float height = Mathf.Sin(time * Mathf.PI) * arcHeight;
            pos.y += height;

            transform.position = pos;

            //目標地点に到達(着弾)した場合は対象のユニットを回復
            if (time >= 1f)
            {
                //ユニットを回復
                if (targetUnit != null) targetUnit.Heal(value);

                //エフェクトを生成
                if (effect != null)
                {
                    GameObject effectObj = Instantiate(effect);
                    effectObj.transform.position = targetPos;
                    effectObj.transform.localScale = effect.transform.localScale;
                }

                isShot = false;

                //自身を消す
                Destroy(gameObject);
            }
        }
    }
}
