using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit_PointUp : BattleUnit_Base
{
    [Header("BattleUnit_PointUp")]

    //�^�C�}�[
    float timer_Interval;

    void FixedUpdate()
    {
        if (!isBattle) return; //�퓬���łȂ��ꍇ�͖߂�

        PointUp();
    }

    //�|�C���g�㏸
    void PointUp()
    {
        if (timer_Interval < interval)
        {
            timer_Interval += Time.fixedDeltaTime;
        }
        else
        {
            //�|�C���g���㏸���ăC���^�[�o���J�n
            BattleManager.Instance.PointChange(value);
            timer_Interval = 0;
        }
    }
}
