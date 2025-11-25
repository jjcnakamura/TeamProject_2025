using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit_SlowDebuff : BattleUnit_Base
{
    [Header("[BattleUnit_SlowDebuff]")]

    //速度を鈍化させている敵のコンポーネント
    List<Enemy_Base> debuffEnemy = new List<Enemy_Base>();

    //状態を表すフラグ
    public bool isStart, isDeadCheck_SlowDebuff;

    protected override void Update()
    {
        base.Update(); //基底クラスのUpdate

        if (!isBattle || !BattleManager.Instance.isMainGame) return; //戦闘中でない場合は戻る

        Place();
        DeadCheck_StatusChange();
    }

    //配置された時の処理
    void Place()
    {
        if (!isStart && isBattle)
        {
            //基底クラスで死亡の判定をしない
            isDeadCheck = false;
            isDeadCheck_SlowDebuff = true;
            isStart = true;
        }
    }

    //敵にデバフをかける
    public void SlowDebuff(Collider targetCol, bool flag)
    {
        Enemy_Base targetEnemy = targetCol.transform.parent.GetComponent<Enemy_Base>();
        if (targetEnemy == null) return;

        if (flag)
        {
            for (int i = debuffEnemy.Count - 1; i >= 0; i--)
            {
                if (debuffEnemy[i] == targetEnemy)
                {
                    return;
                }
            }

            float argValue = 1f / Mathf.Max((float)defaultValue, 1f);
            targetEnemy.SpeedChange(argValue, true);
            debuffEnemy.Add(targetEnemy);
        }
        else
        {
            float argValue = 1f / Mathf.Max((float)defaultValue, 1f);
            targetEnemy.SpeedChange(argValue, false);

            for (int i = debuffEnemy.Count - 1; i >= 0; i--)
            {
                if (debuffEnemy[i] == targetEnemy)
                {
                    debuffEnemy.RemoveAt(i);
                    break;
                }
            }
        }
    }

    //死亡している場合はデバフを解除
    void DeadCheck_StatusChange()
    {
        if (!isDeadCheck_SlowDebuff) return;

        if (isDead)
        {
            //デバフを解除
            for (int i = debuffEnemy.Count - 1; i >= 0; i--)
            {
                float argValue = 1f / Mathf.Max((float)defaultValue, 1f);
                debuffEnemy[i].SpeedChange(argValue, false);
            }

            isDead = true;
            isDeadCheck = true;
            isDeadCheck_SlowDebuff = false;
        }
    }
}
