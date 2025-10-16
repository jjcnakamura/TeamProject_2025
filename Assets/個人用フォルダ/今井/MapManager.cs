using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public ParameterManager.UnitStatus unitStatus;
    public GameObject MapImage;

    
    public int[] Stageint;//�X�e�[�W��
    public int[] MapSelectBlock;//����H��

    public int[] UnitUpGold;//�K�v�S�[���h��
    public int Gold;//(��)

    public GridLayoutGroup grid1;//GridLayoutGroup�̎w���
    public GridLayoutGroup grid2;//GridLayoutGroup�̎w�蒆
    public GridLayoutGroup grid3;//GridLayoutGroup�̎w�艺
    public float X = 0f;//GridLayoutGroup��spacing��ς��ăX�e�[�W�����߂����

    void Update()
    {
        if (Stageint[0] >= 7)
        {
            int BlockX = Stageint[0] - 7;
            BlockX -= 5;
        }
        grid1.spacing = new Vector2(X, 0);
        if (Stageint[1] >= 7)
        {
            int BlockX = Stageint[1] - 7;
            BlockX -= 5;
        }
        grid2.spacing = new Vector2(X, 0);
        if (Stageint[2] >= 7)
        {
            int BlockX = Stageint[2] - 7;
            BlockX -= 5;
        }
        grid3.spacing = new Vector2(X, 0);
    }
    

    public void EventContDown()//�ݒu���̃R�X�g�@(�Z�k�n)
    {
        unitStatus.cost -= 1;//�i���j
    }
    public void EventrecastDown()//�Ĕz�u�܂ł̎��ԁ@(�Z�k�n)
    {
       unitStatus.recast -= 1;//�i���j
    }
    public void EventmaxInstallationUp()//���j�b�g�ő�z�u���@(�����n)
    {
        ParameterManager.Instance.maxInstallation += 1;//�i���j
    }
    public void EventmaxUnitPossessionUp()//�ő僆�j�b�g�������@(�����n)
    {
        ParameterManager.Instance.maxUnitPossession += 1;//�i���j
    }
    public void EventsameUnitMaxInstallationUp()//�������j�b�g�̍ő�z�u���@(�����n)
    {
        ParameterManager.Instance.sameUnitMaxInstallation += 1;//�i���j
    }
    public void Event()//HP�񕜂���i�񕜁j
    {
        //�i���j
    }
    public void UnitLevelUpBottun()
    {
        if (Gold >= UnitUpGold[unitStatus.lv])
        {
            Gold -= UnitUpGold[unitStatus.lv];
            unitStatus.lv += 1;
            unitStatus.hp += 1;
            unitStatus.value += 1;
            unitStatus.interval += 1;
            unitStatus.distance += 1;
            unitStatus.range += 1;
            
        }
        if(Gold < UnitUpGold[unitStatus.lv])
        {
            //�����e�L�X�g���o���ďo���Ȃ����Ƃ����߂�
        }
    }
}
