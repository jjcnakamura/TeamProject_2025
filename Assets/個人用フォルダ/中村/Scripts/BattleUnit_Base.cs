using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全ユニットが戦闘中に持つパラメーター
/// </summary>
public class BattleUnit_Base : MonoBehaviour
{
    [Header("BattleUnit_Base")]

    //Collider
    public BoxCollider col_Body;

    [Space(10)]

    //ゲーム中のパラメーター
    public int zoneIndex;  //どこに配置されているか

    public int role;
    public int maxHp;
    public int hp;
    public int value;
    public float interval;
    public float distance;
    public float range;

    //向く方向に関する変数
    float rotateSpeed = 8f;
    Quaternion dir;

    //状態を表すフラグ
    public bool isBattle, isRotation, isTarget, isStatusChange, isDead;

    protected virtual void FixedUpdate()
    {
        if (!isBattle) return; //戦闘中でない場合は戻る

        DeadCheck();
        Rotate();
    }

    //ダメージ
    public bool Damage(int damage)
    {
        if (!isBattle) return false; //戦闘中でない場合は戻る
        if (isDead) return true;

        hp = Mathf.Max(hp - damage, 0);

        //HPが0になった場合死亡
        isDead = (hp <= 0);
        if (isDead)
        {
            col_Body.gameObject.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }
    //死亡している場合は自身を削除する
    void DeadCheck()
    {
        if (isDead)
        {
            BattleManager.Instance.OutUnit(zoneIndex);
        }
    }
    //狙っている敵の方向を向く処理
    void Rotate()
    {
        //向きを変更
        if (isRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, dir, rotateSpeed);
        }
    }

    /// <summary>
    /// 配置されていて削除される場合の処理
    /// </summary>
    public void Out()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 向く報向を指定　第２引数をtrueにするとその位置を優先して向く
    /// </summary>
    public void DirectionChange(Quaternion targetDir, bool targetChange = false)
    {
        dir = targetDir;
        if (targetChange)isTarget = true;

        isRotation = true;
    }
}
