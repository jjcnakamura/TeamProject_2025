using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit_PointUp : BattleUnit_Base
{
    [Header("BattleUnit_PointUp")]

    //�^�C�}�[
    float timer_Interval;

    protected override void FixedUpdate()
    {
        base.FixedUpdate(); //���N���X��FixedUpdate

        if (!isBattle || !BattleManager.Instance.isMainGame) return; //�퓬���łȂ��ꍇ�͖߂�

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
            BattleManager.Instance.PointChange(defaultValue);
            timer_Interval = 0;
        }
    }
}
