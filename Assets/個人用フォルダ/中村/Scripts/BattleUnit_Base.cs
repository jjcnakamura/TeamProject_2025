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
    public int defaultValue;
    public int value;
    public float interval;
    public float distance;
    public float range;

    //向く方向に関する変数
    float rotateSpeed = 8f;
    Quaternion dir;

    //バフ、デバフ用の変数
    List<int> buffValue = new List<int>();
    List<int> debuffValue = new List<int>();
    int maxBuffValue, minDebuffValue;
    int buffNum, deBuffNum;  

    //状態を表すフラグ
    public bool isBattle, isRotation, isTarget, isBuff, isDebuff, isDead;

    protected virtual void Update()
    {
        if (!isBattle || !BattleManager.Instance.isMainGame) return; //戦闘中でない場合は戻る

        DeadCheck();
    }

    protected virtual void FixedUpdate()
    {
        if (!isBattle || !BattleManager.Instance.isMainGame) return; //戦闘中でない場合は戻る

        Rotate();
    }

    //ダメージ
    public bool Damage(int damage)
    {
        if (!isBattle || !BattleManager.Instance.isMainGame) return false; //戦闘中でない場合は戻る
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
    //ステータスにバフかデバフをかける（同時にかかった場合は値が大きい方を優先）
    public void StatusChange(int val, bool flag)
    {
        //バフ
        if (val > 0)
        {
            //バフをかける
            if (flag)
            {
                //バフ数を追加
                buffNum++;
                buffValue.Add(val);

                //現在のバフ量より大きい場合
                if (val > maxBuffValue)
                {
                    maxBuffValue = val;
                    value = Mathf.Max(defaultValue + val + minDebuffValue, 1);
                }

                isBuff = true;
            }
            //バフを解除する
            else if (buffNum > 0)
            {
                //現在のバフ量と等しい場合は新しく参照するバフ量を決める
                bool isMax = (val >= maxBuffValue);
                bool isRemove = false;
                int maxVal = 0;

                //バフ数を減らす
                buffNum--;
                for (int i = buffValue.Count - 1; i >= 0; i--)
                {
                    if (isMax && buffValue[i] <= maxVal) maxVal = buffValue[i];

                    if (buffValue[i] == val)
                    {
                        if (!isRemove && isMax)
                        {
                            buffValue.RemoveAt(i);
                            isRemove = true;
                        }
                        else if (!isMax)
                        {
                            buffValue.RemoveAt(i);
                            break;
                        }
                    }
                }
                //新しく参照するバフ量を決める
                if (isMax)
                {
                    maxBuffValue = maxVal;
                    value = Mathf.Max(value + val, 1);
                }
                //バフ数が0になった場合はステータスを戻す
                if (buffNum <= 0)
                {
                    maxBuffValue = 0;
                    value = Mathf.Max(defaultValue + val + minDebuffValue, 1);

                    isBuff = false;
                }
            }
        }
        //デバフ
        if (val < 0)
        {
            //デバフをかける
            if (flag)
            {
                //デバフ数を追加
                deBuffNum++;
                debuffValue.Add(val);

                //現在のデバフ量より大きい場合
                if (val < minDebuffValue)
                {
                    minDebuffValue = val;
                    value = Mathf.Max(defaultValue + val + maxBuffValue, 1);
                }

                isDebuff = true;
            }
            //デバフを解除する
            else if (deBuffNum > 0)
            {
                //現在のデバフ量と等しい場合は新しく参照するデバフ量を決める
                bool isMin = (val <= minDebuffValue);
                bool isRemove = false;
                int minVal = 0;

                //デバフ数を減らす
                deBuffNum--;
                for (int i = debuffValue.Count - 1; i >= 0; i--)
                {
                    if (isMin && debuffValue[i] <= minVal) minVal = debuffValue[i];

                    if (debuffValue[i] == val)
                    {
                        if (!isRemove && isMin)
                        {
                            debuffValue.RemoveAt(i);
                            isRemove = true;
                        }
                        else if (!isMin)
                        {
                            debuffValue.RemoveAt(i);
                            break;
                        }
                    }
                }
                //新しく参照するデバフ量を決める
                if (isMin)
                {
                    minDebuffValue = minVal;
                    value = Mathf.Max(defaultValue + val + maxBuffValue, 1);
                }
                //デバフ数が0になった場合はステータスを戻す
                if (deBuffNum <= 0)
                {
                    minDebuffValue = 0;
                    value = defaultValue + maxBuffValue;

                    isDebuff = false;
                }
            }
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
