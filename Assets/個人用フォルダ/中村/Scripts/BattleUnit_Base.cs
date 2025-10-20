using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit_Base : MonoBehaviour
{
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

    /// <summary>
    /// 配置されている場合に削除される処理
    /// </summary>
    public void Out()
    {
        Destroy(gameObject);
    }
}
