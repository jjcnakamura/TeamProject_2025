using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit_Base : MonoBehaviour
{
    [Header("BattleUnit_Base")]

    //Collider
    public BoxCollider col_Body;

    [Space(10)]

    //ゲーム中のパラメーター
    public int zoneIndex;  //どこに配置されているか

    public int role;
    public int cost;
    public int recast;

    public int maxHp;
    public int hp;
    public int value;
    public float interval;
    public float distance;
    public float range;

    //タイマー
    float timer_Recast;

    //状態を表すフラグ
    public bool isBattle;

    //ダメージ
    public void Damage(int damage)
    {
        if (!isBattle) return; //戦闘中でない場合は戻る


    }

    /// <summary>
    /// 配置されていて削除される場合の処理
    /// </summary>
    public void Out()
    {
        Destroy(gameObject);
    }
}
