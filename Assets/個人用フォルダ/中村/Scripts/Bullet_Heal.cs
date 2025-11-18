using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Heal : MonoBehaviour
{
    //弾のパラメーター
    int value;
    float speed = 10f;

    //エフェクト
    GameObject effect;

    //弾の向かう位置
    Vector3 targetPos;

    //対象のユニット
    BattleUnit_Base targetUnit;

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
            //目標地点に向かう
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);

            //目標地点に到達(着弾)した場合は対象のユニットを回復
            if (transform.position == targetPos)
            {
                //ユニットを回復
                if (targetUnit != null) targetUnit.Heal(value);

                //エフェクトを生成
                if (effect != null)
                {
                    GameObject effectObj = Instantiate(effect);
                    effectObj.transform.position = transform.position;
                    effectObj.transform.localScale = effect.transform.localScale;
                }

                isShot = false;

                //自身を消す
                Destroy(gameObject);
            }
        }
    }
}
